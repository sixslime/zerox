namespace SixShaded.VeiledOhOne;
using FourZeroOne.Core.Roggis;
public static class Extensions
{
    public static Task<IResult<EProcessorStep, EProcessorHalt>> GetNextStepVeiled(this IProcessorFZO processsor, IStateFZO state, IInputFZO input, Veil veil, IMetaResolver metaResolver)
    {
        throw new NotImplementedException();
    }

    public static IVeiledState Veiled(this IStateFZO state, Veil veil)
    {
        throw new NotImplementedException();
    }
    public static Kor Veil(this Kor korssa, IVeilCreationHandle handle, EVeilOperation operation, params int[] observers)
    {
        return korssa;
    }
    public static Kor Veil(this Kor korssa, IVeilCreationHandle handle, EVeilOperation operation, IKorssa<IMulti<Number>> observers)
    {
        return korssa;
    }
    public static Addr VeilMemory(this Addr roda, IVeilCreationHandle handle, EVeilOperation operation, params int[] observers)
    {
        return roda;
    }
    public static Addr VeilMemory(this Addr roda, IVeilCreationHandle handle, EVeilOperation operation, IKorssa<IMulti<Number>> observers)
    {
        return roda;
    }
}
