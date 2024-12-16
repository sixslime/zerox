
using System;
using System.Collections;
using System.Collections.Generic;
using FourZeroOne.Core.Syntax;
using t = FourZeroOne.Core.Tokens;
using p = FourZeroOne.Core.Proxies;
using FZ = FourZeroOne;
using ResObj = FourZeroOne.Resolution.IResolution;
using r = FourZeroOne.Core.Resolutions;
using a = FourZeroOne.Libraries.Axiom;
using ro = FourZeroOne.Core.Resolutions.Objects;
using System.Threading.Tasks;
using Perfection;
using ControlledFlows;
using FourZeroOne.Testing;
using FourZeroOne.Testing.Syntax;
namespace PROTO_ZeroxFour_1;


public class Tester
{
    public readonly static FZ.StateModels.Minimal BLANK_STARTING_STATE = new() { };
    public async Task Run()
    {
        Dictionary<string, List<ITest<FZ.Runtimes.FrameSaving.Gebug, ResObj>>> testGroups = new();

        /* Welcome.
         * I understand that reading lots of code is not fun, so this demo is quick and uncomprehensive (~5-10 min)
         * This demo is expressed as a series of tests.
         * 
         * Important notes:
         * - A Token evaluates it's arguements left-to-right to 'Resolutions', then evaluates itself to a Resolution.
         * - A Resolution can change the 'State' (memory) of the program when evaluated to.
         *  - A State change is only present to evaluations of depth >= it's own.
         * - methods that start with lowercase:
         *  - 't' return Tokens.
         *  - 'r' return Resolutions.
         *  - 'p' return Proxies (will be explained when encountered).
         */

        testGroups["Demo"] =
        [
            //                         ┌['ro.Number' is the Resolution-type that this test expects]
            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //            ┌['tFixed' creates a constant Token, containing a "fixed" Resolution]
                Evaluate = 10.tFixed().tAdd(5.tFixed()),
                Expect = new() {
                    Resolution = new ro.Number() { Value = 15 }.AsSome()
                    //                                          └[Resolutions can be 'None', so need to wrap "raw" Resolution in 'Some']
                }
            })
            .Named("10+5"),


            //                         ┌[an "array" of numbers]
            MkRuntime().MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //                       ┌['RHint' is explicitly required in some places because C# type inference isn't perfect]
                Evaluate = Core.tMultiOf(RHint<ro.Number>.Hint(), [
                    1.tFixed(),
                    2.tFixed(),
                    3.tFixed(),
                    2.tFixed().tAdd(2.tFixed())
                    ]),

