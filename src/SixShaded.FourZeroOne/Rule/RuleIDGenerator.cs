#nullable enable
namespace SixShaded.FourZeroOne.Rule
{
    public static class RuleIDGenerator
    {
        private static int _currentID = 0;
        public static RuleID Next() => new(_currentID++);
    }
}
