using MorseCode.ITask;

// Made with duct tape and dreams.

namespace SixShaded.SixLib.ControlledFlows;
//make ControlledFlow.FromResult

public class ControlledFlow : ICeasableFlow
{
    private static readonly ControlledFlow _completedTask;

    static ControlledFlow()
    {
        _completedTask = new();
        _completedTask.Awaiter.OnCompleted(() => { });
        _completedTask.Resolve();
    }

    /// <summary>
    ///     Creates a task that halts <see langword="await" /> execution until Resolve() is called.
    /// </summary>
    public ControlledFlow()
    {
        Awaiter = new();
    }

    public static ControlledFlow CompletedTask => _completedTask;
    public ControlledAwaiter Awaiter { get; private set; }

    public static ControlledFlow<TResult> Resolved<TResult>(TResult result)
    {
        var o = new ControlledFlow<TResult>();
        o.Resolve(result);
        return o;
    }

    /// <summary>
    ///     Resolves this task and releases <see langword="await" /> execution.
    /// </summary>
    public void Resolve() => Awaiter.Resolve();

    public void Cease() => Awaiter.Cease();
    public IAwaiter GetAwaiter() => Awaiter;
    public IConfiguredTask ConfigureAwait(bool continueOnCapturedContext) => throw new NotImplementedException();
}

public class ControlledFlow<T> : ICeasableFlow<T>
{
    /// <summary>
    ///     <inheritdoc cref="ControlledFlow.ControlledFlow" /><br></br>
    ///     > Yields type <typeparamref name="T" /> when resolved.
    /// </summary>
    public ControlledFlow()
    {
        Awaiter = new();
    }

    public ControlledAwaiter<T> Awaiter { get; private set; }

    /// <summary>
    ///     <inheritdoc cref="ControlledFlow.Resolve" /> <br></br>
    ///     > Yields <paramref name="result" />.
    /// </summary>
    /// <param name="result"></param>
    public void Resolve(T result) => Awaiter.Resolve(result);

    public T Result => Awaiter.GetResult();
    public void Cease() => Awaiter.Cease();
    IConfiguredTask<T> ITask<T>.ConfigureAwait(bool continueOnCapturedContext) => throw new NotImplementedException();
    IConfiguredTask ITask.ConfigureAwait(bool continueOnCapturedContext) => throw new NotImplementedException();
    public IAwaiter<T> GetAwaiter() => Awaiter;
    IAwaiter ITask.GetAwaiter() => Awaiter;
    public ICeasableAwaiter<T> GetCeasableAwaiter() => Awaiter;
}

public class TransformedFlow<I, R> : ICeasableFlow<R>
{
    public TransformedFlow(ICeasableFlow<I> inputFlow, Func<I, R> transform)
    {
        Awaiter = new(inputFlow.GetCeasableAwaiter(), transform);
    }

    public TransformedAwaiter<I, R> Awaiter { get; private set; }
    public R Result => Awaiter.GetResult();
    public void Cease() => Awaiter.Cease();
    public IConfiguredTask<R> ConfigureAwait(bool continueOnCapturedContext) => throw new NotImplementedException();
    public IAwaiter<R> GetAwaiter() => Awaiter;
    IAwaiter ITask.GetAwaiter() => Awaiter;
    public ICeasableAwaiter<R> GetCeasableAwaiter() => Awaiter;
    IConfiguredTask ITask.ConfigureAwait(bool continueOnCapturedContext) => throw new NotImplementedException();
}