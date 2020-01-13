using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class BubbleGameLevelParameters {
    [SerializeField] float batchesPerBeat;
    [SerializeField] float bubblesPerBeatMean;
    [SerializeField] float bubblesPerBeatStdev;
    [SerializeField] float bubbleSpeed;
    [SerializeField] float neutralChance;

    public BubbleGameLevelParameters() { }

    public BubbleGameLevelParameters(float batchesPerBeat, float bubblesPerBeatMean, float bubblesPerBeatStdev, float bubbleSpeed, float neutralChance) {
        this.batchesPerBeat = batchesPerBeat;
        this.bubblesPerBeatMean = bubblesPerBeatMean;
        this.bubblesPerBeatStdev = bubblesPerBeatStdev;
        this.bubbleSpeed = bubbleSpeed;
        this.neutralChance = neutralChance;
    }

    public float GetBatchInterval(float bpm) => 60f / (bpm * batchesPerBeat);
    public float GetRandomBatchSize() {
        return MathUtil.BoxMuller(
            bubblesPerBeatMean / batchesPerBeat,
            Mathf.Sqrt(batchesPerBeat) * bubblesPerBeatStdev,
            UnityEngine.Random.value,
            UnityEngine.Random.value
        );
    }

    public bool GetRandomIsNeutral() => UnityEngine.Random.value <= neutralChance;

    public float BatchesPerBeat => batchesPerBeat;
    public float BubbleSpeed => bubbleSpeed;
}