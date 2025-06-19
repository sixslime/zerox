namespace SixShaded.VeiledOhOne.Impl;

using FourZeroOne.Core.Roggis;

internal class VeilCreationHandleImpl : IVeilCreationHandle
{
    internal Dictionary<Kor, VeilObservers> KorssaVeils { get; private set; }
    internal Dictionary<Addr, VeilObservers> RodaVeils { get; private set; }
    internal VeilCreationHandleImpl()
    {
        KorssaVeils = new();
        RodaVeils = new();
    }
    public void SetKorssaVeil(Kor korssa, EVeilOperation operation, int[] observers)
    {
        KorssaVeils[korssa] =
            new()
            {
                Operation = operation,
                Observers = observers.AsOk(Hint<IKorssa<IMulti<Number>>>.HINT),
            };
    }
    public void SetKorssaVeil(Kor korssa, EVeilOperation operation, IKorssa<IMulti<Number>> observers)
    {
        KorssaVeils[korssa] =
            new()
            {
                Operation = operation,
                Observers = observers.AsErr(Hint<int[]>.HINT),
            };
    }
    public void SetMemoryVeil(Addr roda, EVeilOperation operation, int[] observers)
    {
        RodaVeils[roda] =
            new()
            {
                Operation = operation,
                Observers = observers.AsOk(Hint<IKorssa<IMulti<Number>>>.HINT),
            };
    }
    public void SetMemoryVeil(Addr roda, EVeilOperation operation, IKorssa<IMulti<Number>> observers)
    {
        RodaVeils[roda] =
            new()
            {
                Operation = operation,
                Observers = observers.AsErr(Hint<int[]>.HINT),
            };
    }

}