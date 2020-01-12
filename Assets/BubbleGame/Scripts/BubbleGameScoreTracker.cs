using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BubbleGameScoreTracker : MonoBehaviour {
    [SerializeField] float scoreDrainGravity = 1f;

    private float scoreDrainRate = 0f;

    public ReactiveProperty<int> Combo { get; } = new ReactiveProperty<int>(0);

    private ReactiveProperty<float> rawScore = new ReactiveProperty<float>(0);

    private IObservable<float> _mainTrackVolume;
    public IObservable<float> MainTrackVolume => _mainTrackVolume ?? (_mainTrackVolume = rawScore.Select(v => Mathf.SmoothStep(0, 1, v)));

    void Update() {
        DrainScore();
    }

    public void NotifyHit() {
        Combo.Value = Combo.Value + 1;
        PumpScore();
    }
    


    public void NotifyMiss() {
        Combo.Value = 0;
    }

    private void PumpScore() {
        rawScore.Value = rawScore.Value + 0.1f;
        scoreDrainRate = 0;
    }

    private void DrainScore() {
        scoreDrainRate += scoreDrainGravity * Time.deltaTime;

        var nextScore = rawScore.Value - Time.deltaTime * scoreDrainRate;
        if(nextScore>0) {
            rawScore.Value = nextScore;
        } else {
            rawScore.Value = 0;
            scoreDrainRate = 0;
        }
    }
}
