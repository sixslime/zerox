
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


        /* Intro Demo (~5-10 min)
         * This demo covers everything important in a breif manner.
         * Demos are expressed through series' of tests.
         * 
         * Important notes:
         * - A Token evaluates it's arguements left-to-right to 'Resolutions', then evaluates itself to a Resolution.
         * - A Resolution can change the 'State' (memory) of the program when evaluated to.
         *  - A State change is only present to evaluations of depth >= it's own.
         * - methods that start with lowercase:
         *  - 't' return Tokens.
         *  - 'r' return Resolutions.
         *  - 'p' return Proxies (will be explained when encountered).
         * - when "prompted" to select, enter the index number of the desired element.
         */

        testGroups["Intro Demo"] =
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
                //                       ┌['RHint' is in general specifies Resolution-type where it can't be inferred]
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


            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //                ┌[use the Resolution of the previous test]
                Evaluate = (await array_test.GetResolution()).Unwrap().tFixed().tIOSelectOne(),
                //                                                              └[prompt user to select one element]
                Assert = new() {
                    Resolution = x => x is Some<ro.Number> num && num.Unwrap().Value > 0,
                }
            })
            .Named("Selection"),


            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
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


            // ## functionally equivalent to the previous test, but with a MetaFunction ##
            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
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
                        10.tFixed().tAdd(5.tFixed()).WithHooks(["my_hook"]),
                    //              ┌[unaffected by Rule]
                        10.tFixed().tAdd(5.tFixed())
                    ])
                }),
                Expect = new() {
                //               ┌[[6, 15]]
                    Resolution = Iter.Over(6.rAsRes(), 15.rAsRes()).rAsRes()
                }
            })
            .Named("Rule")
        ];

        /* Advanced Demo (~10 min)
         * This demo covers everything the Intro doesn't, and includes more advanced and real(ish) use cases.
         * It also makes full use of shorthands and testing features.
         *
         * 
         * - when prompted to select multiple elements, type index numbers with spaces. ex: "1 2 4"
         */

        testGroups["Advanced Demo"] =
        [
        //  ┌['MkRuntimeWithAuto' makes a Test with auto-selections]
            MkRuntimeWithAuto([[1], [4, 3, 2, 0]]).MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
            //                 └[replace element with 'null' to send prompt to user]
                State = BLANK_STARTING_STATE,
                Evaluate = Iter.Over(1, 2, 3, 4, 5).Map(x => x.tFixed()).t_ToConstMulti()
                //   ┌[first prompt to select 2 or 4, then prompt to select *that* number of elements from the list above]
                    .tIOSelectMany(Iter.Over(2, 4).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()),
                Assert = new() {
                    Resolution = x => x is Some<r.Multi<ro.Number>> selection && selection.Unwrap().Count is 2 or 4
                }
            })
            .Named("tIOSelectMany Token"),


            // ## Effectively equivalent to the previous test, but implemented using recursion and 'tIOSelectOne' ##
            //                                                        ┌[would be annoying to specify this hint-type repeatedly]
            MkRuntimeWithAuto([[1], [4], [3], [2], [0]]).MakeTest(RHint<r.Multi<ro.Number>>.Hint(), hint => async () => new() {
            //                                                                                          └['hint' is simply the hint-type of this test]
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tSubEnvironment(hint, new() {
                    Environment = Iter.Over(2, 4).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne().tAsVariable(out var firstSelection),
                    //           ┌[includes a pointer to itself as first arguement in it's definition]
                    Value = Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<ro.Number>, r.Multi<ro.Number>>.Hint(),
                //   ┌['thisFunc' points to this MetaFunction]
                    (thisFunc, argA, argB) =>
                        firstSelection.tRef().tIsGreaterThan(argA.tRef())
                        .tIfTrue(hint, new()
                        {
                            Then = Core.tSubEnvironment(hint, new()
                            {
                                Environment = argB.tRef().tIOSelectOne().tAsVariable(out var partialSelection).tYield(),
                                //                              ┌['tYield' converts a value to single-element list]
                                Value = partialSelection.tRef().tYield()
                                .tUnion(thisFunc.tRef().tExecuteWith(new()
                                {
                                //  ┌[count of selections/recursions that have occured]
                                    A = argA.tRef().tAdd(1.tFixed()),
                                //  ┌[the next pool of elements to select from]
                                    B = argB.tRef().tWithout(partialSelection.tRef().tYield())
                                //                  └['tWithout' is like LINQ 'Except']
                                }))
                            }).tMetaBoxed(),
                        //  ┌[evaluate to Nolla (None) if the recursion count has surpassed 'firstSelection' value]
                            Else = Core.tNolla(hint).tMetaBoxed()
                        //              └[the Union Token ('tUnion') treats Nolla like an empty list]
                        })
                        .tExecute())
                //   ┌[initial call of recursive function]
                    .tExecuteWith(new()
                    {
                        A = 0.tFixed(),
                        B = Iter.Over(1, 2, 3, 4, 5).Map(x => x.tFixed()).t_ToConstMulti()
                    })
                }),
                Assert = new() {
                    Resolution = x => x is Some<r.Multi<ro.Number>> selection && selection.Unwrap().Count is 2 or 4
                }
            })
            .Named("Select Many Through Recursion"),

        ];
        // make better later
        foreach (var testGroup in testGroups)
        {
            var groupList = testGroup.Value;
            Console.WriteLine($"=== GROUP: \"{testGroup.Key}\" ===");
            if (testGroup.Key != "Advanced Demo")
            {
                Console.WriteLine("(SKIPPED)");
                continue;
            }
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
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime()
    {
        return MkRuntimeWithAuto([]);
    }
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntimeWithAuto(int[]?[] selections)
    {
        return MkRuntimeWithAuto(selections, []);
    }
    /// <summary>
    /// Creates a basic Runtime, with the option to specify auto-selections and auto-rewinds.
    /// </summary>
    /// <param name="selections"></param>
    /// <returns></returns>
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntimeWithAuto(int[]?[] selections, int?[] rewinds)
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