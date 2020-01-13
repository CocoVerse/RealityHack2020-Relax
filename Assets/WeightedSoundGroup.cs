using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="BubbleGame/WeightedSoundGroup")]
public class WeightedSoundGroup : ScriptableObject {
    public List<WeightedSound> items;

    public AudioClip GetClipAt(float weightPoint01) {

        float t = 0;

        var weightPositioned = items.Select(i => {
            t += i.weight;
            return (t, i.sound);
        }).ToList();

        var denormalized = weightPoint01 * t;

        for(int i = 0; i < weightPositioned.Count;i++) {
            if (denormalized <= weightPositioned[i].t) return weightPositioned[i].sound;
        }

        return weightPositioned.Last().sound;
    }
}
