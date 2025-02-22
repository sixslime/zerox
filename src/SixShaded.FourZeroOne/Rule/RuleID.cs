#nullable enable
namespace SixShaded.FourZeroOne.Rule
{
    public readonly struct RuleID(int id)
    {
        public readonly int ID = id;
        public override string ToString() => $"RuleID({ID})";
    }
}
