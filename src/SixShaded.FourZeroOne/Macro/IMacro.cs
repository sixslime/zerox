#nullable enable
namespace SixShaded.FourZeroOne.Macro
{
    public interface IMacro<R> : IToken<R> where R : Res
    {
        public MacroLabel Label { get; }
        public object[] CustomData { get; }
    }
}