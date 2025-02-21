using FourZeroOne.FZOSpec;
using Res = FourZeroOne.Resolution.IResolution;
using ResOpt = SixShaded.NotRust.IOption<FourZeroOne.Resolution.IResolution>;
using SixShaded.NotRust;
using SixShaded.FourZeroOne;
#nullable enable
namespace SixShaded.DeTes.Declaration
{
    using IToken = IToken<Res>;
    public delegate IToken TokenDeclaration(IDeTesContext C);
    public interface IDeTesTest
    {
        public IMemoryFZO InitialMemory { get; }
        public TokenDeclaration Token { get; }
    }
    public interface IDeTesReference<out R> where R : class, Res
    {
        public IToken<R> Token { get; }
        public R Resolution { get; }
        public IOption<R> ResolutionUnstable { get; }
        public IMemoryFZO Memory { get; }
    }
    public interface IDeTesSingleDomain
    {
        public int SelectedIndex();
    }
    public interface IDeTesMultiDomain
    {
        public int[] SelectedIndicies();
    }
    public interface IDeTesContext
    {
        public void AddAssertionResolution<R>(IToken<R> subject, Predicate<R> assertion, string? description)
            where R : class, Res;
        public void AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
            where R : class, Res;
        public void AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
            where R : class, Res;
        public void AddAssertionMemory(IToken subject, Predicate<IMemoryFZO> assertion, string? description);
        public void MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
            where R : class, Res;
        public void MakeSingleSelectionDomain(IToken subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description);
        public void MakeMultiSelectionDomain(IToken subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description);
    }
}