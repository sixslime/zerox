namespace SixShaded.FourZeroOne.Core.Rovetus;

using Roveggi;
using Roveggi.Unsafe;

public record MergeSpec<C> : IRovetu
    where C : IRovetu
{
    public static MergeRovu<R> MERGE<R>(IRovu<C, R> component)
        where R : class, Rog =>
        new(component);

    public interface IMergeRovu
    {
        public IRovu<C> ForRovuUnsafe { get; }
    }

    public record MergeRovu<R>(IRovu<C, R> ForRovu) : IRovu<MergeSpec<C>, R>, IMergeRovu
        where R : class, Rog
    {
        public override string ToString() => $"{ForRovu}*";
        IRovu<C> IMergeRovu.ForRovuUnsafe => ForRovu;
    }
}