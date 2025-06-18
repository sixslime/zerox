namespace SixShaded.VeiledOhOne;
using FourZeroOne.Core.Roggis;
public interface IVeilCreationHandle
{
    public void SetKorssaVeil(Kor korssa, EVeilOperation operation, int[] observers);
    public void SetKorssaVeil(Kor korssa, EVeilOperation operation, IKorssa<IMulti<Number>> observers);
    public void SetMemoryVeil(Addr roda, EVeilOperation operation, int[] observers);
    public void SetMemoryVeil(Addr roda, EVeilOperation operation, IKorssa<IMulti<Number>> observers);
}