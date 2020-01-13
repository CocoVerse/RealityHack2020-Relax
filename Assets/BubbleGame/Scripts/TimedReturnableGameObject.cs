using System.Collections;
using UnityEngine;

public class TimedReturnableGameObject : ReturnableGameObject {
    [SerializeField] float lifespan;
    private Coroutine coroutine;

    private void OnEnable() {
        coroutine = StartCoroutine(DeactivateAfter(lifespan));
    }

    private void OnDisable() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator DeactivateAfter(float lifespan) {
        yield return new WaitForSeconds(lifespan);
        Deactivate();
    }
}
