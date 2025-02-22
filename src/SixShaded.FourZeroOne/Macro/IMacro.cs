#nullable enable
namespace SixShaded.FourZeroOne.Macro
{
    using Core.Resolutions.Boxed;
    using FourZeroOne.FZOSpec;
    using SixShaded.NotRust;
    using Token;
    using Res = Resolution.IResolution;

    public interface IMacro<R> : IToken<R> where R : Res
    {
        public MacroLabel Label { get; }
        public object[] CustomData { get; }
    }
}