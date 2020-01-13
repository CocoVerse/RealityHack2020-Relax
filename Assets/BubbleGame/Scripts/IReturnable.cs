using System;

public interface IReturnable : IDisposable {
    void Activate(Action returnAction);
    void Deactivate();
}
