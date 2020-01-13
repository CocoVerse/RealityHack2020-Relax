using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BubbleGame/LevelSequence")]
public class BubbleGameLevelSequence : ScriptableObject{
    [SerializeField] List<BubbleGameLevelParameters> levels;
    public IReadOnlyList<BubbleGameLevelParameters> Levels => levels;
}
