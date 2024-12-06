using Perfection;
using System;


#nullable enable
namespace FourZeroOne.Testing
{
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Runtime;
    using Token;
    
    public class Tester
    {
        public required IRuntime Runtime;
    }
    namespace Spec
    {
        public record Test<T, R> : ITest where T : IToken<R> where R : class, ResObj
        {
            public IState? State { get; init; }
            public required T Evaluate { get; init; }
            public int[][] Selections { get; init; } = [];
            public Expects<T, R>? Expect { get; init; }
            public Asserts<T, R>? Assert { get; init; }
            public IExpects? UntypedExpect => Expect;
            public IAsserts? UntypedAssert => Assert;
            public IToken UntypedEvaluate => Evaluate;
        }
        public record Expects<T, R> : IExpects where T : IToken<R> where R : class, ResObj
        {
            public R? Resolution { get; init; }
            public Func<IState, IState>? State { get; init; }
            public ResObj? UntypedResolution => Resolution;
        }
        public record Asserts<T, R> : IAsserts where T : IToken<R> where R : class, ResObj
        {
            public Predicate<T>? Token { get; init; }
            public Predicate<R>? Resolution { get; init; }
            public Predicate<IState>? State { get; init; }
            // SILLY
            public Predicate<ResObj>? UntypedResolution => (Resolution is Predicate<R> P) ? x => x is R r && P(r) : null;
            public Predicate<IToken>? UntypedToken => (Token is Predicate<T> P) ? x => x is T t && P(t) : null;
        }

        // really dumb that i have to make these
        public interface ITest
        {
            public IState? State { get; }
            public IToken UntypedEvaluate { get; }
            public int[][] Selections { get; }
            public IExpects? UntypedExpect { get; }
            public IAsserts? UntypedAssert { get; }
        }
        public interface IExpects
        {
            public ResObj? UntypedResolution { get; }
            public Func<IState, IState>? State { get; }
        }
        public interface IAsserts
        {
            public Predicate<ResObj>? UntypedResolution { get; }
            public Predicate<IToken>? UntypedToken { get; }
            public Predicate<IState>? State { get; }
        }
    }
    
}