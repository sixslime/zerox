namespace SixShaded.DeTes.Declaration.Impl;
internal interface IAssertionAccessor<in T> : IKorssaLinked, IHasDescription
{
    Predicate<T> Condition { get; }
}
