namespace SixShaded.FourZeroOne.Handles.Defined;

public class KorssaContextHandle(FZOSpec.IProcessorFZO.IKorssaContext implementation) : IKorssaContext
{
    private readonly FZOSpec.IProcessorFZO.IKorssaContext _implementation = implementation;
    IMemory IKorssaContext.CurrentMemory => _implementation.CurrentMemory.ToHandle();
    IInput IKorssaContext.Input => _implementation.Input.ToHandle();
    FZOSpec.IProcessorFZO.IKorssaContext IKorssaContext.InternalValue => _implementation;
}