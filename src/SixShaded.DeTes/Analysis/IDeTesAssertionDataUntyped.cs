namespace SixShaded.DeTes.Analysis;

public interface IDeTesAssertionDataUntyped
{
    public Kor OnKorssa { get; }
    public IResult<bool, Exception> Result { get; }
    public string? Description { get; }
}