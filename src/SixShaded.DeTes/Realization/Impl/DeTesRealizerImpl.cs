namespace SixShaded.DeTes.Realization.Impl;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SixShaded.DeTes.Analysis.Impl;
using SixShaded.DeTes.Declaration.Impl;

internal class DeTesRealizerImpl
{
    public async Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(IDeTesTest test, IDeTesFZOSupplier supplier)
    {
        IContextAccessor context = new ContextImpl();
        var evalState =
            supplier.UnitializedState.Initialize(
            new StateOrigin
            {
                InitialMemory = test.InitialMemory,
                Program = test.Declaration(context.PublicContext),
            });
        var processor = supplier.Processor;
        try
        {
            return (await Eval(evalState, processor, new(context), new(null)))
                .AsOk(Hint<EDeTesInvalidTest>.HINT);
        }
        catch (DeTesInvalidTestException invalid)
        {
            return Invalid(invalid.Value);
        }
        catch (Exception e)
        {
            throw new DeTesInternalException(e);
        }
    }

    private static async Task<ResultImpl> Eval(IStateFZO state, IProcessorFZO processor, RuntimeResources runtime, Input input)
    {
        List<EDeTesFrame> frames = new();
        CriticalPointType? critPoint = null;
        Stopwatch timer = new();
        timer.Start();
        while (true)
        {
            IResult<EProcessorStep, EProcessorHalt>? processorStep = null;
            try
            {
                processorStep = await processor.GetNextStep(state, input);
            }
            catch (RequiresDomainSplit)
            {
                if (!runtime.DomainStack.TryPop(out var domain))
                {
                    throw new DeTesInvalidTestException
                    {
                        Value =
                            new EDeTesInvalidTest.NoSelectionDomainDefined
                            {
                                SelectionKorssa = state.OperationStack.GetAt(0).Expect("How?").Operation,
                            },
                    };
                }
                var paths = new IDeTesSelectionPath[domain.Selections.Length];
                for (int i = 0; i < paths.Length; i++)
                {
                    int[] thisSelection = domain.Selections[i];
                    domain.MetaIndex = i;
                    var selKorssa = state.OperationStack.First().Operation;

                    // DEBUG
                    //Console.WriteLine(selKorssa);
                    paths[i] =
                        new SelectionPathImpl
                        {
                            RootSelectionKorssa = selKorssa,
                            ResultObject =
                                await Eval(
                                state, processor, runtime, new(
                                new()
                                {
                                    Domain = domain,
                                    Selection = thisSelection,
                                    SelectionKorssa = selKorssa,
                                })),
                            DomainData =
                                new()
                                {
                                    Values = domain.Selections,
                                    Description = domain.Description,
                                },
                            ThisSelection = thisSelection,
                        };
                }
                critPoint = critPoint.Err(paths);
            }
            catch (DeTesInvalidTestException)
            {
                throw;
            }
            catch (Exception e)
            {
                critPoint = critPoint.Ok(e.AsErr(Hint<EProcessorHalt>.HINT));
            }
            if (processorStep is null)
            {
                if (critPoint is null) throw new UnreachableException();
                break;
            }
            if (!processorStep.Split(out var step, out var halt))
            {
                if (halt is EProcessorHalt.Completed complete)
                {
                    var linkedKorssa = runtime.GetLinkedKorssa(GetLastOperation(state));
                    var roggi = complete.Roggi;
                    frames.Add(
                    new EDeTesFrame.Complete
                    {
                        Origin = linkedKorssa,
                        PreState = state,
                        CompletionHalt = complete,
                        Assertions = GenerateOnResolveAssertionObject(runtime, linkedKorssa, roggi, GetMemoryAfterRoggi(state, roggi), GetLastOperation(state)),
                    });
                }
                critPoint = critPoint.Ok(halt.AsOk(Hint<Exception>.HINT));
                break;
            }
            switch (step)
            {
            case EProcessorStep.KorssaMutate v:
                {
                    switch (v.Mutation)
                    {
                    case EKorssaMutation.Identity identity:
                        runtime.PreprocessMap[identity.Result] = identity.Result;
                    break;
                    default:
                        runtime.PreprocessMap[v.Mutation.Result] =
                            runtime.GetLinkedKorssa(state.KorssaMutationStack.Last().IsA<EKorssaMutation.Identity>().Result);
                    break;
                    }
                    frames.Add(
                    new EDeTesFrame.KorssaPrep
                    {
                        Origin = runtime.GetLinkedKorssa(v.Mutation.Result),
                        PreState = state,
                        NextStep = v,
                    });
                }
            break;
            case EProcessorStep.PushOperation v:
                {
                    var linkedKorssa = runtime.GetLinkedKorssa(v.OperationKorssa);
                    frames.Add(
                    new EDeTesFrame.PushOperation
                    {
                        Origin = linkedKorssa,
                        PreState = state,
                        NextStep = v,
                    });
                    if (runtime.Domains.TryGetValue(linkedKorssa, out var domains))
                        foreach (var domain in domains)
                            runtime.DomainStack.Push(domain);
                    if (runtime.References.TryGetValue(linkedKorssa, out var references))
                        foreach (var reference in references)
                            reference.SetKorssa(v.OperationKorssa);
                }
            break;
            case EProcessorStep.Resolve v:
                {
                    var linkedKorssa = runtime.GetLinkedKorssa(GetLastOperation(state));

                    // DEBUG
                    //Console.WriteLine($"- {linkedKorssa}: {v.Roggi}");
                    //runtime.RoggiAssertions.TryGetValue(linkedKorssa, out var ttt);
                    //Console.WriteLine($"::: {ttt?.Count}");
                    if (v.Roggi.Split(out var roggi, out var stateImplemented))
                    {
                        var nMemory = GetMemoryAfterRoggi(state, roggi);
                        var nKorssa = GetLastOperation(state);
                        frames.Add(
                        new EDeTesFrame.Resolve
                        {
                            Origin = linkedKorssa,
                            PreState = state,
                            NextStep = v,
                            Assertions = GenerateOnResolveAssertionObject(runtime, linkedKorssa, roggi, nMemory, nKorssa),
                        });
                        if (runtime.References.TryGetValue(linkedKorssa, out var references))
                        {
                            foreach (var reference in references)
                            {
                                reference.SetRoggi(roggi);
                                reference.SetMemory(nMemory);
                                reference.SetKorssa(nKorssa);
                            }
                        }
                    }
                    else
                    {
                        switch (stateImplemented)
                        {
                        case EStateImplemented.MetaExecute metaExecute:
                            runtime.PreprocessMap[metaExecute.Korssa] = linkedKorssa;
                        break;
                        default:
                            throw new NotSupportedException();
                        }
                    }
                }
            break;
            default:
                throw new NotSupportedException();
            }
            state = state.WithStep(step);
        }
        timer.Stop();
        if (critPoint is null) throw new UnreachableException();
        return new()
        {
            TimeTaken = timer.Elapsed,
            CriticalPoint = critPoint,
            EvaluationFrames = frames.ToArray(),
        };
    }

