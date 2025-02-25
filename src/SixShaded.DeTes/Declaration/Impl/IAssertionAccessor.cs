namespace SixShaded.DeTes.Declaration.Impl;
internal interface IAssertionAccessor<in T> : ITokenLinked, IHasDescription
{
    Predicate<T> Condition { get; }
}
