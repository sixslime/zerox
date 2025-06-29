namespace SixShaded.DeTes;

using Declaration.Impl;

public static class Extensions
{
    public static Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(this IDeTesTest test, DeTesFZOSupplier supplier) => new DeTesRealizer().Realize(test, supplier);

    public static IKorssa<R> DeTesAssertRoggi<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<R> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionRoggi(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> DeTesAssertRoggiUnstable<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<IOption<R>> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionRoggiUnstable(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> DeTesAssertKorssa<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<IKorssa<R>> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionKorssa(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> DeTesAssertMemory<R>(this IKorssa<R> subject, IDeTesContext context, Predicate<IMemoryFZO> assertion, string? description = null)
        where R : class, Rog
    {
        context.AddAssertionMemory(subject, assertion, description);
        return subject;
    }

    public static IKorssa<R> DeTesReference<R>(this IKorssa<R> subject, IDeTesContext context, out IDeTesReference<R> reference, string? description = null)
        where R : class, Rog
    {
        context.MakeReference(subject, out reference, description);
        return subject;
    }

    public static IKorssa<R> DeTesDomain<R>(this IKorssa<R> subject, IDeTesContext context, IEnumerable<int> values, out IDeTesSingleDomain domainHandle, string? description = null)
        where R : class, Rog
    {
        context.MakeSingleSelectionDomain(subject, values.ToArray(), out domainHandle, description);
        return subject;
    }

    public static IKorssa<R> DeTesDomain<R>(this IKorssa<R> subject, IDeTesContext context, IEnumerable<IEnumerable<int>> values, out IDeTesMultiDomain domainHandle, string? description = null)
        where R : class, Rog
    {
        context.MakeMultiSelectionDomain(subject, values.Map(x => x.ToArray()).ToArray(), out domainHandle, description);
        return subject;
    }

    /// <summary>
    /// DEV:
    /// Actual declaration korssa will differ; Reference generation is stateful.
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public static Kor GetDeclarationKorssa(this IDeTesTest test) => test.Declaration(new DummyContext());
}