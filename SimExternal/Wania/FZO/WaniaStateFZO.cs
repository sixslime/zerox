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
        private PStack<ETokenPrep> _prepStack;

        public WaniaStateFZO()
        {
            _opStack = new();
            _prepStack = new();
            _isInitialized = false;
        }
        IEnumerable<IStateFZO.IOperationNode> IStateFZO.OperationStack => _opStack.Elements;
        IEnumerable<ETokenPrep> IStateFZO.TokenPrepStack => _prepStack.Elements;
        IStateFZO IStateFZO.Initialize(FZOSource source)
        {
            return new WaniaStateFZO()
            {
                _isInitialized = true,
                _opStack = new PStack<OperationNode>().WithEntries(new OperationNode()
                {
                    Operation = source.Program,
                    MemoryStack = new PStack<IMemoryFZO>().WithEntries(source.InitialMemory),
                    MemCount = 1,
                    ResolvedArgs = new()
                })
            };
        }
        IStateFZO IStateFZO.WithStep(EProcessorStep step)
        {
            if (!_isInitialized) throw new InvalidOperationException("Operation on an uninitialized state");
            return step switch
            {
                EProcessorStep.TokenPrep v => this with
                {
                    _prepStack = _prepStack.WithEntries(v.Value)
                },
                EProcessorStep.PushOperation v => this with
                {
                    _opStack = _opStack.WithEntries(new OperationNode()
                    {
                        Operation = v.OperationToken,
                        MemoryStack = _opStack.TopValue.Expect("No parent operation node?").MemoryStack,
                        MemCount = 1,
                        ResolvedArgs = new(),
                    })
                },
                EProcessorStep.Resolve v => v.Resolution.CheckOk(out var resolution, out var stateImplemented)
                    ? this with
                    {
                        _opStack = _opStack.At(1).Expect("No parent operation node?")
                            .MapTopValue(
                                opNode => opNode with
                                {
                                    ResolvedArgs = opNode.ResolvedArgs.WithEntries(resolution),
                                    MemCount = opNode.MemCount.ExprAs(x => resolution.IsSome() ? x + 1 : x),
                                    MemoryStack =
                                        resolution.Check(out var r)
                                        ? opNode.MemoryStack.WithEntries(
                                            r.Instructions.AccumulateInto(
                                                opNode.MemoryStack.TopValue.Expect("no memory in operation node?"),
                                                (mem, instruction) => instruction.TransformMemoryUnsafe(mem)))
                                        : opNode.MemoryStack
                                })
                            .IsA<PStack<OperationNode>>()
                    }
                    : stateImplemented switch
                    {
                        EStateImplemented.MetaExecute metaExecute => this with
                        {
                            _opStack = _opStack.WithEntries(new OperationNode()
                            {
                                Operation = metaExecute.FunctionToken,
                                MemoryStack = _opStack.TopValue.Expect("No parent operation node?").MemoryStack,
                                MemCount = 1,
                                ResolvedArgs = new(),
                            })
                        },
                        _ => throw new NotSupportedException()
                    },
                _ => throw new NotSupportedException(),
            };
        }
        private record OperationNode : IStateFZO.IOperationNode
        {
            public required int MemCount { get; init; }
            public required IToken Operation { get; init; }
            public required PStack<ResOpt> ResolvedArgs { get; init; }
            public required PStack<IMemoryFZO> MemoryStack { get; init; }
            IEnumerable<ResOpt> IStateFZO.IOperationNode.ArgResolutionStack => ResolvedArgs.Elements;
            IEnumerable<IMemoryFZO> IStateFZO.IOperationNode.MemoryStack => MemoryStack.Elements.Take(MemCount);
        }
    }

}