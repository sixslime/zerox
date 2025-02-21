using FourZeroOne.FZOSpec;
using MorseCode.ITask;
using Res = FourZeroOne.Resolution.IResolution;
using ResOpt = SixShaded.NotRust.IOption<FourZeroOne.Resolution.IResolution>;
using FourZeroOne.Token;
using SixLib.GFunc;
using SixShaded.NotRust;
#nullable enable
namespace DeTes.Realization
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Analysis;
    using FourZeroOne.FZOSpec.Shorthands;
    using Declaration;
    using CriticalPointType = IResult<IResult<EProcessorHalt, Exception>, Analysis.IDeTesSelectionPath[]>;
    using IToken = IToken<Res>;
    using SixShaded.NotRust;

    internal class DeTesRealizerImpl
    {
        public async Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(IDeTesTest test, IDeTesFZOSupplier supplier)
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
            catch (DeTesInvalidTestException invalid) { return Invalid(invalid.Value); }
            catch (Exception e) { throw new DeTesInternalException(e); }

        }

        // WARNING: can be optimized, particularly with code elegance/repitition
        private static async Task<ResultImpl> Eval(IStateFZO state, IProcessorFZO processor, RuntimeResources runtime, Input input)
        {
            List<EDeTesFrame> frames = new();
            CriticalPointType? critPoint = null;
            Stopwatch timer = new();
            timer.Start();
            while (true)
            {
                IResult<EProcessorStep, EProcessorHalt>? processorStep = null;

                // get next step:
                try { processorStep = await processor.GetNextStep(state, input); }
                // create domain paths recursively if requested:
                catch (RequiresDomainSplit)
                {
                    if (!runtime.DomainQueue.TryDequeue(out var domain))
                        throw new DeTesInvalidTestException
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
                        domain.MetaIndex = i;
                        var selToken = state.OperationStack.First().Operation;
                        paths[i] = new SelectionPathImpl
                        {
                            RootSelectionToken = selToken,
                            ResultObject = await Eval(state, processor, runtime, new(new()
                            {
                                Domain = domain,
                                Selection = thisSelection,
                                SelectionToken = selToken
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
                catch (DeTesInvalidTestException) { throw; }
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
                        var linkedToken = runtime.GetLinkedToken(GetLastOperation(state));
                        var resolution = complete.Resolution;
                        frames.Add(new EDeTesFrame.Complete
                        {
                            Origin = linkedToken,
                            PreState = state,
                            CompletionHalt = complete,
                            Assertions = GenerateOnResolveAssertionObject(runtime, linkedToken, resolution, GetMemoryAfterResolution(state, resolution), GetLastOperation(state))
                        });
                    }
                    
                    critPoint = critPoint.Ok(halt.AsOk(Hint<Exception>.HINT));
                    break;
                }

                // main runtime/processor loop logic:
                switch (step)
                {
                    case EProcessorStep.TokenMutate v:
                        {
                            //DEBUG
                            //if (v.Mutation is not ETokenMutation.Identity)
                            //{
                            //    Console.ForegroundColor = ConsoleColor.Yellow;
                            //    Console.WriteLine(state.TokenMutationStack.Map(x => $"{x.GetType().Name} : {x.Result}").ToArray().LookNicePls());
                            //    Console.WriteLine(v.Mutation.GetType().Name);
                            //    Console.ForegroundColor = ConsoleColor.Gray;
                            //    Console.WriteLine(v.Mutation.Result);
                            //    Console.ResetColor();
                            //}
                            
                            //tokenmap can get pretty large because its just 1 mutable object.
                            switch (v.Mutation)
                            {
                                case ETokenMutation.Identity identity:
                                    runtime.PreprocessMap[identity.Result] = identity.Result;
                                    break;
                                default:
                                    // kinda inefficient but the alternative is using potentially unproven cache assumptions.
                                    runtime.PreprocessMap[v.Mutation.Result] =
                                        runtime.GetLinkedToken(state.TokenMutationStack.Last().IsA<ETokenMutation.Identity>().Result);
                                    break;
                            }
                            frames.Add(new EDeTesFrame.TokenPrep
                            {
                                Origin = runtime.GetLinkedToken(v.Mutation.Result),
                                PreState = state,
                                NextStep = v
                            });
                        }
                        break;
                    case EProcessorStep.PushOperation v:
                        {
                            var linkedToken = runtime.GetLinkedToken(v.OperationToken);
                            frames.Add(new EDeTesFrame.PushOperation
                            {
                                Origin = linkedToken,
                                PreState = state,
                                NextStep = v,
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
                                var nToken = GetLastOperation(state);
                                frames.Add(new EDeTesFrame.Resolve
                                {
                                    Origin = linkedToken,
                                    PreState = state,
                                    NextStep = v,
                                    Assertions = GenerateOnResolveAssertionObject(runtime, linkedToken, resolution, nMemory, nToken)
                                });
                                // DEBUG
                                //Console.WriteLine($"{linkedToken}");
                                //Console.WriteLine($"{runtime.References.ToArray().LookNicePls()}");
                                if (runtime.References.TryGetValue(linkedToken, out var references))
                                    foreach (var reference in references)
                                    {
                                        // DEBUG
                                        //Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        //Console.WriteLine($"set {reference.Description} to {resolution}");
                                        //Console.ResetColor();
                                        reference.SetResolution(resolution);
                                        reference.SetMemory(nMemory);
                                        reference.SetToken(nToken);
                                    }
                            }
                            else
                            {
                                switch (stateImplemented)
                                {
                                    case EStateImplemented.MetaExecute metaExecute:
                                        //runtime.MetaExecuteMap[metaExecute.FunctionToken] = linkedToken;
                                        runtime.PreprocessMap[metaExecute.Token] = linkedToken;
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
            timer.Stop();
            if (critPoint is null) throw new UnreachableException();
            return new()
            {
                TimeTaken = timer.Elapsed,
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
        private static OnResolveAssertionsImpl GenerateOnResolveAssertionObject(RuntimeResources runtime, IToken linkedToken, ResOpt resolution, IMemoryFZO nMemory, IToken token)
        {
            // DEBUG
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Console.WriteLine(linkedToken);
            //foreach (var (key, value) in runtime.ResolutionAssertions)
            //{
            //    Console.ForegroundColor = ConsoleColor.DarkGray;
            //    if (key.GetHashCode().Equals(linkedToken.GetHashCode())) Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine($"{key} : {linkedToken} - {key.Equals(linkedToken)}");
            //}
            //Console.ResetColor();
            return new()
            {
                Resolution =
                    runtime.ResolutionAssertions
                    .TryGetValue(linkedToken, out var resolutionAssertions)
                    .ToOption(resolutionAssertions).Or([])!
                    .Map(assertion => EvaluateAssertion(assertion, linkedToken, resolution))
                    .ToArray(),
                Memory =
                    runtime.MemoryAssertions
                    .TryGetValue(linkedToken, out var memoryAssertions)
                    .ToOption(memoryAssertions).Or([])!
                    .Map(assertion =>
                        EvaluateAssertion(assertion, linkedToken, nMemory))
                    .ToArray(),
                Token =
                    runtime.TokenAssertions
                    .TryGetValue(linkedToken, out var tokenAssertions)
                    .ToOption(tokenAssertions).Or([])!
                    .Map(assertion =>
                        EvaluateAssertion(assertion, linkedToken, token))
                    .ToArray(),
            };
        }
        private static AssertionDataImpl<A> EvaluateAssertion<A>(IAssertionAccessor<A> assertion, IToken linkedToken, A value)
        {
            IResult<bool, Exception>? result = null;
            try { result = result.Ok(assertion.Condition(value)); }
            catch (DeTesInvalidTestException) { throw; }
            catch (Exception e) { result = result.Err(e); }
            return new()
            {
                OnToken = linkedToken,
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
                var o = new Dictionary<IToken, List<A>>(new EqualityByReference());
                foreach (var a in accessors)
                {
                    if (o.TryGetValue(a.LinkedToken, out var list)) list.Add(a);
                    else o[a.LinkedToken] = [a];
                }
                return o;
            }
            public IToken GetLinkedTokenOld(IToken token)
            {
                return PreprocessMap[token].ExprAs(preV => MetaExecuteMap.TryGetValue(preV, out var metaV) ? metaV : preV);
            }
            public IToken GetLinkedToken(IToken token)
            {
                return PreprocessMap[token];
            }
        }
        private class Input(Input.Data? data) : IInputFZO
        {

            private Data? _data = data;
            ITask<int[]> IInputFZO.GetSelection(IHasElements<Res> pool, int count)
            {
                if (_data is null) throw new RequiresDomainSplit();
                var data = _data;
                _data = null;

                if (data.Selection.Length != count || data.Selection.Any(i => i >= pool.Count)) throw new DeTesInvalidTestException
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
    

}
