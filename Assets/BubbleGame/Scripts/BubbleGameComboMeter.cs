using UnityEngine;
using UniRx;
using TMPro;

public class BubbleGameComboMeter : MonoBehaviour {
    [SerializeField] TextMeshPro displayText;

    [SerializeField] BubbleGameScoreTracker scores;

    void Start() {
        scores.Heat.Subscribe(v => displayText.text = $"{v:n3}");
        //scores.Combo.Subscribe(v => displayText.text = $"x{v}").AddTo(this);
    }
}
