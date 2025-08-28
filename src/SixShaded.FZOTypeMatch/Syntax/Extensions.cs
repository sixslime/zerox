namespace SixShaded.FZOTypeMatch.Syntax;

public static class Extensions
{
    public static IOption<IKorssaType> FZOTypeMatch(this Kor korssa, FZOTypeMatch match) => match.GetKorssaType(korssa);
}