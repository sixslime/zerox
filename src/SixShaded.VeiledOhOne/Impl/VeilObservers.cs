namespace SixShaded.VeiledOhOne.Impl;

internal record VeilObservers
{
    public required EVeilOperation Operation { get; init; }
    public required IResult<int[], IKorssa<IMulti<FourZeroOne.Core.Roggis.Number>>> Observers { get; init; }

}