using MorseCode.ITask;

namespace SixShaded.SixLib.ControlledFlows;

public interface ICeasableAwaiter : IAwaiter
{
    public void Cease();
}

public interface ICeasableAwaiter<out T> : IAwaiter<T>, ICeasableAwaiter
{ }

public interface IControlledAwaiter : ICeasableAwaiter
{
    public void Resolve();
}

public interface IControlledAwaiter<T> : ICeasableAwaiter<T>
{
    public void Resolve(T result);
}

public class ControlledAwaiter : IControlledAwaiter
{
    private bool _completed;
    private Action _continueAction;

    public ControlledAwaiter()
    {
        _completed = false;
    }

    private static void DoFuckingNothing()
    { }

    public bool IsCompleted => _completed;

    public void Resolve()
    {
        Cease();

        //this is fucking stupid, i hate this, i want to die, why cant i do anything right
        _continueAction ??= DoFuckingNothing;
        _continueAction();
    }

    public void Cease()
    {
        if (_completed) throw new("Awaiter already resolved");
        _completed = true;
    }

    public void OnCompleted(Action continuation) => _continueAction = continuation;

    // yea, no idea how this is supposed to be different than non-unsafe OnCompleted(). if we crash we crash.
    // the documentation also isnt very helpful. sucks to suck!
    public void UnsafeOnCompleted(Action continuation) => _continueAction = continuation;

    public void GetResult()
    { }
}

public class ControlledAwaiter<T> : ControlledAwaiter, ICeasableAwaiter<T>
{
    private T _result;

    public virtual void Resolve(T result)
    {
        _result = result;
        Resolve();
    }

    // reeks
    public new T GetResult() => _result;
}

public class TransformedAwaiter<T, R> : ICeasableAwaiter<R>
{
    private readonly ICeasableAwaiter<T> _awaiter;
    private readonly Func<T, R> _transformFunction;

    public TransformedAwaiter(ICeasableAwaiter<T> awaiter, Func<T, R> transformFunction)
    {
        _awaiter = awaiter;
        _transformFunction = transformFunction;
    }

    private IAwaiter<T> _baseAwaiter => _awaiter;
    public bool IsCompleted => _baseAwaiter.IsCompleted;
    public void Cease() => _awaiter.Cease();
    public void OnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);
    public void UnsafeOnCompleted(Action continuation) => _awaiter.UnsafeOnCompleted(continuation);
    public R GetResult() => _transformFunction(_baseAwaiter.GetResult());
    void IAwaiter.GetResult() => _baseAwaiter.GetResult();
}