    private static Kor GetLastOperation(IStateFZO state) => state.OperationStack.First().Operation;

    private static IMemoryFZO GetMemoryAfterRoggi(IStateFZO state, RogOpt roggi) =>
        (state.OperationStack.GetAt(1).Check(out var node)
            ? node.MemoryStack.First()
            : state.Initialized.Unwrap().InitialMemory)
        .WithRoggi(roggi);

    private static OnResolveAssertionsImpl GenerateOnResolveAssertionObject(RuntimeResources runtime, Kor linkedKorssa, RogOpt roggi, IMemoryFZO nMemory, Kor korssa) =>
        new()
        {
            Roggi =
                runtime.RoggiAssertions
                    .TryGetValue(linkedKorssa, out var roggiAssertions)
                    .ToOption(roggiAssertions)
                    .Or([])!
                    .Map(assertion => EvaluateAssertion(assertion, linkedKorssa, roggi))
                    .ToArray<IDeTesAssertionData<RogOpt>>(),
            Memory =
                runtime.MemoryAssertions
                    .TryGetValue(linkedKorssa, out var memoryAssertions)
                    .ToOption(memoryAssertions)
                    .Or([])!
                    .Map(
                    assertion =>
                        EvaluateAssertion(assertion, linkedKorssa, nMemory))
                    .ToArray<IDeTesAssertionData<IMemoryFZO>>(),
            Korssa =
                runtime.KorssaAssertions
                    .TryGetValue(linkedKorssa, out var korssaAssertions)
                    .ToOption(korssaAssertions)
                    .Or([])!
                    .Map(
                    assertion =>
                        EvaluateAssertion(assertion, linkedKorssa, korssa))
                    .ToArray<IDeTesAssertionData<Kor>>(),
        };

