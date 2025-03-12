namespace SixShaded.SixLib.ControlledFlows;

public static class Extensions
{
    public static TransformedFlow<I, R> WithTransformedResult<I, R>(
        this ICeasableFlow<I> inputFlow,
        Func<I, R> transform) =>
        new(inputFlow, transform);
}