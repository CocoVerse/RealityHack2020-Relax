using System;
using System.Collections.Concurrent;

public class ObjectPool<T> : ILender<T> where T : IReturnable {
    private ConcurrentQueue<T> readyInstances = new ConcurrentQueue<T>();
    private Func<T> factory;
    private bool disposed;

    public ObjectPool(Func<T> factory) {
        this.factory = factory;
    }

    public T GetOne() {
        if (disposed) throw new ObjectDisposedException(nameof(ObjectPool<T>));
        T instance;
        if (!readyInstances.TryDequeue(out instance)) instance = factory();
        instance.Activate(() => this.Return(instance));
        return instance;
    }

    public void Return(T instance) {
        if (disposed) {
            instance.Dispose();
            return;
        }
        instance.Deactivate();
        readyInstances.Enqueue(instance);
    }

    public void Dispose() {
        if (disposed) return;
        disposed = true;
        foreach (var instance in readyInstances) instance.Dispose();
    }
}