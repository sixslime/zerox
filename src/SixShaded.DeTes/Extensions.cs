
namespace SixShaded.DeTes;

public static class Extensions
{
    public static Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(this IDeTesTest test, DeTesFZOSupplier supplier) => new DeTesRealizer().Realize(test, supplier);
    public static IKorssa<R> AssertRoggi<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<R> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionRoggi(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> AssertRoggiUnstable<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<IOption<R>> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionRoggiUnstable(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> AssertKorssa<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<IKorssa<R>> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionKorssa(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> AssertMemory<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<IMemoryFZO> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionMemory(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> ReferenceAs<R>(this IKorssa<R> subject, IDeTesContext context, out IDeTesReference<R> reference, string? description = null)
        where R : class, Rog
    {
        context.MakeReference(subject, out reference, description);
        return subject;
    }

    public static IKorssa<R> DefineSelectionDomain<R>(this IKorssa<R> subject, IDeTesContext context, IEnumerable<int> values, out IDeTesSingleDomain domainHandle, string? description = null)
        where R : class, Rog
    {
        context.MakeSingleSelectionDomain(subject, values.ToArray(), out domainHandle, description);
        return subject;
    }

    public static IKorssa<R> DefineSelectionDomain<R>(this IKorssa<R> subject, IDeTesContext context, IEnumerable<IEnumerable<int>> values, out IDeTesMultiDomain domainHandle, string? description = null)
        where R : class, Rog
    {
        context.MakeMultiSelectionDomain(subject, values.Map(x => x.ToArray()).ToArray(), out domainHandle, description);
        return subject;
    }
}