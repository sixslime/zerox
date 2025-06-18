namespace SixShaded.VeiledOhOne.Impl;

using FourZeroOne.Core.Roggis;

internal class VeilCreationHandleImpl : IVeilCreationHandle
{
    internal VeilCreationHandleImpl()
    {

    }
    void IVeilCreationHandle.SetKorssaVeil(Kor korssa, int[] observers) => throw new NotImplementedException();
    void IVeilCreationHandle.SetKorssaVeil(Kor korssa, IKorssa<IMulti<Number>> observers) => throw new NotImplementedException();
    void IVeilCreationHandle.SetMemoryVeil(Addr roda, int[] observers) => throw new NotImplementedException();
    void IVeilCreationHandle.SetMemoryVeil(Addr roda, IKorssa<IMulti<Number>> observers) => throw new NotImplementedException();
}