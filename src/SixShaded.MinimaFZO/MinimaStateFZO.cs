
namespace SixShaded.MinimaFZO;

using MorseCode.ITask;
public record MinimaStateFZO : IStateFZO
{
    private IOption<FZOSource> _initialized;
    private PStack<OperationNode> _opStack;
    private PStack<EKorssaMutation> _prepStack;

    public MinimaStateFZO()
    {
        _opStack = new();
        _prepStack = new();
        _initialized = new None<FZOSource>();
    }

    IEnumerable<IStateFZO.IOperationNode> IStateFZO.OperationStack => _opStack.Elements.Take(_opStack.Count - 1);
    IEnumerable<EKorssaMutation> IStateFZO.KorssaMutationStack => _prepStack.Elements;
    IOption<FZOSource> IStateFZO.Initialized => _initialized;

    IStateFZO IStateFZO.Initialize(FZOSource source)
    {
        if (_initialized.IsSome())
            throw new InvalidOperationException("Attempted initialization of an already initialized IStateFZO");
        return new MinimaStateFZO
        {
            _initialized = _initialized.Some(source),
            _opStack = _opStack.WithEntries(new OperationNode
            {
                Operation = new DummyOperation(),
                MemCount = 1,
                MemoryStack = new PStack<IMemoryFZO>().WithEntries(source.InitialMemory),
                ArgRoggiStack = new(),

            }),
        };
    }

    IStateFZO IStateFZO.WithStep(EProcessorStep step)
    {
        if (_initialized.CheckNone(out var init))
            throw new InvalidOperationException("Attempted operation on an uninitialized IStateFZO");
        return step switch
        {
            EProcessorStep.KorssaMutate v => this with
            {
                _prepStack = _prepStack.WithEntries(v.Mutation),
            },
            EProcessorStep.PushOperation v => this with
            {
                _opStack = _opStack.WithEntries(new OperationNode
                {
                    Operation = v.OperationKorssa,
                    MemoryStack = _opStack.TopValue.Unwrap().MemoryStack,
                    MemCount = 1,
                    ArgRoggiStack = new(),
                }),
                _prepStack = new(),
            },
            EProcessorStep.Resolve v => v.Roggi.Split(out var roggi, out var stateImplemented)
                ? this with
                {
                    _opStack = _opStack.At(1).Expect("No parent operation node?")
                        .MapTopValue(
                            opNode => opNode with
                            {
                                ArgRoggiStack = opNode.ArgRoggiStack.WithEntries(roggi),
                                MemCount = opNode.MemCount.ExprAs(x => roggi.IsSome() ? x + 1 : x),
                                MemoryStack =
                                roggi.Check(out var r)
                                    ? opNode.MemoryStack.WithEntries(
                                        r.Instructions.AccumulateInto(
                                            opNode.MemoryStack.TopValue.Expect("no memory in operation node?"),
                                            (mem, instruction) => instruction.TransformMemoryUnsafe(mem)))
                                    : opNode.MemoryStack,
                            })
                        .IsA<PStack<OperationNode>>(),
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
                                                .WithMellsanoMutes(metaExecute.MellsanoMutes)
                                                .WithoutMellsanoMutes(metaExecute.MellsanoAllows))),
                            }).IsA<PStack<OperationNode>>(),
                        _prepStack = _prepStack.WithEntries(new EKorssaMutation.Identity { Result = metaExecute.Korssa }),
                    },
                    _ => throw new NotSupportedException(),
                },
            _ => throw new NotSupportedException(),
        };
    }

    private record OperationNode : IStateFZO.IOperationNode
    {
        public required int MemCount { get; init; }
        public required PStack<RogOpt> ArgRoggiStack { get; init; }
        public required PStack<IMemoryFZO> MemoryStack { get; init; }
        public required Kor Operation { get; init; }
        IEnumerable<RogOpt> IStateFZO.IOperationNode.ArgRoggiStack => ArgRoggiStack.Elements;
        IEnumerable<IMemoryFZO> IStateFZO.IOperationNode.MemoryStack => MemoryStack.Elements.Take(MemCount);
    }

    private class DummyOperation : Kor
    {
        public Kor[] ArgKorssas => throw new System.Diagnostics.UnreachableException();

        public IResult<ITask<RogOpt>, EStateImplemented> ResolveWith(IProcessorFZO.IKorssaContext runtime, RogOpt[] args) => throw new System.Diagnostics.UnreachableException();
    }
}