using UnityEngine;
using UniRx;

public class ExponentialTracker {
    public ReactiveProperty<float> Score { get; }

    private float decayPerSecond; // decrease per second

    public ExponentialTracker(float initialValue, float decayPerSecond) {
        this.Score = new ReactiveProperty<float>(initialValue);
        this.decayPerSecond = decayPerSecond;
    }

    public void Put(float x) => Score.Value = x;

    public void Push(float x, float deltaT) {
        var retention = Mathf.Pow(decayPerSecond, deltaT);
        Score.Value =  retention* Score.Value + (1f - retention) * x;
    }
}