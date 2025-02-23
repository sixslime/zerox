namespace SixShaded.DeTes.Analysis;

public interface IDeTesAssertionDataUntyped
{
    public Tok OnToken { get; }
    public IResult<bool, Exception> Result { get; }
    public string? Description { get; }
}