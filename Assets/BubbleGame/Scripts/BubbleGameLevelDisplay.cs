using UnityEngine;
using UniRx;
using TMPro;
using System.Collections;

public class BubbleGameLevelDisplay : MonoBehaviour {
    [SerializeField] TextMeshPro currentLevelText;
    [SerializeField] GameObject sessionScoreGroup;
    [SerializeField] TextMeshPro sessionScoreText;

    [SerializeField] BubbleGameStateManager state;
    [SerializeField] BubbleGameScoreTracker scores;

    Coroutine scoreTimeOutCoroutine;

    bool wasEverActive = false;

    void Start() {
        state.IsRunning.Subscribe(ApplyRunningState).AddTo(this);
        scores.Level.Subscribe(v => currentLevelText.text = $"LEVEL {v+ 1}").AddTo(this);
        scores.CurrentSessionScore.Subscribe(v => sessionScoreText.text = $"{v:n0}");
    }

    void ApplyRunningState(bool isRunning) {
        if (isRunning) wasEverActive = true;
        

        if (scoreTimeOutCoroutine != null) StopCoroutine(scoreTimeOutCoroutine);

        currentLevelText.gameObject.SetActive(isRunning);
        sessionScoreGroup.gameObject.SetActive(!isRunning && wasEverActive);

        if (!isRunning && wasEverActive) scoreTimeOutCoroutine = StartCoroutine(VanishScoreDisplay());
    }

    IEnumerator VanishScoreDisplay() {
        yield return new WaitForSeconds(30f);
        sessionScoreGroup.gameObject.SetActive(false);
    }


}
