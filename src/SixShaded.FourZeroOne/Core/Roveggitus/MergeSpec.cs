namespace SixShaded.FourZeroOne.Core.Roveggitus;

// not even a roggi, consider moving.
public record MergeSpec<C> : IRoveggitu where C : IRoveggitu
{
    public static MergeRovu<R> MERGE<R>(IRovu<C, R> component) where R : class, Rog => new(component);

    public interface IMergeRovu
    {
        public Roggi.Unsafe.IRovu<C> ForRovuUnsafe { get; }
    }

    public record MergeRovu<R>(IRovu<C, R> ForRovu) : IRovu<MergeSpec<C>, R>, IMergeRovu
        where R : class, Rog
    {
        public Axodu Axodu => Core.Axoi.Du;
        public string Identifier => $"{ForRovu.Axodu}~{ForRovu.Identifier}";
        public string TypeExpression => $"MergeRovu<{typeof(C).Name}>";
        Roggi.Unsafe.IRovu<C> IMergeRovu.ForRovuUnsafe => ForRovu;
        public override string ToString() => $"{ForRovu.Identifier}*";
    }
}