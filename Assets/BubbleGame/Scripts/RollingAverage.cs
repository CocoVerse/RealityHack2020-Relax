using System.Collections.Concurrent;
using System.Linq;

public class RollingAverage {
    private float window;

    private ConcurrentQueue<(float time, float value)> records = new ConcurrentQueue<(float time, float value)>();

    public RollingAverage(float window) {
        this.window = window;
    }

    public bool TryGetAverage(out float mean) {
        if (records.Count > 0) {
            mean = records.Select(r => r.value).Sum() / records.Count;
            return true;
        } else {
            mean = 0;
            return false;
        }
    }

    public void Push(float time, float value) {
        records.Enqueue((time, value));
    }

    public void Crop(float now) {
        while(records.TryPeek(out var r) && r.time < now-window) {
            records.TryDequeue(out _);
        }
    }

    public void Clear() {
        while (records.TryDequeue(out _)) { }
    }
}
