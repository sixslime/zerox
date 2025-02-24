using MC = MorseCode.ITask;

namespace SixShaded.SixLib.ControlledFlows;

public interface ICeasableFlow : MC.ITask
{
    public void Cease();
}

public interface ICeasableFlow<out T> : MC.ITask<T>
{
    public ICeasableAwaiter<T> GetCeasableAwaiter();
    public void Cease();
}