using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrack : MonoBehaviour
{
    [SerializeField] float bpm = 120;
    [SerializeField] int rows = 4;
    [SerializeField] int columns = 6;
    [SerializeField] float gap = .25f;

    [SerializeField] float bubblesPerBeatMean = 2;
    [SerializeField] float bubblesPerBeatStdev = 1f;

    [SerializeField] List<BubbleMove> prefabs;

    [SerializeField] BubbleGameScoreTracker scoreTracker;

    float Interval => 60f / bpm;

    private float tq = 0;
    private int nextColorIndex = 0;

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
            MathUtil.BoxMuller(bubblesPerBeatMean,bubblesPerBeatStdev,Random.value,Random.value),
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
            var obj = Instantiate(prefabs[nextColorIndex], position, transform.rotation);
            obj.ScoreTracker = scoreTracker;
            nextColorIndex++;
            nextColorIndex %= prefabs.Count;
        }
    }
}


public class MathUtil {
    public static float BoxMuller(float mean, float stdev, float u0, float u1) {
        var y = Mathf.Sqrt(-2f * Mathf.Log(u0)) * Mathf.Sin(2f * Mathf.PI * u1);
        return mean + stdev * y;
    }
}