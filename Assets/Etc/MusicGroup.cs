using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BubbleGame/MusicGroup")]
public class MusicGroup : ScriptableObject {
    public AudioClip backgroundAmbience;
    public AudioClip mainTrack;
    public AudioClip guideBeat;
    public AudioClip mainTrackLowEQ;
    public AudioClip mainTrackHighEQ;
    public int bpm;

    internal AudioClip GetTrackForLabel(MusicPlayer.LabeledAudioSource.Label label) {

        switch (label) {
            case MusicPlayer.LabeledAudioSource.Label.Main:
                return mainTrack;
            case MusicPlayer.LabeledAudioSource.Label.GuideBeat:
                return guideBeat;
            case MusicPlayer.LabeledAudioSource.Label.HighEQ:
                return mainTrackHighEQ;
            case MusicPlayer.LabeledAudioSource.Label.LowEQ:
                return mainTrackLowEQ;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
