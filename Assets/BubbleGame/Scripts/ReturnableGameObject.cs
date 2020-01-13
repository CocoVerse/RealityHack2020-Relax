using System;
using UnityEngine;

public class ReturnableGameObject : MonoBehaviour, IReturnable {
    private Action returnAction;

    private bool activated = false;

    public void Activate(Action returnAction) {
        if (activated) throw new InvalidOperationException();
        activated = true;
        gameObject.SetActive(true);
        this.returnAction = returnAction;
    }

    public void Deactivate() {
        if (!activated) return;
        activated = false;
        gameObject.SetActive(false);
        returnAction();
    }

    public void Dispose() {
        Destroy(gameObject);
    }
}
