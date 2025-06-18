namespace SixShaded.VeiledOhOne;
using FourZeroOne.Core.Roggis;
public interface IVeilCreationHandle
{
    public void SetKorssaVeil(Kor korssa, int[] observers);
    public void SetKorssaVeil(Kor korssa, IKorssa<IMulti<Number>> observers);
    public void SetMemoryVeil(Addr roda, int[] observers);
    public void SetMemoryVeil(Addr roda, IKorssa<IMulti<Number>> observers);
}