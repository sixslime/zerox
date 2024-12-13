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

    public delegate Spec.Test<R> TestStatement<R>(Handle.Context context) where R : class, ResObj;
    public delegate Spec.ITest StoredStatement(Handle.Context context);

    public class Tester
    {
        public required IRuntime Runtime { get; init; }
        public required IState BaseState { get; init; }

        public Handle.Test<R> AddTest<R>(string name, TestStatement<R> test) where R : class, ResObj
        {
            _storedTests.Add(new Structure.StoredTest()
            {
                Name = name,
                Stored =
                new StoredStatement(x => test(x)).AsErr(new Hint<IResult<Structure.FinishedTest, Exception>>())
            });
            return new Handle.Test<R>()
            {
                Index = _storedTests.Count - 1,
                Source = this
            };
        }
        private List<Structure.StoredTest> _storedTests = [];
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
        public record StoredTest
        {
            public required string Name { get; init; }
            public required IResult<IResult<FinishedTest, Exception>, StoredStatement> Stored { get; init; }
        }
        public record FinishedTest
        {
            public required Spec.ITest Spec { get; init; }
            public required IResult<TestResults, Exception> RunResult { get; init; }
        }
        public record TestResults
        {
            public required IOption<ResObj> Resolution { get; init; }
            public required IState State { get; init; }
        }
    }
    namespace Spec
    {
        public record Test<R> : ITest where  R : class, ResObj
        {
            public Func<IState, IState> State { get; init; } = x => x;
            public required IToken<R> Evaluate { get; init; }
            public IToken<ResObj> EvaluateI => Evaluate;
            public int[][] Selections { get; init; } = [];
            public Expects<R>? Expect { get; init; }
            public Asserts? Assert { get; init; }
            public IExpects? ExpectI => Expect;
            public IAsserts? AssertI => Assert;
        }
        public record Expects<R> : IExpects where R : class, ResObj
        {
            public IOption<R>? Resolution { get; init; }
            public IOption<ResObj>? ResolutionI => Resolution;
            public Func<IState, IState>? State { get; init; }
        }
        public record Asserts : IAsserts
        {
            public Predicate<IToken>? Token { get; init; }
            public Predicate<ResObj>? Resolution { get; init; }
            public Predicate<IState>? State { get; init; }
        }
        
        // really dumb that i have to make these
        public interface ITest
        {
            public Func<IState, IState> State { get; }
            public IToken<ResObj> EvaluateI { get; }
            public int[][] Selections { get; }
            public IExpects? ExpectI { get; }
            public IAsserts? AssertI { get; }
        }
        public interface IExpects
        {
            public IOption<ResObj>? ResolutionI { get; }
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