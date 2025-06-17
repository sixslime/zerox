namespace SixShaded.VeiledOhOne;
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
}
