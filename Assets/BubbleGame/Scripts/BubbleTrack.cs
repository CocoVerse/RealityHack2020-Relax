using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrack : MonoBehaviour
{
    [SerializeField] float bpm = 120;
    [SerializeField] int rows = 4;
    [SerializeField] int columns = 6;
    [SerializeField] float gap = .25f;
    [SerializeField] int num = 4;

    [SerializeField] List<GameObject> prefabs;

    float Interval => 60f / bpm;

    private float tq = 0;

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

        var n = Mathf.Min(num, rows * columns);

        for(int i = 0; i < n; i++) {
            (int, int) x;
            do {
                x = (Random.Range(0, columns), Random.Range(0, rows));

            } while (!filled.Add(x));
        }

        int k = 0;
        var o0 = new Vector3(0.5f * (columns - 1), 0.5f * (rows - 1));
        foreach (var (x,y) in filled) {
            var offset = gap * (new Vector3(x, y) - o0);
            var position = transform.position + transform.rotation*offset;
            Instantiate(prefabs[k], position, transform.rotation);
            k++;
            k %= prefabs.Count;
        }
    }
}
