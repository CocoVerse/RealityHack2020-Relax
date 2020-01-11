using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class BubbleGameScoreTracker : MonoBehaviour {
    [SerializeField] float scoreDrainRate = 0.1f;

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
    }

    private void DrainScore() {
        rawScore.Value = Mathf.Max(0, rawScore.Value - Time.deltaTime * scoreDrainRate);
    }
}

public class BubbleGameComboMeter : MonoBehaviour {
    [SerializeField] TextMeshPro displayText;

    [SerializeField] BubbleGameScoreTracker scores;

    void Start() {
        scores.Combo.Subscribe(v => displayText.text = $"x{v}").AddTo(this);
    }
}

public class BubbleGameVolumeController : MonoBehaviour {
    [SerializeField] AudioSource source;
    [SerializeField] BubbleGameScoreTracker scores;

    void Start() {
        scores.MainTrackVolume.Subscribe(v => source.volume = v).AddTo(this);
    }
}