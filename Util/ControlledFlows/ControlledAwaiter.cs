using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using MC = MorseCode.ITask;
using MorseCode.ITask;

#nullable enable
namespace ControlledFlows
{
    public interface ICeasableAwaiter : MC.IAwaiter
    {
        public void Cease();
    }
    public interface ICeasableAwaiter<out T> : MC.IAwaiter<T>, ICeasableAwaiter { }
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
        public bool IsCompleted => _completed;
        private Action _continueAction;

        private static void DoFuckingNothing() { }
        public ControlledAwaiter()
        {
            _completed = false;
        }
        public void Resolve()
        {
            Cease();
            //this is fucking stupid, i hate this, i want to die, why cant i do anything right
            _continueAction ??= DoFuckingNothing;
            _continueAction();
        }
        public void Cease()
        {
            if (_completed) throw new Exception("Awaiter already resolved");
            _completed = true;
        }
        public void OnCompleted(Action continuation)
        {
            _continueAction = continuation;
        }
        // yea, no idea how this is supposed to be different than non-unsafe OnCompleted(). if we crash we crash.
        // the documentation also isnt very helpful. sucks to suck!
        public void UnsafeOnCompleted(Action continuation)
        {
            _continueAction = continuation;
        }

        public void GetResult() { }
    }
    public class ControlledAwaiter<T> : ControlledAwaiter, ICeasableAwaiter<T>
    {
        private T _result;

        public ControlledAwaiter() : base() { }
        public virtual void Resolve(T result)
        {
            _result = result;
            base.Resolve();
        }
        // reeks
        public new T GetResult() => _result;
    }

    public class TransformedAwaiter<T, R> : ICeasableAwaiter<R>
    {
        private ICeasableAwaiter<T> _awaiter;
        private IAwaiter<T> _baseAwaiter => (IAwaiter<T>)_awaiter;
        private Func<T, R> _transformFunction;
        public bool IsCompleted => _baseAwaiter.IsCompleted;

        public TransformedAwaiter(ICeasableAwaiter<T> awaiter, Func<T, R> transformFunction)
        {
            _awaiter = awaiter;
            _transformFunction = transformFunction;
        }
        public void Cease() => _awaiter.Cease();
        public void OnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);
        public void UnsafeOnCompleted(Action continuation) => _awaiter.UnsafeOnCompleted(continuation);

        public R GetResult() => _transformFunction(_baseAwaiter.GetResult());
        void IAwaiter.GetResult() => _baseAwaiter.GetResult();
    }

}