using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using FourZeroOne.Token;
using ResObj = FourZeroOne.Resolution.IResolution;
using ResOpt = SixShaded.NotRust.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace DeTes.Declaration
{
    using Analysis;
    using Realization;
    using SixShaded.NotRust;
    using IToken = IToken<ResObj>;
    internal class ContextImpl : IDeTesContext, IContextAccessor
    {
        private List<IDomainAccessor> _domains = [];
        private List<IReferenceAccessor> _references = [];
        private AssertSet _assertions = new();
        private class AssertSet
        {
            public List<IAssertionAccessor<ResOpt>> Resolution = [];
            public List<IAssertionAccessor<IMemoryFZO>> Memory = [];
            public List<IAssertionAccessor<IToken>> Token = [];
        }
        IDomainAccessor[] IContextAccessor.Domains => _domains.ToArray();
        IReferenceAccessor[] IContextAccessor.References => _references.ToArray();
        IAssertionAccessor<ResOpt>[] IContextAccessor.ResolutionAssertions => _assertions.Resolution.ToArray();
        IAssertionAccessor<IMemoryFZO>[] IContextAccessor.MemoryAssertions => _assertions.Memory.ToArray();
        IAssertionAccessor<IToken>[] IContextAccessor.TokenAssertions => _assertions.Token.ToArray();
        IDeTesContext IContextAccessor.PublicContext => this;

        public void AddAssertionResolution<R>(IToken<R> subject, Predicate<R> assertion, string? description)
            where R : class, ResObj
        {
            _assertions.Resolution.Add(new AssertionImpl<ResOpt>
            {
                Description = description,
                LinkedToken = subject,
                Condition = x => x.Check(out var res)
                    ? res is R r ? assertion(r) : throw new UnexpectedResolutionTypeException(res.GetType(), typeof(R))
                    : throw new UnexpectedNollaException(),
            });
        }
        public void AddAssertionResolutionUnstable<R>(IToken<R> subject, Predicate<IOption<R>> assertion, string? description)
            where R : class, ResObj
        {
            _assertions.Resolution.Add(new AssertionImpl<ResOpt>
            {
                Description = description,
                LinkedToken = subject,
                Condition = x => x is IOption<R> r ? assertion(r) : throw new UnexpectedResolutionTypeException(x.GetType(), typeof(IOption<R>))
            });
        }
        public void AddAssertionToken<R>(IToken<R> subject, Predicate<IToken<R>> assertion, string? description)
            where R : class, ResObj
        {
            _assertions.Token.Add(new AssertionImpl<IToken>
            {
                Description = description,
                LinkedToken = subject,
                Condition = x => assertion((IToken<R>)x)
            });
        }
        public void AddAssertionMemory(IToken subject, Predicate<IMemoryFZO> assertion, string? description)
        {
            _assertions.Memory.Add(new AssertionImpl<IMemoryFZO>
            {
                Description = description,
                LinkedToken = subject,
                Condition = assertion
            });
        }
        public void MakeReference<R>(IToken<R> subject, out IDeTesReference<R> reference, string? description)
            where R : class, ResObj
        {
            var o = new ReferenceImpl<R> { Description = description, LinkedToken = subject };
            reference = o;
            _references.Add(o);
        }
        public void MakeSingleSelectionDomain(IToken subject, int[] selections, out IDeTesSingleDomain domainHandle, string? description)
        {
            MakeSelectionDomain(subject, selections.Map(x => x.Yield().ToArray()).ToArray(), out var impl, description);
            domainHandle = impl;
        }
        public void MakeMultiSelectionDomain(IToken subject, int[][] selections, out IDeTesMultiDomain domainHandle, string? description)
        {
            MakeSelectionDomain(subject, selections, out var impl, description);
            domainHandle = impl;
        }
        private void MakeSelectionDomain(IToken subject, int[][] selections, out DomainImpl impl, string? description)
        {
            impl = new() { LinkedToken = subject, Selections = selections.Map(x => x.ToArray()).ToArray(), Description = description };
            _domains.Add(impl);
        }

    }
    internal interface IContextAccessor
    {
        IDeTesContext PublicContext { get; }
        IDomainAccessor[] Domains { get; }
        IReferenceAccessor[] References { get; }
        IAssertionAccessor<ResOpt>[] ResolutionAssertions { get; }
        IAssertionAccessor<IMemoryFZO>[] MemoryAssertions { get; }
        IAssertionAccessor<IToken>[] TokenAssertions { get; }
    }
    internal interface IReferenceAccessor : ITokenLinked, IHasDescription
    {
        void SetResolution(ResOpt resolution);
        void SetToken(IToken token);
        void SetMemory(IMemoryFZO state);
        void Reset();
    }
    internal interface IDomainAccessor : ITokenLinked, IHasDescription
    {
        int[][] Selections { get; }
        int MetaIndex { get; set; }
    }
    internal interface IAssertionAccessor<in T> : ITokenLinked, IHasDescription
    {
        Predicate<T> Condition { get; }
    }
    internal interface ITokenLinked
    {
        IToken LinkedToken { get; }
    }
    internal interface IHasDescription
    {
        string? Description { get; }
    }
    internal class ReferenceImpl<R> : IReferenceAccessor, IDeTesReference<R> where R : class, ResObj
    {
        private IOption<IOption<R>> _resolution = new None<IOption<R>>();
        private IOption<IMemoryFZO> _memory = new None<IMemoryFZO>();
        private IOption<IToken<R>> _tokenOverride = new None<IToken<R>>();

        public required string? Description { get; init; }
        public required IToken<R> LinkedToken { get; init; }
        IToken ITokenLinked.LinkedToken => LinkedToken;

        IToken<R> IDeTesReference<R>.Token => _tokenOverride.Check(out var v) ? v : LinkedToken;
        R IDeTesReference<R>.Resolution =>
            _resolution.Check(out var v) ? v.DeTesUnwrap() : throw MakeUnevaluatedException();
        IOption<R> IDeTesReference<R>.ResolutionUnstable =>
            _resolution.Check(out var v) ? v : throw MakeUnevaluatedException();
        IMemoryFZO IDeTesReference<R>.Memory =>
            _memory.Check(out var v) ? v : throw MakeUnevaluatedException();

        private DeTesInvalidTestException MakeUnevaluatedException()
        {
            return new()
            {
                Value = new EDeTesInvalidTest.ReferenceUsedBeforeEvaluated()
                {
                    Description = Description,
                    NearToken = LinkedToken,
                }
            };
        }
        void IReferenceAccessor.SetResolution(ResOpt resolution)
        {
            if (resolution is not IOption<R> r) throw new UnexpectedResolutionTypeException(resolution.GetType(), typeof(IOption<R>));
            _resolution = r.AsSome();
        }

        void IReferenceAccessor.SetMemory(IMemoryFZO memory)
        {
            _memory = memory.AsSome();
        }
        void IReferenceAccessor.Reset()
        {
            _resolution = _resolution.None();
            _memory = _memory.None();
            _tokenOverride = _tokenOverride.None();
        }

        void IReferenceAccessor.SetToken(IToken token)
        {
            _tokenOverride = ((IToken<R>)token).AsSome();
        }
    }
    internal class DomainImpl : IDeTesMultiDomain, IDeTesSingleDomain, IDomainAccessor
    {
        private int _index = -1;
        public required int[][] Selections { get; init; }
        public required string? Description { get; init; }
        public required IToken LinkedToken { get; init; }
        int[][] IDomainAccessor.Selections => Selections;
        IToken ITokenLinked.LinkedToken => LinkedToken;
        int IDomainAccessor.MetaIndex { get => _index; set => _index = value; }
        int IDeTesSingleDomain.SelectedIndex() => _index >= 0 ? Selections[_index][0] : throw MakeOutsideScopeException();
        int[] IDeTesMultiDomain.SelectedIndicies() => _index >= 0 ? Selections[_index] : throw MakeOutsideScopeException();
        private DeTesInvalidTestException MakeOutsideScopeException()
        {
            return new()
            {
                Value = new EDeTesInvalidTest.DomainUsedOutsideOfScope
                {
                    Description = Description,
                    NearToken = LinkedToken,
                    Domain = Selections,
                }
            };
        }
    }
    internal class AssertionImpl<T> : IAssertionAccessor<T>
    {
        public required string? Description { get; init; }
        public required IToken LinkedToken { get; init; }
        public required Predicate<T> Condition { get; init; }
        IToken ITokenLinked.LinkedToken => LinkedToken;
        Predicate<T> IAssertionAccessor<T>.Condition => Condition;
    }
    
    internal static class Extensions
    {
        public static T DeTesUnwrap<T>(this IOption<T> option)
        {
            return option.Check(out var v)
                ? v
                : throw new UnexpectedNollaException();
        }
    }
}
