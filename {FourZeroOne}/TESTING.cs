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

    public delegate Spec.Test<R> TestStatement<R>(IRuntime runtime) where R : class, ResObj;
    public delegate Spec.ITest StoredStatement(IRuntime runtime);
    public class TestCreationException(Exception e) : Exception("", e) { }
    public class TestEvaluateException(Exception e) : Exception("", e) { }
    public class TestFailedException(Structure.FinishedTest test) : Exception()
    {
        public Structure.FinishedTest FailedTest = test;
    }

    public record Test<R> : ITest<R> where R : class, ResObj
    {
        public required string Name { get; init; }
        public required IRuntime Runtime { get; init; }
        public required TestStatement<R> Statement { init { _value = new Err<IResult<Structure.FinishedTest, TestCreationException>, StoredStatement>(x => value(x)); } }

        public async ITask<IToken<R>> GetToken()
        {
            return (IToken<R>)(await EvaluateMustPass()).Spec.EvaluateI;
        }
        public async ITask<IOption<R>> GetResolution()
        {
            return (await EvaluateMustPass()).RunResult.Break(out var results, out var exc)
                ? results.Resolution.RemapAs(x => (R)x)
                : throw exc;
        }
        public async Task<Structure.FinishedTest> EvaluateMustPass()
        {
            var o = await Evaluate();
            return o.Passed ? o : throw new TestFailedException(o);
        }
        public async Task<Structure.FinishedTest> Evaluate()
        {
            if (_value.Break(out var cached, out var statement))
            {
                return cached.Break(out var test, out var exception) ? test : throw exception;
            }
            else
            {
                var creation = (await Result.CatchExceptionAsync(async () =>
                {
                    var spec = statement(Runtime);
                    return new Structure.FinishedTest()
                    {
                        Spec = spec,
                        RunResult = (await Result.CatchExceptionAsync(async () =>
                        {
                            var r = await Runtime.Run(spec.State, spec.EvaluateI);
                            return new Structure.TestResults
                            {
                                Resolution = r,
                                State = r.Check(out var some) ? spec.State.WithResolution(some) : spec.State
                            };
                        })).RemapErr(x => new TestEvaluateException(x))
                    };
                }
                )).RemapErr(x => new TestCreationException(x));

                _value = _value.ToOk(creation);
                return creation.Break(out var finished, out var exc)
                    ? finished
                    : throw exc;
            }

        }
               
        private IResult<IResult<Structure.FinishedTest, TestCreationException>, StoredStatement> _value;
    }
    public interface ITest<out R> where R : class, ResObj
    {
        public string Name { get; init; }
        public IRuntime Runtime { get; init; }
        public ITask<IToken<R>> GetToken();
        public ITask<IOption<R>> GetResolution();
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
            public required IResult<TestResults, TestEvaluateException> RunResult { get; init; }
            public bool Passed { get
                {
                    return RunResult.Break(out var results, out var exc)
                        ? NullPass(Spec.AssertI, assert =>
                                NullPass(assert.State, f => f(Spec.State)(results.State)) &&
                                NullPass(assert.Token, p => p(Spec.EvaluateI)) &&
                                NullPass(assert.Resolution, p => p(results.Resolution))) &&
                            NullPass(Spec.ExpectI, expect =>
                                NullPass(expect.ResolutionI, x => x.Equals(results.Resolution)) &&
                                NullPass(expect.State, f => f(Spec.State).Equals(results.State)))
                        : throw exc;
                } }
            private static bool NullPass<T>(T? v, Predicate<T> pred)
            {
                return v is null || pred(v);
            }
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
            public required IState State { get; init; } 
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
            public Predicate<IOption<ResObj>>? Resolution { get; init; }
            public Func<IState, Predicate<IState>>? State { get; init; }
        }
        
        // really dumb that i have to make these
        public interface ITest
        {
            public IState State { get; }
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
            public Predicate<IOption<ResObj>>? Resolution { get; }
            public Predicate<IToken>? Token { get; }
            public Func<IState, Predicate<IState>>? State { get; }
        }
    }
    
}