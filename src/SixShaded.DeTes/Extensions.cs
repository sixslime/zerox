
namespace SixShaded.DeTes;

public static class Extensions
{
    public static Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(this IDeTesTest test, DeTesFZOSupplier supplier)
    {
        return new DeTesRealizer().Realize(test, supplier);
    }
    public static bool Passed<A>(this IDeTesAssertionData<A> assertion)
    {
        return assertion.Result.KeepOk().Or(false);
    }
    public static IToken<R> AssertResolution<R>(this IToken<R> subject, IDeTesContext context, Predicate<R> assertion, string? description = null)
        where R : class, Res
    {
        context.AddAssertionResolution(subject, assertion, description);
        return subject;
    }
    public static IToken<R> AssertResolutionUnstable<R>(this IToken<R> subject, IDeTesContext context, Predicate<IOption<R>> assertion, string? description = null)
        where R : class, Res
    {
        context.AddAssertionResolutionUnstable(subject, assertion, description);
        return subject;
    }
    public static IToken<R> AssertToken<R>(this IToken<R> subject, IDeTesContext context, Predicate<IToken<R>> assertion, string? description = null)
        where R : class, Res
    {
        context.AddAssertionToken(subject, assertion, description);
        return subject;
    }
    public static IToken<R> AssertMemory<R>(this IToken<R> subject, IDeTesContext context, Predicate<IMemoryFZO> assertion, string? description = null)
        where R : class, Res
    {
        context.AddAssertionMemory(subject, assertion, description);
        return subject;
    }
    public static IToken<R> ReferenceAs<R>(this IToken<R> subject, IDeTesContext context, out IDeTesReference<R> reference, string? description = null)
        where R : class, Res
    {
        context.MakeReference(subject, out reference, description);
        return subject;
    }
    public static IToken<R> DefineSelectionDomain<R>(this IToken<R> subject, IDeTesContext context, IEnumerable<int> values, out IDeTesSingleDomain domainHandle, string? description = null)
        where R : class, Res
    {
        context.MakeSingleSelectionDomain(subject, values.ToArray(), out domainHandle, description);
        return subject;
    }
    public static IToken<R> DefineSelectionDomain<R>(this IToken<R> subject, IDeTesContext context, IEnumerable<IEnumerable<int>> values, out IDeTesMultiDomain domainHandle, string? description = null)
        where R : class, Res
    {
        context.MakeMultiSelectionDomain(subject, values.Map(x => x.ToArray()).ToArray(), out domainHandle, description);
        return subject;
    }
}