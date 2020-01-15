using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOccasionalNoise : MonoBehaviour
{
    [SerializeField] AudioSource target;
    [SerializeField] float intervalMean;
    [SerializeField] float intervalStdev;
    [SerializeField] float pitchMean;
    [SerializeField] float pitchStdev;

    [SerializeField] WeightedSoundGroup sounds;

    IEnumerator Start() {
        while(true) {
            yield return new WaitForSeconds(Mathf.Max(0.1f, MathUtil.BoxMuller(intervalMean, intervalStdev, Random.value,Random.value)));
            Next();
            yield return new WaitForSeconds(target.clip.length);
        }
    }

    void Next() {
        target.Stop();
        target.pitch = MathUtil.BoxMuller(pitchMean, pitchStdev, Random.value, Random.value);
        target.clip = sounds.GetClipAt(Random.value);
        target.Play();
    }
}
