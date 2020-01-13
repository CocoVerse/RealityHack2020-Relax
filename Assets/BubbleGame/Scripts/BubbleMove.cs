using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class BubbleMove : MonoBehaviour {
    [SerializeField] BubbleColorCode colorCode;

    public BubbleColorCode ColorCode => colorCode;

    [SerializeField] float travelBeforePop = 5f;
    [SerializeField] Animator growAnimation;
    
    private IDisposable disposable;

    public BubbleGameScoreTracker ScoreTracker { get; set; }
    public Func<BubblePopEffect> PopEffectGetter { get; set; }

    public float Speed { get; internal set; } = 1f;

    private float travelDistance = 0;

    // Start is called before the first frame update
    void Start() {
        if (ScoreTracker != null) {
            Action clearPop = () => Pop(BubblePopCategory.Other);
            ScoreTracker.OnClearBubbles += clearPop;
            this.disposable = Disposable.Create(() => ScoreTracker.OnClearBubbles -= clearPop);
        }
        growAnimation.speed = Speed;
    }

    private void OnDestroy() {
        disposable?.Dispose();
    }

    internal void Pop(BubblePopCategory category) {
        if (PopEffectGetter != null) {
            var obj = PopEffectGetter();
            obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            obj.transform.localScale = transform.localScale;
        }
        ScoreTracker?.NotifyPop(category);
        if (category == BubblePopCategory.Hit && colorCode == BubbleColorCode.LevelUp) ScoreTracker?.LevelUp();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update() {
        var delta = Time.deltaTime * Speed;
        transform.position += transform.forward * delta;
        travelDistance += delta;
        if (travelDistance > travelBeforePop) Pop(BubblePopCategory.Miss);
    }
}