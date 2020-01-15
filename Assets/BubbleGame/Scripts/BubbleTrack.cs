using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.Playables;
using System;

[Serializable]
public class BubblePrefabGroup {
    [SerializeField] List<BubbleMove> prefabs;

}

public class BubbleTrack : MonoBehaviour {

    public class BubbleHelperGroup {
        public IReadOnlyDictionary<BubbleColorCode, BubbleHelper> Helpers { get; }

        private BubbleColorCode nextColorCode;

        public BubbleHelperGroup(List<BubbleMove> prefabs) { 
            var dict = new Dictionary<BubbleColorCode, BubbleHelper>();
            foreach (var prefab in prefabs) {
                dict.Add(prefab.ColorCode, new BubbleHelper(prefab));
            }
            Helpers = dict;

            nextColorCode = UnityEngine.Random.value > 0.5f ? BubbleColorCode.Yellow : BubbleColorCode.Pink;
        }

        public BubbleHelper NextColorBubble() {
            var obj = Helpers[nextColorCode];
            nextColorCode = nextColorCode == BubbleColorCode.Pink ? BubbleColorCode.Yellow : BubbleColorCode.Pink;
            return obj;
        }
    }

    public class BubbleHelper {
        private BubbleMove prefab;

        readonly ObjectPool<BubblePopEffect> popEffectPool;

        public BubbleHelper(BubbleMove prefab) {
            this.prefab = prefab;
            this.popEffectPool = new ObjectPool<BubblePopEffect>(() => Instantiate(prefab.PopEffectPrefab));
        }
        
        public BubbleMove GetBubbleInstance(Vector3 position, Quaternion rotation) {
            var obj = Instantiate(prefab, position, rotation);
            obj.PopEffectGetter = popEffectPool.GetOne;
            return obj;
        }
    }

    private int bpm = 60;
    
    [SerializeField] int rows = 4;
    [SerializeField] int columns = 6;
    [SerializeField] float gap = .25f;

    [SerializeField] AutoLevelGenerator sequence;
    
    [SerializeField] List<BubbleMove> bubblePrefabs;

    [SerializeField] BubbleGameScoreTracker scoreTracker;

    [SerializeField] MusicManager musicManager;
    

    float Interval => currentLevelParameters?.GetBatchInterval(bpm) ?? float.MaxValue;

    private BubbleGameLevelParameters currentLevelParameters;
    private BubbleHelperGroup bubbleHelperGroup;
    
    private float timeSinceWave = 0;

    private void Start() {
        musicManager.SelectedMusicGroup.Subscribe(ApplyMusicGroup).AddTo(this);
        bubbleHelperGroup = new BubbleHelperGroup(bubblePrefabs);
        scoreTracker.Level.Subscribe(i => currentLevelParameters = sequence.GetLevel(i));
    }

    void ApplyMusicGroup(MusicGroup musicGroup) {
        bpm = musicGroup.bpm;
    }
    
    void Update()
    { 
        timeSinceWave += Time.deltaTime;
        var interval = Interval;
        if(timeSinceWave > interval) {
            timeSinceWave %= interval;
            SpawnWave();
        }
    }

    void SpawnWave() {
        var filled = new HashSet<(int, int)>();

        var n = Mathf.Clamp(
            currentLevelParameters.GetRandomBatchSize(),
            0,
            rows * columns
        );

        for(int i = 0; i < n; i++) {
            (int, int) x;
            do {
                x = (UnityEngine.Random.Range(0, columns), UnityEngine.Random.Range(0, rows));
            } while (!filled.Add(x));
        }
        
        var o0 = new Vector3(0.5f * (columns - 1), 0.5f * (rows - 1));
        foreach (var (x,y) in filled) {
            var offset = gap * (new Vector3(x, y) - o0);
            var position = transform.position + transform.rotation*offset;

            var obj = NextHelper().GetBubbleInstance(position,transform.rotation); 

            obj.ScoreTracker = scoreTracker;
            obj.Speed = currentLevelParameters.BubbleSpeed;
        }
    }

    BubbleHelper NextHelper() {
        if (scoreTracker.IsReadyForLevelUp) {
            scoreTracker.ClearLevelUpAvailability();
            return bubbleHelperGroup.Helpers[BubbleColorCode.LevelUp];
        } else if (currentLevelParameters.GetRandomIsNeutral()) {
            return bubbleHelperGroup.Helpers[BubbleColorCode.Neutral];
        } else {
            return bubbleHelperGroup.NextColorBubble();
        }
    }
}


public class MathUtil {
    public static float BoxMuller(float mean, float stdev, float u0, float u1) {
        var y = Mathf.Sqrt(-2f * Mathf.Log(u0)) * Mathf.Sin(2f * Mathf.PI * u1);
        return mean + stdev * y;
    }
}