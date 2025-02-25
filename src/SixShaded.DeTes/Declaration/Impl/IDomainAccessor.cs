namespace SixShaded.DeTes.Declaration.Impl;
internal interface IDomainAccessor : IKorssaLinked, IHasDescription
{
    int[][] Selections { get; }
    int MetaIndex { get; set; }
}