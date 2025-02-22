namespace SixShaded.FourZeroOne.Core.Macros
{
    internal static class Package
    {
        public const string NAMESPACE = "CORE";
        public static MacroLabel Label(string identifier) => new() { Package = NAMESPACE, Identifier = identifier };
    }
}