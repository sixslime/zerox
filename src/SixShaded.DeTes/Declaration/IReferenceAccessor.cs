namespace SixShaded.DeTes.Declaration;

internal interface IReferenceAccessor : ITokenLinked, IHasDescription
{
    void SetResolution(ResOpt resolution);
    void SetToken(Tok token);
    void SetMemory(IMemoryFZO state);
    void Reset();
}