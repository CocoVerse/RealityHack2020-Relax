using UnityEngine;
using UniRx;
using TMPro;

public class BubbleGameLevelDisplay : MonoBehaviour {
    [SerializeField] TextMeshPro displayText;

    [SerializeField] BubbleGameScoreTracker scores;

    void Start() {
        scores.Level.Subscribe(v => displayText.text = $"LEVEL {v+ 1}").AddTo(this);
    }
}
