using Perfection;
using System;
using MorseCode.ITask;


#nullable enable

// not clean, not safe, not elegant, not effecient, however:...
namespace FourZeroOne.Testing
{
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Runtime;
    using Token;

    public delegate Spec.ITest<R> TestStatement<out R>(Handle.Context context) where R : class, ResObj;

    public class Tester
    {
        public required IRuntime Runtime { get; init; }
        public required IState BaseState { get; init; }

        public Handle.Test<R> AddTest<R>(string name, TestStatement<R> test) where R : class, ResObj
        {
            _storedTests.Add(new Structure.StoredTest<R>() { Name = name, Stored = test.AsErr(new Hint<IResult<Structure.FinishedTest<R>, Exception>>()) });
            return new Handle.Test<R>()
            {
                Index = _storedTests.Count - 1,
                Source = this
            };
        }

        private async Task<IResult<Structure.IFinishedTest<R>, Exception>> EvaluateTest<R>(int index) where R : class, ResObj
        {
            // utterly restarted
            var test = _storedTests[index];
            _storedTests[index] = new Structure.StoredTest<R>()
            {
                Name = test.Name,
                Stored = test.Stored.Break(out var ok, out var statement)
                ? ok.RemapOk(x => (Structure.IFinishedTest<R>)x).AsOk(new Hint<TestStatement<R>>())
                : (await ResolveStatement<R>((TestStatement<R>)statement)).AsOk(new Hint<TestStatement<R>>())
            });
            return _storedTests[index].Stored.UnwrapOk().RemapOk(x => (Structure.IFinishedTest<R>)x);
            
        }
        private async Task<IResult<Structure.IFinishedTest<R>, Exception>> ResolveStatement<R>(TestStatement<R> statement) where R : class, ResObj
        {
            // horrendous
            var spec = statement(new() { Source = this });
            var startState = spec.State(BaseState);
            var result = await Runtime.Run(startState, spec.Evaluate);
            return Result.Caught(() => new Structure.FinishedTest<R>()
            {
                Spec = spec,
                RunResult = Result.Caught(() => new Structure.TestResults<R>()
                {
                    Resolution = result.RemapAs(x => (R)x),
                    State = result.Check(out var x) ? startState : startState.WithResolution(x)
                })
            });
        }
        private List<Structure.IStoredTest<ResObj>> _storedTests = [];
    }
    namespace Handle
    {
        public record Context
        {
            public required Tester Source { get; init; }
        }
        public record Test<R> : ITest<R> where R : class, ResObj
        {
            public required int Index { get; init; }
            public required Tester Source { get; init; }
        }
        public interface ITest<out R>
        {
            public int Index { get; }
            public Tester Source { get; }
        }
    }
    namespace Structure
    {
        public record StoredTest<R> : IStoredTest<R> where R : class, ResObj
        {
            public required string Name { get; init; }
            public required IResult<IResult<IFinishedTest<R>, Exception>, TestStatement<R>> Stored { get; init; }
        }
        public record FinishedTest<R> : IFinishedTest<R> where R : class, ResObj
        {
            public required Spec.ITest<R> Spec { get; init; }
            public required IResult<ITestResults<R>, Exception> RunResult { get; init; }
        }
        public record TestResults<R> : ITestResults<R> where R : class, ResObj
        {
            public required IOption<R> Resolution { get; init; }
            public required IState State { get; init; }
        }
    }
    namespace Spec
    {
        public record Test<R> : ITest<R> where  R : class, ResObj
        {
            public Func<IState, IState> State { get; init; } = x => x;
            public required IToken<R> Evaluate { get; init; }
            public int[][] Selections { get; init; } = [];
            public Expects<R>? Expect { get; init; }
            public Asserts? Assert { get; init; }
            public IExpects<R>? ExpectI => Expect;
            public IAsserts? AssertI => Assert;
        }
        public record Expects<R> : IExpects<R> where R : class, ResObj
        {
            public IOption<R>? Resolution { get; init; }
            public Func<IState, IState>? State { get; init; }
        }
        public record Asserts : IAsserts
        {
            public Predicate<IToken>? Token { get; init; }
            public Predicate<ResObj>? Resolution { get; init; }
            public Predicate<IState>? State { get; init; }
        }
        
        // really dumb that i have to make these
        public interface ITest<out R> where R : class, ResObj
        {
            public Func<IState, IState> State { get; }
            public IToken<R> Evaluate { get; }
            public int[][] Selections { get; }
            public IExpects<R>? ExpectI { get; }
            public IAsserts? AssertI { get; }
        }
        public interface IExpects<out R> where R : class, ResObj
        {
            public IOption<R>? Resolution { get; }
            public Func<IState, IState>? State { get; }
        }
        public interface IAsserts
        {
            public Predicate<ResObj>? Resolution { get; }
            public Predicate<IToken>? Token { get; }
            public Predicate<IState>? State { get; }
        }
    }
    
}