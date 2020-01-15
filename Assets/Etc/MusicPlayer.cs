using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

public class MusicPlayer : MonoBehaviour
{
    [Serializable] public class LabeledAudioSource {
        public enum Label {
            Main,
            GuideBeat,
            LowEQ,
            HighEQ
        }

        public Label label;
        public AudioSource source;
    }

    [SerializeField] public List<LabeledAudioSource> sources;
    [SerializeField] PlayableDirector fadeOut;

    [SerializeField] MusicManager musicManager;

    public bool resetFlag = false;
    public float volume = 1f;
    private float knownVolume;

    private MusicGroup current;

    private void Start() {
        musicManager.SelectedMusicGroup.Subscribe(OnSongChanged).AddTo(this);
    }

    private void Update() {
        if (knownVolume != volume) ApplyVolume(volume);
        if(resetFlag) {
            resetFlag = false;
            StartAllPlaying();
        }
    }

    private void ApplyVolume(float next) {
        this.knownVolume = next;
        foreach (var lsrc in sources) {
            lsrc.source.volume = next;
        }
    }


    void OnSongChanged(MusicGroup next) {
        var off = current == null;
        current = next;
        if (off) {
            StartAllPlaying();
        } else {
            fadeOut.Play();
        }
    }

    private void StartAllPlaying() {
        volume = 1;
        knownVolume = 1;
        foreach(var lsrc in sources) {
            lsrc.source.Stop();
            lsrc.source.clip = current.GetTrackForLabel(lsrc.label);
            lsrc.source.time = 0;
            lsrc.source.volume = 1;
            lsrc.source.Play();
        }
    }
}
