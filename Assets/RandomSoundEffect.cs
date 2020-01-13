using System;
using System.Collections;
using UnityEngine;

public class RandomSoundEffect : MonoBehaviour
{
    [SerializeField] WeightedSoundGroup group;
    [SerializeField] AudioSource src;
    [SerializeField] float pitchStdev = 0.25f;

    private void OnEnable() {
        src.clip = group.GetClipAt(UnityEngine.Random.value);
        src.pitch = MathUtil.BoxMuller(1f, pitchStdev, UnityEngine.Random.value, UnityEngine.Random.value);
        src.Play();
    }

    private void OnDisable() {
        src.Stop();
    }
}
