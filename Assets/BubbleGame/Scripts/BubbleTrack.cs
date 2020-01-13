using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.Playables;

public class BubbleTrack : MonoBehaviour
{
    [SerializeField] float bpm = 120;
    [SerializeField] int rows = 4;
    [SerializeField] int columns = 6;
    [SerializeField] float gap = .25f;

    [SerializeField] AutoLevelGenerator sequence;
    
    [SerializeField] List<BubbleMove> prefabs;
    [SerializeField] BubbleMove levelUpBubblePrefab;
    [SerializeField] List<BubblePopEffect> popEffectPrefabs;

    [SerializeField] BubbleGameScoreTracker scoreTracker;


    private Dictionary<BubbleColorCode, ObjectPool<BubblePopEffect>> popEffectPools;

    float Interval => currentLevelParameters?.GetBatchInterval(bpm) ?? float.MaxValue;

    private float tq = 0;
    private int nextColorIndex = 0;

    private BubbleGameLevelParameters currentLevelParameters;

    private void Start() {
        popEffectPools = new Dictionary<BubbleColorCode, ObjectPool<BubblePopEffect>>();
        foreach(var effect in popEffectPrefabs) {
            popEffectPools.Add(effect.ColorCode, new ObjectPool<BubblePopEffect>(() => Instantiate(effect)));
        }
        scoreTracker.Level.Subscribe(i => currentLevelParameters = sequence.GetLevel(i));
    }

    // Update is called once per frame
    void Update()
    { 
        tq += Time.deltaTime;
        if(tq > Interval) {
            tq %= Interval;
            Spawn();
        }
    }

    void Spawn() {
        var filled = new HashSet<(int, int)>();

        var n = Mathf.Clamp(
            currentLevelParameters.GetRandomBatchSize(),
            0,
            rows * columns
        );

        for(int i = 0; i < n; i++) {
            (int, int) x;
            do {
                x = (Random.Range(0, columns), Random.Range(0, rows));
            } while (!filled.Add(x));
        }
        
        var o0 = new Vector3(0.5f * (columns - 1), 0.5f * (rows - 1));
        foreach (var (x,y) in filled) {
            var offset = gap * (new Vector3(x, y) - o0);
            var position = transform.position + transform.rotation*offset;

            var obj = NextBubble(position); 

            obj.ScoreTracker = scoreTracker;
            obj.Speed = currentLevelParameters.BubbleSpeed;

            if (popEffectPools.TryGetValue(obj.ColorCode, out var pool)) obj.PopEffectGetter = pool.GetOne;
        }
    }

    BubbleMove NextBubble(Vector3 position) {
        if (scoreTracker.IsReadyForLevelUp) {
            var obj = Instantiate(levelUpBubblePrefab, position, transform.rotation);
            scoreTracker.ClearLevelUpAvailability();
            return obj;
        } else {
            var obj = Instantiate(prefabs[nextColorIndex], position, transform.rotation);
            nextColorIndex++;
            nextColorIndex %= prefabs.Count;
            return obj;
        }
    }
}


public class MathUtil {
    public static float BoxMuller(float mean, float stdev, float u0, float u1) {
        var y = Mathf.Sqrt(-2f * Mathf.Log(u0)) * Mathf.Sin(2f * Mathf.PI * u1);
        return mean + stdev * y;
    }
}