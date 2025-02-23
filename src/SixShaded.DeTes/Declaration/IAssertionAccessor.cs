namespace SixShaded.DeTes.Declaration;
internal interface IAssertionAccessor<in T> : ITokenLinked, IHasDescription
{
    Predicate<T> Condition { get; }
}
