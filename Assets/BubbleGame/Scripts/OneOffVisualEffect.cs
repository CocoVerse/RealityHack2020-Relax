using UnityEngine;
using UnityEngine.VFX;

public class OneOffVisualEffect : MonoBehaviour {
    [SerializeField] float lifespan;

    [SerializeField] VisualEffect vfx;

    private void Start() {
        Destroy(gameObject, lifespan);
        vfx.Play();
    }

}
