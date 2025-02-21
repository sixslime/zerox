#nullable enable
namespace SixShaded.FourZeroOne.Macro
{
    using Core.Resolutions.Boxed;
    using FourZeroOne.FZOSpec;
    using SixShaded.NotRust;
    using Token;
    using Res = Resolution.IResolution;
    public sealed record MacroLabel
    {
        private string _package;
        private string _identifier;
        public required string Package { get => _package; init => _package = value.ToLower(); }
        public required string Identifier { get => _identifier; init => _identifier = value.ToLower(); }
        public override string ToString()
        {
            return $"{Package}~{Identifier}";
        }
    }
}