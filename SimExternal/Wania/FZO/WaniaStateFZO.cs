using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using FourZeroOne.Macro.Unsafe;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
using FourZeroOne.Token.Unsafe;
#nullable enable
namespace Wania.FZO
{
    public record WaniaStateFZO : IStateFZO
    {
        private bool _isInitialized;
        private PStack<OperationNode> _opStack;
        private PStack<IMemoryFZO> _memStack;
        private PStack<ETokenPrep> _prepStack;

        public WaniaStateFZO()
        {
            _opStack = new();
            _memStack = new();
            _prepStack = new();
            _isInitialized = false;
        }
        IEnumerable<IStateFZO.IOperationNode> IStateFZO.OperationStack => _opStack.Elements;
        IEnumerable<IMemoryFZO> IStateFZO.MemoryStack => _memStack.Elements;
        IEnumerable<ETokenPrep> IStateFZO.TokenPrepStack => _prepStack.Elements;
        IStateFZO IStateFZO.Initialize(FZOSource source)
        {
            return new WaniaStateFZO()
            {
                _isInitialized = true,
                _opStack = new PStack<OperationNode>().WithEntries(new OperationNode()
                {
                    Operation = source.Program,
                    ResolvedArgs = new()
                }),
                _memStack = new PStack<IMemoryFZO>().WithEntries(source.InitialMemory)
            };
        }
        IStateFZO IStateFZO.WithStep(EDelta step)
        {
            switch (step)
            {
                case EDelta.TokenPrep v:
                    return this with
                    {
                        _prepStack = _prepStack.WithEntries(v.Value)
                    };
                case EDelta.PushOperation v:
                    return this with
                    {
                        _opStack = _opStack.WithEntries(new OperationNode()
                        {
                            Operation = v.OperationToken,
                            ResolvedArgs = new()
                        })
                    };
                case EDelta.Resolve v:
                    return v.Resolution.CheckOk(out var resolution, out var stateImplemented)
                        ? this with
                        {
                            _opStack = _opStack.At(1).Expect("No parent operation node on resolve?")
                                .MapTopValue(x => x with { ResolvedArgs = x.ResolvedArgs.WithEntries(resolution) })
                                .IsA<PStack<OperationNode>>()
                        }
                        : (stateImplemented is EStateImplemented.MetaExecute metaExecute)
                            ? this with
                            {

                            }
                            : throw new NotSupportedException();
                default:
                    throw new NotSupportedException();
            }
        }
        private record OperationNode : IStateFZO.IOperationNode
        {
            public required IToken Operation { get; init; }
            public required PSequence<ResOpt> ResolvedArgs { get; init; }
            IEnumerable<ResOpt> IStateFZO.IOperationNode.ArgResolutionStack => ResolvedArgs.Elements;
        }
    }

}