﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BubbleGame/AutoLevelGenerator")]
public class AutoLevelGenerator : ScriptableObject
{

    [SerializeField] float difficultyBase = 1.08f;


    public BubbleGameLevelParameters GetLevel(int index) {
        var raw_difficulty = Mathf.Pow(difficultyBase, index) - 1f;

        var speed = 0.8f* raw_difficulty + 0.8f;
        var bubble_clearance = 1.5f / (1f + raw_difficulty) + 0.5f;


        var batchesPerBeat_raw = speed / bubble_clearance;
        var batchesPerBeat = Mathf.Pow(2, Mathf.Floor(Mathf.Log(batchesPerBeat_raw, 2)));


        var mean = 0.5f + 0.6f * raw_difficulty;
        var stdev = 0.5f + raw_difficulty * 0.2f;
        return new BubbleGameLevelParameters(batchesPerBeat, mean, stdev, speed);
    }
}
