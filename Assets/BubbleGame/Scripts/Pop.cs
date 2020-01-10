using System;
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
        if (bubble.ColorCode == colorCode) bubble.Pop(true);
    }
}
