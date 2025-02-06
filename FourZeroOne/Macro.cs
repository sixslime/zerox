
using System.Collections.Generic;
using Perfection;
using ControlledFlows;
using MorseCode.ITask;
#nullable enable
namespace FourZeroOne.Macro
{

    using ResObj = Resolution.IResolution;
    using r = Core.Resolutions;
    using Token;
    using Handles;
    public interface IMacro<out R> : IToken<R>, Unsafe.IMacro where R : class, ResObj
    {
        public IToken<R> Expand();
    }
    public abstract record Macro<R> : Token<R>, IMacro<R> where R : class, ResObj
    {
        protected abstract Proxy.Unsafe.IProxy<R> InternalProxy { get; }
        public override IResult<ITask<IOption<R>>, Resolution.EProcessorHandled> Resolve(ITokenContext _, IOption<ResObj>[] __)
        {
            throw new Exception("Macro directly resolved without expansion.");
        }
        protected Macro()
        {
            _cachedRealization = null;
        }
        public IToken<R> Expand()
        {
            _cachedRealization ??= InternalProxy.UnsafeTypedRealize(this, new None<Rule.IRule>());
            return _cachedRealization;
        }
        public Token.Unsafe.IToken ExpandUnsafe() => Expand();

        private IToken<R>? _cachedRealization;
    }

    /// <summary>
    /// Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    public abstract record OneArg<RArg1, ROut> : Macro<ROut>, IFunction<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 { get; }
        protected OneArg(IToken<RArg1> in1)
        {
            Arg1 = in1;
        }
    }
    /// <summary>
    /// Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;, IToken&lt;<typeparamref name="RArg2"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    /// <typeparam name="RArg2"></typeparam>
    public abstract record TwoArg<RArg1, RArg2, ROut> : Macro<ROut>, IFunction<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 { get; }
        public IToken<RArg2> Arg2 { get; }
        protected TwoArg(IToken<RArg1> in1, IToken<RArg2> in2)
        {
            Arg1 = in1;
            Arg2 = in2;
        }
    }
    /// <summary>
    /// Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;, IToken&lt;<typeparamref name="RArg2"/>&gt;, IToken&lt;<typeparamref name="RArg3"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    /// <typeparam name="RArg2"></typeparam>
    /// <typeparam name="RArg3"></typeparam>
    public abstract record ThreeArg<RArg1, RArg2, RArg3, ROut> : Macro<ROut>, IFunction<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 { get; }
        public IToken<RArg2> Arg2 { get; }
        public IToken<RArg3> Arg3 { get; }
        protected ThreeArg(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3)
        {
            Arg1 = in1;
            Arg2 = in2;
            Arg3 = in3;
        }
    }
}

namespace FourZeroOne.Macro.Unsafe
{
    using ResObj = Resolution.IResolution;
    using r = Core.Resolutions;
    using Token.Unsafe;
    public interface IMacro : IToken
    {
        public IToken ExpandUnsafe();
    }
}