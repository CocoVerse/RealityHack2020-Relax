using UnityEngine;
using UniRx;

public class BubbleGameVolumeController : MonoBehaviour {
    [SerializeField] AudioSource source;
    [SerializeField] BubbleGameScoreTracker scores;

    void Start() {
        scores.MainTrackVolume.Subscribe(v => source.volume = v).AddTo(this);
    }
}