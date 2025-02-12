using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using FourZeroOne.Macro.Unsafe;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
using FourZeroOne.Token.Unsafe;
using LookNicePls;
#nullable enable
namespace Minima.FZO
{
    public record MinimaStateFZO : IStateFZO
    {
        private IOption<FZOSource> _initialized;
        private PStack<OperationNode> _opStack;
        private PStack<ETokenPrep> _prepStack;

        public MinimaStateFZO()
        {
            _opStack = new();
            _prepStack = new();
            _initialized = new None<FZOSource>();
        }
        IEnumerable<IStateFZO.IOperationNode> IStateFZO.OperationStack => _opStack.Elements;
        IEnumerable<ETokenPrep> IStateFZO.TokenPrepStack => _prepStack.Elements;
        IOption<FZOSource> IStateFZO.Initialized => _initialized;
        IStateFZO IStateFZO.Initialize(FZOSource source)
        {
            if (_initialized.IsSome())
                throw new InvalidOperationException("Attempted initialization of an already initialized IStateFZO");
            return new MinimaStateFZO()
            {
                _initialized = _initialized.Some(source)
            };
        }
        IStateFZO IStateFZO.WithStep(EProcessorStep step)
        {
            if (_initialized.CheckNone(out var init))
                throw new InvalidOperationException("Attempted operation on an uninitialized IStateFZO");
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
                        MemoryStack = 
                            _opStack.TopValue.RemapAs(x => x.MemoryStack)
                            .Or(new PStack<IMemoryFZO>().WithEntries(init.InitialMemory)),
                        MemCount = 1,
                        ArgResolutionStack = new(),
                    }),
                    _prepStack = new()
                },
                EProcessorStep.Resolve v => v.Resolution.Split(out var resolution, out var stateImplemented)
                    ? this with
                    {
                        _opStack = _opStack.At(1).Expect("No parent operation node?")
                            .MapTopValue(
                                opNode => opNode with
                                {
                                    ArgResolutionStack = opNode.ArgResolutionStack.WithEntries(resolution),
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
                            _opStack = _opStack.At(1).Expect("No parent operation node?")
                            .MapTopValue(node => node with
                            {
                                MemoryStack = node.MemoryStack.MapTopValue(
                                    mem => metaExecute.MemoryWrites.ExprAs(
                                        writes => mem.WithObjects(
                                                writes.FilterMap(x => x.B.RemapAs(r => (x.A, r).Tiple())))
                                            .WithClearedAddresses(writes.FilterMap(x => x.B.IsSome().Not().ToOption(x.A)))))
                                .IsA<PStack<IMemoryFZO>>()
                            }).IsA<PStack<OperationNode>>(),

                            _prepStack = _prepStack.WithEntries(new ETokenPrep.Identity { Result = metaExecute.FunctionToken })
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
            public required PStack<ResOpt> ArgResolutionStack { get; init; }
            public required PStack<IMemoryFZO> MemoryStack { get; init; }
            IEnumerable<ResOpt> IStateFZO.IOperationNode.ArgResolutionStack => ArgResolutionStack.Elements;
            IEnumerable<IMemoryFZO> IStateFZO.IOperationNode.MemoryStack => MemoryStack.Elements.Take(MemCount);
        }
    }

}