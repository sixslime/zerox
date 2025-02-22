namespace SixShaded.FourZeroOne.Rule;

public static class RuleIDGenerator
{
    private static int _currentID;
    public static RuleID Next() => new(_currentID++);
}