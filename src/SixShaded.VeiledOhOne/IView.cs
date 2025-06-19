namespace SixShaded.VeiledOhOne;

public interface IStateView : IView
{
    public IOriginView Origin { get; }
    public IEnumerable<IShownOrHidden<IOperationNodeView>> OperationStack { get; }
    public IShownOrHidden<IEnumerable<EKorssaMutation>> KorssaMutationStack { get; }

}

public interface IOriginView : IView
{
    public IKorssaView Program { get; }
    public IMemoryView InitialMemory { get; }
}
public interface IOperationNodeView : IView
{
    public IKorssaView Operation { get; }
    public IEnumerable<IShownOrHidden<RogOpt>> ArgRoggiStack { get; }
    public IEnumerable<IShownOrHidden<IMemoryView>> MemoryStack { get; }

}
public interface IMemoryView : IView
{
    public IEnumerable<ITiple<Addr, IShownOrHidden<Rog>>> Objects { get; }
    public IEnumerable<IShownOrHidden<Mel>> Mellsanos { get; }
    public IEnumerable<ITiple<MellsanoID, int>> MellsanoMutes { get; }
    public IOption<IShownOrHidden<R>> GetObject<R>(IRoda<R> roda)
        where R : class, Rog;
    public int GetMellsanoMuteCount(MellsanoID mellsanoId);
}

public interface IKorssaView : IView
{
    public Type KorssaType { get; }
    public IShownOrHidden<IKorssaView>[] ArgKorssas { get; }
}
public interface IShownOrHidden<out T>;

public interface IHidden<out T> : IShownOrHidden<T>;

public interface IShown<out T> : IShownOrHidden<T>
{
    public T Value { get; }
}

public interface IView
{
    public int Observer { get; }
}