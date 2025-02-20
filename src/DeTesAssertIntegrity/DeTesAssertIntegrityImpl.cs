using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Token;
using Res = FourZeroOne.Resolution.IResolution;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
using DeTes.Declaration;
#nullable enable
namespace DeTesAssertIntegrity
{
    internal class DeTesAssertIntegrityContextProvider : IDeTesContext
    {
        private int _accumulator = 0;
        public DeTesAssertIntegrityContext[] GetSanityContexts(TokenDeclaration declaration)
        {
            try
            {
                _accumulator = 0;
                _ = declaration(this);
                var o = new DeTesAssertIntegrityContext[_accumulator];
                for (int i = 0; i < _accumulator; i++)
                {
                    o[i] = new(i);
                }
                return o;
            }
            catch (Exception ex)
            {
                if (ex is IKnownException) throw;
                throw new InternalDeTesAssertIntegrityException(ex);
            }
        }
        void IDeTesContext.AddAssertionMemory(IToken<Res> subject, Predicate<IMemoryFZO> assertion, string? description)
        {
            _accumulator += 1;
        }
        void IDeTesContext.AddAssertionResolution<R>(IToken<R> subject, Predicate<R> assertion, string? description)
        {
            _accumulator += 1;
        }
        void IDeTesContext.AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
        {
            _accumulator += 1;
        }
        void IDeTesContext.AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
        {
            _accumulator += 1;
        }
        void IDeTesContext.MakeMultiSelectionDomain(IToken<Res> subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description)
        {
            domainHandle = new DeTesAssertIntegrityDummyObject<Res>();
        }
        void IDeTesContext.MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
        {
            reference = new DeTesAssertIntegrityDummyObject<R>();
        }
        void IDeTesContext.MakeSingleSelectionDomain(IToken<Res> subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description)
        {
            domainHandle = new DeTesAssertIntegrityDummyObject<Res>();
        }

    }
    internal class DeTesAssertIntegrityDummyObject<RNon> : IDeTesMultiDomain, IDeTesSingleDomain, IDeTesReference<RNon>
            where RNon : class, Res
    {
        public static readonly DeTesAssertIntegrityDummyObject<RNon> INSTANCE = new();
        IToken<RNon> IDeTesReference<RNon>.Token => throw new UnexpectedDeTesUseException();
        RNon IDeTesReference<RNon>.Resolution => throw new UnexpectedDeTesUseException();
        IOption<RNon> IDeTesReference<RNon>.ResolutionUnstable => throw new UnexpectedDeTesUseException();
        IMemoryFZO IDeTesReference<RNon>.Memory => throw new UnexpectedDeTesUseException();
        int IDeTesSingleDomain.SelectedIndex() => throw new UnexpectedDeTesUseException();
        int[] IDeTesMultiDomain.SelectedIndicies() => throw new UnexpectedDeTesUseException();
    }
    internal class DeTesAssertIntegrityContext(int triggerIndex) : IDeTesContext
    {
        public readonly int TriggerIndex = triggerIndex;
        private int? _accumulator = null;
        private IDeTesContext? _implementingContext = null;

        // very weird flow.
        public IDeTesContext WithImplementingContext(IDeTesContext implementation)
        {
            _accumulator = 0;
            _implementingContext = implementation;
            return this;
        }

        private void Trigger(Action<IDeTesContext> action)
        {
            if (_accumulator is null) return;
            if (_accumulator != TriggerIndex)
            {
                _accumulator++;
                return;
            }
            action(_implementingContext!);
            _implementingContext = null;
            _accumulator = null;
        }
        void IDeTesContext.AddAssertionMemory(IToken<Res> subject, Predicate<IMemoryFZO> assertion, string? description)
        {
            Trigger(C => C.AddAssertionMemory(subject, _ => false, description));
        }
        void IDeTesContext.AddAssertionResolution<R>(IToken<R> subject, Predicate<R> assertion, string? description)
        {
            Trigger(C => C.AddAssertionResolution(subject, _ => false, description));
        }
        void IDeTesContext.AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
        {
            Trigger(C => C.AddAssertionResolutionUnstable(subject, _ => false, description));
        }
        void IDeTesContext.AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
        {
            Trigger(C => C.AddAssertionToken(subject, _ => false, description));
        }
        void IDeTesContext.MakeMultiSelectionDomain(IToken<Res> subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description)
        {
            _implementingContext!.MakeMultiSelectionDomain(subject, selections, out domainHandle, description);
        }
        void IDeTesContext.MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
        {
            _implementingContext!.MakeReference(subject, out reference, description);
        }
        void IDeTesContext.MakeSingleSelectionDomain(IToken<Res> subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description)
        {
            _implementingContext!.MakeSingleSelectionDomain(subject, selections, out domainHandle, description);
        }
    }
    internal interface IKnownException { }
}