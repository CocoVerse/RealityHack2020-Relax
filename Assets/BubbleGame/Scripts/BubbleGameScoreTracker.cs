using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class BubbleGameScoreTracker : MonoBehaviour {
    // Increase to average the accuracy over a longer time.
    private const float ACCURACY_TRACKER_WINDOW = 3f;

    // Increase toward 1 to decrease the change in score over time.
    private const float SCORE_DECAY_FACTOR = 0.8f;
    private const float LEVELUP_BUBBLE_TIME = 5f;
    private const float MUSIC_DAMPEN_BELOW = 0.25f;
    private const float INITIAL_SCORE = 0.0f;
    private const float LEVEL_RESET_SCORE = 0.25f;


    [SerializeField] float levelRegressAt = 0.2f;

    [SerializeField] BubbleGameStateManager stateManager;

    private int level;

    private float scoreDrainRate = 0f;

    public ReactiveProperty<int> Combo { get; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> Level { get; } = new ReactiveProperty<int>(0);
    
    public event Action OnClearBubbles;

    public bool IsReadyForLevelUp => levelUpReadyTracker.Ready(Time.time);

    private ExponentialTracker _scoreMachine;
    private ExponentialTracker ScoreMachine => _scoreMachine ?? (_scoreMachine = new ExponentialTracker(INITIAL_SCORE, SCORE_DECAY_FACTOR));
    public IObservable<float> Heat =>  ScoreMachine.Score;

    private IObservable<float> _mainTrackVolume;
    public IObservable<float> MainTrackVolume => _mainTrackVolume ?? (_mainTrackVolume = ScoreMachine.Score.Select(v => Mathf.SmoothStep(0, 1, v / MUSIC_DAMPEN_BELOW)));

    RollingAverage accuracyTracker = new RollingAverage(ACCURACY_TRACKER_WINDOW);
    ConditionMetForDurationTracker levelUpReadyTracker = new ConditionMetForDurationTracker(LEVELUP_BUBBLE_TIME);

    private ReactiveProperty<int> highestScore = new ReactiveProperty<int>(0);
    public IObservable<int> CurrentSessionScore => highestScore;

    private bool doScoreUpdate = false;

    private void Start() {
        stateManager.IsRunning.Subscribe(ApplyRunningState).AddTo(this);
        Level.CombineLatest(Heat, ComputeMomentaryScore).Subscribe(CheckHighestScore).AddTo(this);
    }

    void CheckHighestScore(int score) {
        if (highestScore.Value < score) highestScore.Value = score;
    }

    static int ComputeMomentaryScore(int level, float heat) {
        return Mathf.CeilToInt(1000f * (level + heat));
    }

    void Update() {
        if (doScoreUpdate) {
            UpdateScore();
            if(Input.GetKeyDown(KeyCode.LeftBracket)) {
                LevelDown();
            }
            if(Input.GetKeyDown(KeyCode.RightBracket)) {
                LevelUp();
            }
        }
    }

    void ApplyRunningState(bool isRunning) {
        this.doScoreUpdate = isRunning;
        if(isRunning) {
            ResetScores();
        } else {
            OnClearBubbles?.Invoke();
        }
    }

    void ResetScores() {
        Level.Value = 1;
        levelUpReadyTracker.No();
        accuracyTracker.Clear();
        highestScore.Value = 0;
        ScoreMachine.Put(INITIAL_SCORE);
        Level.Value = 0;
        OnClearBubbles?.Invoke();
    }

    public void NotifyPop(BubblePopCategory category) {
        switch (category) {
            case BubblePopCategory.Hit:
                accuracyTracker.Push(Time.time, 1);
                Combo.Value = Combo.Value + 1;
                break;
            case BubblePopCategory.Miss:
                accuracyTracker.Push(Time.time, 0);
                Combo.Value = 0;
                break;
            case BubblePopCategory.Other:
                break;
        }
    }

    public void ClearLevelUpAvailability() => levelUpReadyTracker.No();

    private void UpdateScore() {
        accuracyTracker.Crop(Time.time);
        if (accuracyTracker.TryGetAverage(out var accuracy)) ScoreMachine.Push(accuracy, Time.deltaTime);

        if(ScoreMachine.Score.Value < levelRegressAt) {
            LevelDown();
        }
        
        if(ScoreMachine.Score.Value >= 0.8f) {
            levelUpReadyTracker.Yes(Time.time);
        } else {
            levelUpReadyTracker.No();
        }
    }

    public void LevelDown() {
        if(Level.Value > 0) SetLevel(Level.Value - 1);
    }

    public void LevelUp() {
        SetLevel(Level.Value + 1);
    }

    private void SetLevel(int next) {
        levelUpReadyTracker.No();
        accuracyTracker.Clear();
        ScoreMachine.Put(LEVEL_RESET_SCORE);
        Level.Value = next;
        OnClearBubbles?.Invoke();
    }
    
}
