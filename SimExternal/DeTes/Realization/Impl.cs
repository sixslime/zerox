using FourZeroOne.FZOSpec;
using FourZeroOne.Token.Unsafe;
using MorseCode.ITask;
using Perfection;
using ResObj = FourZeroOne.Resolution.IResolution;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace DeTes.Realization
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Analysis;
    using FourZeroOne.FZOSpec.Shorthands;
    using Declaration;
    using CriticalPointType = IResult<IResult<EProcessorHalt, Exception>, Analysis.IDeTesSelectionPath[]>;

    internal class DeTesRealizerImpl
    {
        public async ITask<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(IDeTesTest test, IDeTesFZOSupplier supplier)
        {
            IContextAccessor context = new ContextImpl();
            var evalState = supplier.UnitializedState.Initialize(new FZOSource()
            {
                InitialMemory = test.InitialMemory,
                Program = test.Token(context.PublicContext)
            });
            var processor = supplier.Processor;
            try
            {
                return (await Eval(evalState, processor, new(context), new(null)))
                    .AsOk(Hint<EDeTesInvalidTest>.HINT);
            }
            catch (InvalidTestException invalid) { return Invalid(invalid.Value); }
            catch (Exception e) { throw new DeTesInternalException(e); }

        }

        // WARNING: can be optimized, particularly with code elegance/repitition
        private static async ITask<ResultImpl> Eval(IStateFZO state, IProcessorFZO processor, RuntimeResources runtime, Input input)
        {
            List<EDeTesFrame> frames = new();
            CriticalPointType? critPoint = null;

            while (true)
            {
                IResult<EProcessorStep, EProcessorHalt>? processorStep = null;

                // get next step:
                try { processorStep = await processor.GetNextStep(state, input); }
                // create domain paths recursively if requested:
                catch (RequiresDomainSplit)
                {
                    if (!runtime.DomainQueue.TryDequeue(out var domain))
                        throw new InvalidTestException
                        {
                            Value = new EDeTesInvalidTest.NoSelectionDomainDefined
                            {
                                SelectionToken = state.OperationStack.GetAt(0).Expect("How?").Operation
                            }
                        };
                    var paths = new IDeTesSelectionPath[domain.Selections.Length]; 
                    for (int i = 0; i < paths.Length; i++)
                    {
                        var thisSelection = domain.Selections[i];
                        paths[i] = new SelectionPathImpl
                        {
                            ResultObject = await Eval(state, processor, runtime, new(new()
                            {
                                Domain = domain,
                                Selection = thisSelection,
                                SelectionToken = state.OperationStack.First().Operation
                            })),
                            DomainData = new()
                            {
                                Values = domain.Selections,
                                Description = domain.Description,
                            },
                            ThisSelection = thisSelection,
                        };
                    }
                    critPoint = critPoint.Err(paths);
                }
                // send invalid test exceptions up:
                catch (InvalidTestException) { throw; }
                // catch non-DeTes exceptions and store it in critical point:
                catch (Exception e) { critPoint = critPoint.Ok(e.AsErr(Hint<EProcessorHalt>.HINT)); }

                // break if 'processorStep' isn't defined, in which case 'critPoint' must be:
                if (processorStep is null) if (critPoint is null) throw new UnreachableException();
                    else break;

                // set 'critPoint' and break if halt:
                if (!processorStep.Split(out var step, out var halt))
                {
                    if (halt is EProcessorHalt.Completed complete)
                    {
                        var resolution = complete.Resolution;
                        frames.Add(new EDeTesFrame.Complete
                        {
                            PreState = state,
                            CompletionHalt = complete,
                            Assertions = GenerateOnResolveAssertionObject(runtime, runtime.GetLinkedToken(GetLastOperation(state)), resolution, GetMemoryAfterResolution(state, resolution))
                        });
                    }
                    
                    critPoint = critPoint.Ok(halt.AsOk(Hint<Exception>.HINT));
                    break;
                }

                // main runtime/processor loop logic:
                switch (step)
                {
                    case EProcessorStep.TokenPrep v:
                        {
                            frames.Add(new EDeTesFrame.TokenPrep
                            {
                                PreState = state,
                                NextStep = v
                            });
                            //tokenmap can get pretty large because its just 1 mutable object.
                            switch (v.Value)
                            {
                                case ETokenPrep.Identity identity:
                                    runtime.PreprocessMap[identity.Result] = identity.Result;
                                    break;
                                default:
                                    // kinda inefficient but does the alternative is using
                                    // potentially unproven cache assumptions.
                                    runtime.PreprocessMap[v.Value.Result] =
                                        state.TokenPrepStack.Last().IsA<ETokenPrep.Identity>().Result;
                                    break;
                            }
                        }
                        break;
                    //FIXME: does not run for the initial program token
                    case EProcessorStep.PushOperation v:
                        {
                            var linkedToken = runtime.GetLinkedToken(v.OperationToken);
                            frames.Add(new EDeTesFrame.PushOperation
                            {
                                PreState = state,
                                NextStep = v,
                                Assertions = GenerateOnPushAssertionObject(runtime, linkedToken, v.OperationToken)
                            });
                            if (runtime.Domains.TryGetValue(linkedToken, out var domains))
                                foreach (var domain in domains) runtime.DomainQueue.Enqueue(domain);
                            if (runtime.References.TryGetValue(linkedToken, out var references))
                                foreach (var reference in references) reference.SetToken(v.OperationToken);
                        }
                        break;
                    case EProcessorStep.Resolve v:
                        {
                            var linkedToken = runtime.GetLinkedToken(GetLastOperation(state));
                            if (v.Resolution.Split(out var resolution, out var stateImplemented))
                            {
                                var nMemory = GetMemoryAfterResolution(state, resolution);
                                frames.Add(new EDeTesFrame.Resolve
                                {
                                    PreState = state,
                                    NextStep = v,
                                    Assertions = GenerateOnResolveAssertionObject(runtime, linkedToken, resolution, nMemory)
                                });
                                if (runtime.References.TryGetValue(linkedToken, out var references))
                                    foreach (var reference in references)
                                    {
                                        reference.SetResolution(resolution);
                                        reference.SetMemory(nMemory);
                                    }
                            }
                            else
                            {
                                switch (stateImplemented)
                                {
                                    case EStateImplemented.MetaExecute metaExecute:
                                        runtime.MetaExecuteMap[metaExecute.FunctionToken] = linkedToken;
                                        runtime.PreprocessMap[metaExecute.FunctionToken] = metaExecute.FunctionToken;
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
            // 'critPoint' must have a value after the above loop:
            if (critPoint is null) throw new UnreachableException();
            return new()
            {
                CriticalPoint = critPoint,
                EvaluationFrames = frames.ToArray()
            };
        }
        private static IToken GetLastOperation(IStateFZO state) => state.OperationStack.First().Operation;
        private static IMemoryFZO GetMemoryAfterResolution(IStateFZO state, ResOpt resolution)
        {
            return (state.OperationStack.GetAt(1).Check(out var node)
                ? node.MemoryStack.First()
                : state.Initialized.Unwrap().InitialMemory)
                .WithResolution(resolution);
        }
        private static OnPushAssertionsImpl GenerateOnPushAssertionObject(RuntimeResources runtime, IToken linkedToken, IToken operation)
        {
            return new()
            {
                Token =
                    runtime.TokenAssertions
                    .TryGetValue(linkedToken, out var tokenAssertions)
                    .ToOption(tokenAssertions).Or([])
                    !.Map(assertion => EvaluateAssertion(assertion, operation))
                    .ToArray()
            };
        }
        private static OnResolveAssertionsImpl GenerateOnResolveAssertionObject(RuntimeResources runtime, IToken linkedToken, ResOpt resolution, IMemoryFZO nMemory)
        {
            return new()
            {
                Resolution =
                    runtime.ResolutionAssertions
                    .TryGetValue(linkedToken, out var resolutionAssertions)
                    .ToOption(resolutionAssertions).Or([])!
                    .Map(assertion => EvaluateAssertion(assertion, resolution))
                    .ToArray(),
                Memory =
                    runtime.MemoryAssertions
                    .TryGetValue(linkedToken, out var memoryAssertions)
                    .ToOption(memoryAssertions).Or([])!
                    .Map(assertion =>
                        EvaluateAssertion(assertion, nMemory))
                    .ToArray(),
            };
        }
        private static AssertionDataImpl<A> EvaluateAssertion<A>(IAssertionAccessor<A> assertion, A value)
        {
            IResult<bool, Exception>? result = null;
            try { result = result.Ok(assertion.Condition(value)); }
            catch (Exception e) { result = result.Err(e); }
            return new()
            {
                Condition = assertion.Condition,
                Description = assertion.Description,
                Result = result
            };
        }
        private static Err<IDeTesResult, EDeTesInvalidTest> Invalid(EDeTesInvalidTest val)
        {
            return new Err<IDeTesResult, EDeTesInvalidTest>(val);
        }
        private class RuntimeResources(IContextAccessor context)
        {
            public Queue<IDomainAccessor> DomainQueue = new();
            public Dictionary<IToken, IToken> PreprocessMap = new(new EqualityByReference());
            public Dictionary<IToken, IToken> MetaExecuteMap = new(new EqualityByReference());
            public Dictionary<IToken, List<IReferenceAccessor>> References = MakeTokenLinkDictionary(context.References);
            public Dictionary<IToken, List<IDomainAccessor>> Domains = MakeTokenLinkDictionary(context.Domains);
            public Dictionary<IToken, List<IAssertionAccessor<IToken>>> TokenAssertions = MakeTokenLinkDictionary(context.TokenAssertions);
            public Dictionary<IToken, List<IAssertionAccessor<ResOpt>>> ResolutionAssertions = MakeTokenLinkDictionary(context.ResolutionAssertions);
            public Dictionary<IToken, List<IAssertionAccessor<IMemoryFZO>>> MemoryAssertions = MakeTokenLinkDictionary(context.MemoryAssertions);
            private static Dictionary<IToken, List<A>> MakeTokenLinkDictionary<A>(IEnumerable<A> accessors) where A : ITokenLinked
            {
                var o = new Dictionary<IToken, List<A>>();
                foreach (var a in accessors)
                {
                    if (o.TryGetValue(a.LinkedToken, out var list)) list.Add(a);
                    else o[a.LinkedToken] = [a];
                }
                return o;
            }
            public IToken GetLinkedToken(IToken token)
            {
                return PreprocessMap[token].ExprAs(preV => MetaExecuteMap.TryGetValue(preV, out var metaV) ? metaV : preV);
            }
        }
        private class Input(Input.Data? data) : IInputFZO
        {

            private Data? _data = data;
            ITask<int[]> IInputFZO.GetSelection(IHasElements<ResObj> pool, int count)
            {
                if (_data is null) throw new RequiresDomainSplit();
                var data = _data;
                _data = null;

                if (data.Selection.Length != count || data.Selection.Any(i => i >= pool.Count)) throw new InvalidTestException
                {
                    Value = new EDeTesInvalidTest.InvalidDomainSelection
                    {
                        InvalidSelection = data.Selection,
                        ExpectedSelectionSize = count,
                        ExpectedMaxIndex = pool.Count - 1,
                        SelectionToken = data.SelectionToken,
                        NearToken = data.Domain.LinkedToken,
                        Description = data.Domain.Description,
                        Domain = data.Domain.Selections
                    }
                };
                return data.Selection.ToCompletedITask();
            }
            public class Data
            {
                public required int[] Selection { get; init; }
                public required IDomainAccessor Domain { get; init; }
                public required IToken SelectionToken { get; init; }
            }
        }
        private class RequiresDomainSplit : Exception { }
    }
    
    internal class EqualityByReference : EqualityComparer<object>
    {
        public override bool Equals(object? x, object? y) => ReferenceEquals(x, y);
        public override int GetHashCode([DisallowNull] object obj) => obj.GetHashCode();
    }
    internal class InvalidTestException : Exception
    {
        public required EDeTesInvalidTest Value { get; init; }
    }

}
