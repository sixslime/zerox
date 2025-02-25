namespace SixShaded.DeTes.Declaration.Impl;
internal interface IReferenceAccessor : IKorssaLinked, IHasDescription
{
    void SetRoggi(RogOpt roggi);
    void SetKorssa(Kor korssa);
    void SetMemory(IMemoryFZO state);
    void Reset();
}