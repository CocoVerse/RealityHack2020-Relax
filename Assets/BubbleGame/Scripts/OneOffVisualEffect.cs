using UnityEngine;
using UnityEngine.VFX;

public class OneOffVisualEffect : MonoBehaviour {
    [SerializeField] VisualEffect vfx;

    private void OnEnable() {
        vfx.Play();
    }

    private void OnDisable() {
        vfx.Stop();
    }

}
