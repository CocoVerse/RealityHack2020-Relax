using UnityEngine;
using UniRx;
using TMPro;

public class BubbleGameComboMeter : MonoBehaviour {
    [SerializeField] TextMeshPro displayText;

    [SerializeField] BubbleGameScoreTracker scores;

    void Start() {
        scores.Combo.Subscribe(v => displayText.text = $"x{v}").AddTo(this);
    }
}
