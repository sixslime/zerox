namespace SixShaded.VeiledOhOne;

using Impl;

public class Veil
{
    public IStateFZO.IOrigin Origin { get; }
    public int MaxObserverId { get; }
    public Veil(int maxObserverId, Func<IVeilCreationHandle, IStateFZO.IOrigin> originExpression)
    {
        var handle = new VeilCreationHandleImpl();
        Origin = originExpression(handle);
        MaxObserverId = maxObserverId;
    }
}

/* FEATURES
 * setVisibleFor()
 * setHiddenFor()
 * addVisibleFor()
 * addHiddenFor()
 *
 * SYNTAX
 * handle =>
 *
 */