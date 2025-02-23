namespace SixShaded.DeTes.Declaration;

internal class DomainImpl : IDeTesMultiDomain, IDeTesSingleDomain, IDomainAccessor
{
    private int _index = -1;
    public required int[][] Selections { get; init; }
    public required string? Description { get; init; }
    public required Tok LinkedToken { get; init; }
    int[][] IDomainAccessor.Selections => Selections;
    Tok ITokenLinked.LinkedToken => LinkedToken;
    int IDomainAccessor.MetaIndex { get => _index; set => _index = value; }

    int IDeTesSingleDomain.SelectedIndex() => _index >= 0 ? Selections[_index][0] : throw MakeOutsideScopeException();
    int[] IDeTesMultiDomain.SelectedIndicies() => _index >= 0 ? Selections[_index] : throw MakeOutsideScopeException();

    private DeTesInvalidTestException MakeOutsideScopeException()
    {
        return new()
        {
            Value = new EDeTesInvalidTest.DomainUsedOutsideOfScope
            {
                Description = Description,
                NearToken = LinkedToken,
                Domain = Selections,
            }
        };
    }
}