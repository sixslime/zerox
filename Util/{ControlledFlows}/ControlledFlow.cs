using System;
using System.Runtime.CompilerServices;
using MorseCode.ITask;
using MC = MorseCode.ITask;

// Made with duct tape and dreams.

#nullable enable
namespace ControlledFlows
{
    //make ControlledFlow.FromResult
    
    public class ControlledFlow : ICeasableFlow
    {
        public static ControlledFlow CompletedTask => _completedTask;
        static ControlledFlow()
        {
            _completedTask = new ControlledFlow();
            _completedTask.Awaiter.OnCompleted(() => { });
            _completedTask.Resolve();
        }
        public static ControlledFlow<TResult> Resolved<TResult>(TResult result)
        {
            var o = new ControlledFlow<TResult>();
            o.Resolve(result);
            return o;
        }
        public ControlledAwaiter Awaiter { get; private set; }

        /// <summary>
        /// Creates a task that halts <see langword="await"/> execution until Resolve() is called.
        /// </summary>
        public ControlledFlow()
        {
            Awaiter = new ControlledAwaiter();
        }

        /// <summary>
        /// Resolves this task and releases <see langword="await"/> execution.
        /// </summary>
        public void Resolve()
        {
            Awaiter.Resolve();
        }
        public void Cease()
        {
            Awaiter.Cease();
        }

        public MC.IAwaiter GetAwaiter() => Awaiter;
        public MC.IConfiguredTask ConfigureAwait(bool continueOnCapturedContext) => throw new System.NotImplementedException();

        private static ControlledFlow _completedTask;
    }
    
    public class ControlledFlow<T> : ICeasableFlow<T>
    {
        public ControlledAwaiter<T> Awaiter { get; private set; }
        public T Result => Awaiter.GetResult();

        /// <summary>
        /// <inheritdoc cref="ControlledFlow.ControlledFlow"/><br></br>
        /// > Yields type <typeparamref name="T"/> when resolved.
        /// </summary>
        public ControlledFlow()
        {
            Awaiter = new ControlledAwaiter<T>();
        }

        /// <summary>
        /// <inheritdoc cref="ControlledFlow.Resolve"/> <br></br>
        /// > Yields <paramref name="result"/>.
        /// </summary>
        /// <param name="result"></param>
        public void Resolve(T result)
        {
            Awaiter.Resolve(result);
        }
        public void Cease()
        {
            Awaiter.Cease();
        }

        MC.IConfiguredTask<T> MC.ITask<T>.ConfigureAwait(bool continueOnCapturedContext) => throw new System.NotImplementedException();
        MC.IConfiguredTask MC.ITask.ConfigureAwait(bool continueOnCapturedContext) => throw new System.NotImplementedException();

        public IAwaiter<T> GetAwaiter() => (IAwaiter<T>)Awaiter;
        IAwaiter ITask.GetAwaiter() => Awaiter;

        public ICeasableAwaiter<T> GetCeasableAwaiter() => (ICeasableAwaiter<T>)Awaiter;
    }

    public class TransformedFlow<I, R> : ICeasableFlow<R>
    {
        public TransformedAwaiter<I, R> Awaiter { get; private set; }
        public R Result => Awaiter.GetResult();

        public void Cease()
        {
            Awaiter.Cease();
        }
        public TransformedFlow(ICeasableFlow<I> inputFlow, Func<I, R> transform)
        {
            Awaiter = new TransformedAwaiter<I, R>(inputFlow.GetCeasableAwaiter(), transform);
        }
        public IConfiguredTask<R> ConfigureAwait(bool continueOnCapturedContext)
        {
            throw new NotImplementedException();
        }

        public IAwaiter<R> GetAwaiter() => (IAwaiter<R>)Awaiter;
        IAwaiter ITask.GetAwaiter() => Awaiter;
        public ICeasableAwaiter<R> GetCeasableAwaiter() => (ICeasableAwaiter<R>)Awaiter;

        IConfiguredTask ITask.ConfigureAwait(bool continueOnCapturedContext)
        {
            throw new NotImplementedException();
        }
    }
    
}
