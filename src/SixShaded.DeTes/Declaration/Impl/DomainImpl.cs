namespace SixShaded.DeTes.Declaration.Impl;

internal class DomainImpl : IDeTesMultiDomain, IDeTesSingleDomain, IDomainAccessor
{
    private int _index = -1;
    public required int[][] Selections { get; init; }
    public required Kor LinkedKorssa { get; init; }

    private DeTesInvalidTestException MakeOutsideScopeException() =>
        new()
        {
            Value =
                new EDeTesInvalidTest.DomainUsedOutsideOfScope
                {
                    Description = Description,
                    NearKorssa = LinkedKorssa,
                    Domain = Selections,
                },
        };

    int[] IDeTesMultiDomain.SelectedIndicies() => _index >= 0 ? Selections[_index] : throw MakeOutsideScopeException();
    int IDeTesSingleDomain.SelectedIndex() => _index >= 0 ? Selections[_index][0] : throw MakeOutsideScopeException();
    public required string? Description { get; init; }
    int[][] IDomainAccessor.Selections => Selections;
    Kor IKorssaLinked.LinkedKorssa => LinkedKorssa;

    int IDomainAccessor.MetaIndex
    {
        get => _index;
        set => _index = value;
    }
}