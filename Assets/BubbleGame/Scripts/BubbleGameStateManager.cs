using UnityEngine;
using UniRx;
using System;
using System.Collections.Generic;

public class BubbleGameStateManager : MonoBehaviour {
    public ReactiveProperty<bool> IsRunning { get; } = new ReactiveProperty<bool>(false);

    private HashSet<GlowStick> heldGlowSticks = new HashSet<GlowStick>();

    internal void NotifyPickUp(GlowStick glowStick) {
        heldGlowSticks.Add(glowStick);
        RefreshState();
    }

    internal void NotifyPutDown(GlowStick glowStick) {
        heldGlowSticks.Remove(glowStick);
        RefreshState();
    }

    private void RefreshState() {
        if(IsRunning.Value) {
            if(heldGlowSticks.Count == 0) {
                IsRunning.Value = false;
            }
        } else {
            if(heldGlowSticks.Count>1) {
                IsRunning.Value = true;
            }
        }
    }
}
