using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using ResOpt = SixShaded.NotRust.IOption<FourZeroOne.Resolution.IResolution>;
using System.Diagnostics;
using FourZeroOne.Token;
using SixLib.GFunc;
using SixShaded.NotRust;
#nullable enable
namespace SixShaded.MinimaFZO
{
    using any_token = IToken<IResolution>;
    public record MinimaStateFZO : IStateFZO
    {
        private IOption<FZOSource> _initialized;
        private PStack<OperationNode> _opStack;
        private PStack<ETokenMutation> _prepStack;

        public MinimaStateFZO()
        {
            _opStack = new();
            _prepStack = new();
            _initialized = new None<FZOSource>();
        }
        IEnumerable<IStateFZO.IOperationNode> IStateFZO.OperationStack => _opStack.Elements.Take(_opStack.Count - 1);
        IEnumerable<ETokenMutation> IStateFZO.TokenMutationStack => _prepStack.Elements;
        IOption<FZOSource> IStateFZO.Initialized => _initialized;
        IStateFZO IStateFZO.Initialize(FZOSource source)
        {
            if (_initialized.IsSome())
                throw new InvalidOperationException("Attempted initialization of an already initialized IStateFZO");
            return new MinimaStateFZO()
            {
                _initialized = _initialized.Some(source),
                _opStack = _opStack.WithEntries(new OperationNode
                {
                    Operation = new DummyOperation(),
                    MemCount = 1,
                    MemoryStack = new PStack<IMemoryFZO>().WithEntries(source.InitialMemory),
                    ArgResolutionStack = new()

                })
            };
        }
        IStateFZO IStateFZO.WithStep(EProcessorStep step)
        {
            if (_initialized.CheckNone(out var init))
                throw new InvalidOperationException("Attempted operation on an uninitialized IStateFZO");
            return step switch
            {
                EProcessorStep.TokenMutate v => this with
                {
                    _prepStack = _prepStack.WithEntries(v.Mutation)
                },
                EProcessorStep.PushOperation v => this with
                {
                    _opStack = _opStack.WithEntries(new OperationNode()
                    {
                        Operation = v.OperationToken,
                        MemoryStack = _opStack.TopValue.Unwrap().MemoryStack,
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
                                MemoryStack = node.MemoryStack.WithEntries(
                                    node.MemoryStack.TopValue.Unwrap().ExprAs(
                                        mem =>
                                        metaExecute.ObjectWrites.ExprAs(
                                            writes =>
                                            mem.WithObjects(writes.FilterMap(x => x.B.RemapAs(r => (x.A, r).Tiple())))
                                            .WithClearedAddresses(writes.FilterMap(x => x.B.IsSome().Not().ToOption(x.A))))
                                        .WithRuleMutes(metaExecute.RuleMutes)
                                        .WithoutRuleMutes(metaExecute.RuleAllows)))
                            }).IsA<PStack<OperationNode>>(),
                            _prepStack = _prepStack.WithEntries(new ETokenMutation.Identity { Result = metaExecute.Token })
                        },
                        _ => throw new NotSupportedException()
                    },
                _ => throw new NotSupportedException(),
            };
        }
        private record OperationNode : IStateFZO.IOperationNode
        {
            public required int MemCount { get; init; }
            public required any_token Operation { get; init; }
            public required PStack<ResOpt> ArgResolutionStack { get; init; }
            public required PStack<IMemoryFZO> MemoryStack { get; init; }
            IEnumerable<ResOpt> IStateFZO.IOperationNode.ArgResolutionStack => ArgResolutionStack.Elements;
            IEnumerable<IMemoryFZO> IStateFZO.IOperationNode.MemoryStack => MemoryStack.Elements.Take(MemCount);
        }
        private class DummyOperation : any_token
        {
            public any_token[] ArgTokens => throw new UnreachableException();
            public IResult<ITask<ResOpt>, EStateImplemented> ResolveWith(IProcessorFZO.ITokenContext runtime, ResOpt[] args)
            {
                throw new UnreachableException();
            }
        }
    }

}