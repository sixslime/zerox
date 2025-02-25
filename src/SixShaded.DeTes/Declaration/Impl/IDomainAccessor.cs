namespace SixShaded.DeTes.Declaration.Impl;
internal interface IDomainAccessor : ITokenLinked, IHasDescription
{
    int[][] Selections { get; }
    int MetaIndex { get; set; }
}