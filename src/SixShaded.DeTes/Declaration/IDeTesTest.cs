namespace SixShaded.DeTes.Declaration;

public interface IDeTesTest
{
    public IMemoryFZO InitialMemory { get; }
    public TokenDeclaration Token { get; }
}