    private static AssertionDataImpl<A> EvaluateAssertion<A>(IAssertionAccessor<A> assertion, Kor linkedKorssa, A value)
    {
        IResult<bool, Exception>? result = null;
        try
        {
            result = result.Ok(assertion.Condition(value));
        }
        catch (DeTesInvalidTestException)
        {
            throw;
        }
        catch (Exception e)
        {
            result = result.Err(e);
        }
        return new()
        {
            OnKorssa = linkedKorssa,
            Condition = assertion.Condition,
            Description = assertion.Description,
            Result = result,
        };
    }

    private static Err<IDeTesResult, EDeTesInvalidTest> Invalid(EDeTesInvalidTest val) => new(val);

    private class StateOrigin : IStateFZO.IOrigin
    {
        public required Kor Program { get; init; }
        public required IMemoryFZO InitialMemory { get; init; }
    }

    private class RuntimeResources(IContextAccessor context)
    {
        public readonly Dictionary<Kor, List<IDomainAccessor>> Domains = MakeKorssaLinkDictionary(context.Domains);
        public readonly Stack<IDomainAccessor> DomainStack = new();
        public readonly Dictionary<Kor, List<IAssertionAccessor<Kor>>> KorssaAssertions = MakeKorssaLinkDictionary(context.KorssaAssertions);
        public readonly Dictionary<Kor, List<IAssertionAccessor<IMemoryFZO>>> MemoryAssertions = MakeKorssaLinkDictionary(context.MemoryAssertions);
        public readonly Dictionary<Kor, Kor> MetaExecuteMap = new(new EqualityByReference());
        public readonly Dictionary<Kor, Kor> PreprocessMap = new(new EqualityByReference());
        public readonly Dictionary<Kor, List<IReferenceAccessor>> References = MakeKorssaLinkDictionary(context.References);
        public readonly Dictionary<Kor, List<IAssertionAccessor<RogOpt>>> RoggiAssertions = MakeKorssaLinkDictionary(context.RoggiAssertions);
        public Kor GetLinkedKorssaOld(Kor korssa) => PreprocessMap[korssa].ExprAs(preV => MetaExecuteMap.GetValueOrDefault(preV, preV));
        public Kor GetLinkedKorssa(Kor korssa) => PreprocessMap[korssa];

        private static Dictionary<Kor, List<A>> MakeKorssaLinkDictionary<A>(IEnumerable<A> accessors)
            where A : IKorssaLinked
        {
            var o = new Dictionary<Kor, List<A>>(new EqualityByReference());
            foreach (var a in accessors)
            {
                if (o.TryGetValue(a.LinkedKorssa, out var list)) list.Add(a);
                else o[a.LinkedKorssa] = [a];
            }
            return o;
        }
    }

    private class Input(Input.Data? data) : IInputFZO
    {
        private Data? _data = data;

        Task<int[]> IInputFZO.GetSelection(Rog[] pool, int minCount, int maxCount)
        {
            if (_data is null) throw new RequiresDomainSplit();
            var data = _data;
            _data = null;
            if (data.Selection.Length > maxCount || data.Selection.Length < minCount || data.Selection.Any(i => i >= pool.Length))
            {
                throw new DeTesInvalidTestException
                {
                    Value =
                        new EDeTesInvalidTest.InvalidDomainSelection
                        {
                            InvalidSelection = data.Selection,
                            ExpectedSelectionSize = (minCount, maxCount),
                            ExpectedMaxIndex = pool.Length - 1,
                            SelectionKorssa = data.SelectionKorssa,
                            NearKorssa = data.Domain.LinkedKorssa,
                            Description = data.Domain.Description,
                            Domain = data.Domain.Selections,
                        },
                };
            }
            return data.Selection.ToCompletedTask();
        }

        public class Data
        {
            public required int[] Selection { get; init; }
            public required IDomainAccessor Domain { get; init; }
            public required Kor SelectionKorssa { get; init; }
        }
    }

    private class RequiresDomainSplit : Exception
    { }

    private class EqualityByReference : EqualityComparer<object>
    {
        public override bool Equals(object? x, object? y) => ReferenceEquals(x, y);
        public override int GetHashCode([DisallowNull] object obj) => obj.GetHashCode();
    }
}