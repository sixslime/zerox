using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Token.Unsafe;
using FourZeroOne.Token;
using ResObj = FourZeroOne.Resolution.IResolution;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace DeTes.Declaration
{
    public delegate IToken TokenDeclaration(IDeTesContext C);
    public interface IDeTesTest
    {
        public IMemoryFZO InitialMemory { get; }
        public TokenDeclaration Token { get; }
    }
    public interface IDeTesReference<out R> where R : class, ResObj
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
            where R : class, ResObj;
        public void AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
            where R : class, ResObj;
        public void AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
            where R : class, ResObj;
        public void AddAssertionMemory(IToken subject, Predicate<IMemoryFZO> assertion, string? description);
        public void MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
            where R : class, ResObj;
        public void MakeSingleSelectionDomain(IToken subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description);
        public void MakeMultiSelectionDomain(IToken subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description);
    }
}