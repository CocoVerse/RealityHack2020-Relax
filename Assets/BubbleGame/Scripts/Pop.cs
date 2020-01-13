﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pop : MonoBehaviour
{
    [SerializeField] BubbleColorCode colorCode;

    private void OnTriggerEnter(Collider other) {
        var bubble = other.GetComponent<BubbleMove>();
        if (bubble != null) HitBubble(bubble);

    }

    private void HitBubble(BubbleMove bubble) {
        if (AreCompatible(colorCode, bubble.ColorCode)) bubble.Pop(BubblePopCategory.Hit);
    }

    private static bool AreCompatible(BubbleColorCode a, BubbleColorCode b) {
        switch (a) {
            case BubbleColorCode.Pink:
                return b != BubbleColorCode.Yellow;
            case BubbleColorCode.Yellow:
                return b != BubbleColorCode.Pink;
            default:
                throw new ArgumentException();
        }
    }
}