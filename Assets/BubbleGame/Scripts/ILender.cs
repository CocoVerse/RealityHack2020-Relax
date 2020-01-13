using System;

public interface ILender<in T> : IDisposable {
    void Return(T instance);
}
