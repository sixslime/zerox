
namespace SixShaded.FourZeroOne.Handles.Defined;

public class TokenContextHandle(FZOSpec.IProcessorFZO.ITokenContext implementation) : ITokenContext
{
    private readonly FZOSpec.IProcessorFZO.ITokenContext _implementation = implementation;
    IMemory ITokenContext.CurrentMemory => _implementation.CurrentMemory.ToHandle();
    IInput ITokenContext.Input => _implementation.Input.ToHandle();

    FZOSpec.IProcessorFZO.ITokenContext ITokenContext.InternalValue => _implementation;
}