namespace SixShaded.VeiledOhOne;
using FourZeroOne.FZOSpec;
using FourZeroOne.Core.Roggis;

public interface IMetaResolver
{
    public IStateFZO UnintializedState { get; }
    public Task<IOption<Number>> Resolve(IStateFZO state, Veil veil);
}