                Expect = new() {
                    Resolution = new r.Multi<ro.Number>() {Values = [1, 2, 3, 4]}.AsSome()
                }
            })
            .Named("1..4 Array")
            .Use(out var array_test),
            // └[create a handle to this test for use in others]


            MkRuntime([0]).MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //                ┌[use the Resolution of the previous test]
                Evaluate = (await array_test.GetResolution()).Unwrap().tFixed().tIOSelectOne(),
                //                                                              └[prompt user to select one element]
                Assert = new() {
                    Resolution = x => x is Some<ro.Number> num && num.Unwrap().Value > 0,
                }
            })
            .Named("Selection"),


            MkRuntime([0]).MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //              ┌[simply evaluates 'Environment', then evaluates and returns 'Value']
                Evaluate = Core.tSubEnvironment(RHint<ro.Number>.Hint(), new() {
                //                                                        ┌[non-essential shorthand for creating an array of fixed Resolutions]
                    Environment = Iter.Over(2, 4, 6).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()
                        .tAsVariable(out var selection),
                //       └[modifies the State, storing the evaluated Resolution with 'selection' pointing to it]
                //                    ┌[refer to the stored Resolution]
                    Value = selection.tRef().tMultiply(selection.tRef())
                //                                               └[refer to it again]
                }),
                Assert = new() {
                    Resolution = x => x is Some<ro.Number> num && num.Unwrap().Value.ExprAs(n => n == 4 || n == 16 || n == 36),
                }
            })
            .Named("Selection Squared"),


            // ## does exactly what the previous test does, but with a MetaFunction ##
            MkRuntime([0]).MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //                                 ┌[same structure as Func<...> delegate]
                Evaluate = Core.tMetaFunction(RHint<ro.Number, ro.Number>.Hint(), x => x.tRef().tMultiply(x.tRef()))
                //              └[equiv to C# Func<...> object]                     └[MetaFunction definition]
                //   ┌[execute the MetaFunction with it's required arg]
                    .tExecuteWith(new() {
                        A = Iter.Over(2, 4, 6).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()
                    }),
                Assert = new() {
                    Resolution = x => x is Some<ro.Number> num && num.Unwrap().Value.ExprAs(n => n == 4 || n == 16 || n == 36),
                }
            })
            .Named("Selection Squared MetaFunction"),


            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = 4.tFixed().tIsGreaterThan(0.tFixed()).tIfTrue(RHint<ro.Number>.Hint(), new() {

                    //                   ┌["() => <previous token>" as a MetaFunction]
                    Then = 9999.tFixed().tMetaBoxed(),
                    Else = (await array_test.GetToken()).tIOSelectOne().tMetaBoxed()
                })
            //   ┌['Then' and 'Else' are always greedily evaluated, regardless of boolean outcome]
                .tExecute(),
            //   └["wrapping" them in MetaFunctions then only executing the result emulates lazy evaluation]
            //    (if 'Then' and 'Else' were not MetaFunctions, the user would be prompted for selection regardless of boolean outcome)
                Expect = new() {
                    Resolution = 9999.rAsRes()
                }
            })
            .Named("Basic 'if'"),


            // ## 'Rules' are evaluation-time Token replacements ##
            // ## they are written in proxies, which can reference the 'Original' information of the Token they are replacing. ##
            // ## 'Hooks' can be arbitrarily attached to any Tokens (see below) ##
            MkRuntime().MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tSubEnvironment(RHint<r.Multi<ro.Number>>.Hint(), new() {
                //                ┌[constructs Rule: replace '(A+B)' with '((A-B)+1)', given '(A+B)' has the Hook 'my_hook']
                    Environment = MakeProxy.AsRule<t.Number.Add, ro.Number>("my_hook", P =>
                        P.pOriginalA().pSubtract(P.pOriginalB()).pAdd(1.tFixed().pDirect(P)))
                    .tAddRule(),
                //   └[Resolution will add Rule to the State]

                    Value = Core.tMultiOf(RHint<ro.Number>.Hint(), [
                    //              ┌[affected by Rule, has "my_hook" Hook]
                        10.tFixed().tAdd(5.tFixed()).WithHookLabels(["my_hook"]),
                    //              ┌[unaffected by Rule]
                        10.tFixed().tAdd(5.tFixed())
                    ])
                }),
                Expect = new() {
                //               ┌[[6, 15]]
                    Resolution = Iter.Over(6.rAsRes(), 15.rAsRes()).rAsRes()
                }
            })
            .Named("Rule Introduction")
        ];

        // make better later
        foreach (var testGroup in testGroups)
        {
            var groupList = testGroup.Value;
            Console.WriteLine($"=== GROUP: \"{testGroup.Key}\" ===");
            for (int i = 0; i < groupList.Count; i++)
            {
                Console.WriteLine($"--[{i}] TEST: \"{groupList[i].Name}\" --");
                await groupList[i].EvaluateMustPass();
            }
            
        }
    }
    /// <summary>
    /// Creates a basic Runtime, with the option to specify auto-selections.
    /// </summary>
    /// <param name="selections"></param>
    /// <returns></returns>
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime(params int[]?[] selections)
    {
        return new FZ.Runtimes.FrameSaving.Gebug().Mut(x => x.SetAutoSelections(selections));
    }
    /// <summary>
    /// Creates a basic Runtime, with the option to specify auto-selections and auto-rewinds.
    /// </summary>
    /// <param name="selections"></param>
    /// <returns></returns>
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime(int[]?[] selections, int?[] rewinds)
    {
        return new FZ.Runtimes.FrameSaving.Gebug().Mut(x => { x.SetAutoSelections(selections); x.SetAutoRewinds(rewinds); });
    }
}

/* NOTES
 *# Boxed Reference Issues (captures)
 * given 'let { a = ... } in { boxed(() => a.tRef)) }',
 * the boxed function will have a nullptr when it's passed up.
 * me when variable captures exist for a reason!
 * I don't even know if capturing is feasable conceptually.
 * The "solution" is just to be careful with boxed functions :P
 * 
 *# There seems to be buggyness with rewinding and metafunctions/tests(?)
 * requires further investigation.
 */