namespace SixShaded.FZOTypeMatch;

public interface IFZOTypeInfo<out T>
    where T : IFZOType
{
    public Type Origin { get; }
    public IOption<T> MatchedType { get; }
}