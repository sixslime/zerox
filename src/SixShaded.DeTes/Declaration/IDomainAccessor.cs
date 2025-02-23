namespace SixShaded.DeTes.Declaration;

internal interface IDomainAccessor : ITokenLinked, IHasDescription
{
    int[][] Selections { get; }
    int MetaIndex { get; set; }
}