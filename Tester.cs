
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
            .Named("1..4 Multi")
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
            .Named("selection"),


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
            .Named("selection squared"),


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
            .Named("MetaFunction introduction"),


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
            .Named("basic \"If\""),


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
                        10.tFixed().tAdd(5.tFixed()).WithHooks("my_hook"),
                    //              ┌[unaffected by Rule]
                        10.tFixed().tAdd(5.tFixed())
                    ])
                }),
                Expect = new() {
                //               ┌[[6, 15]]
                    Resolution = Iter.Over(6.rAsRes(), 15.rAsRes()).rAsRes()
                }
            })
            .Named("Rule introduction")
        ];

        /* Advanced Examples
         * This set of tests covers everything the Intro Demo doesn't and includes tokens with realistic complexity (in no particular order).
         * It also makes full use of shorthands and testing features.
         *
         * Important Notes:
         * - These tests make use of auto-selections for convenience.
         * - When prompted to make a manual selection, you may enter '<' followed by number to "rewind" that many 'Frames'. ex: "<7"
         *  - A Frame is saved whenever a Token evaluates to a Resolution (green text).
         */

        testGroups["Advanced Examples"] =
        [
        //  ┌['MkRuntimeWithAuto' makes a Test with auto-selections]
            MkRuntimeWithAuto([[1], [4, 3, 2, 0]]).MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
            //                 └[replace an element with 'null' to send that prompt to user. ex:'[null, [4, 3, 2, 0]]']
                State = BLANK_STARTING_STATE,
                Evaluate = Iter.Over(1, 2, 3, 4, 5).Map(x => x.tFixed()).t_ToConstMulti()
                //   ┌[first prompt to select 2 or 4, then prompt to select *that* number of elements from the list above]
                    .tIOSelectMany(Iter.Over(2, 4).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()),
                Assert = new() {
                    Resolution = x => x is Some<r.Multi<ro.Number>> final && final.Unwrap().Count is 2 or 4
                }
            })
            .Named("select many"),


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
                    Resolution = x => x is Some<r.Multi<ro.Number>> final && final.Unwrap().Count is 2 or 4
                }
            })
            .Named("recursion introduction")
            .Use(out var recursion_test),

            //                                                                ┌[rewind 63 Frames (back to 2nd selection) on 5th selection prompt]
            MkRuntimeWithAuto([[1], [4], [3], [2], null, [0], [0], [0], [1]], [null, null, null, null, 63])
            //                                     └[rewind happens when this selection would be made; this auto-selection is skipped]
            .MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = (await recursion_test.GetToken()),
                Assert = new() {
                    Resolution = x => x is Some<r.Multi<ro.Number>> final && final.Unwrap().Count is 2 or 4
                }
            })
            .Named("rewind introduction"),


            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), hint => async () => new() {
                State = BLANK_STARTING_STATE,
            //                                    ┌[with two exceptions (shown in next example), a Token evaluates to Nolla if *any* of it's arguements do]
                Evaluate = 100.tFixed().tAdd(Core.tNolla(hint)).tIsGreaterThan(200.tFixed()).tIfTrue(hint, new() {
            //                                    └['tNolla'=Nolla -> 'tAdd'=Nolla -> 'tIsGreaterThan'=Nolla -> 'tIfTrue'=Nolla -> 'tExecute'=Nolla]
                    Then = 1.tFixed().tMetaBoxed(),
                    Else = 0.tFixed().tMetaBoxed(),
                }).tExecute(),
                Expect = new() {
            //                       ┌['None' represents Nolla; Nolla is typed]
                    Resolution = new None<ro.Number>()
                }
            })
            .Named("Nolla propagation"),


            MkRuntime().MakeTest(RHint<r.Multi<ro.Bool>>.Hint(), async () => new() {
                State = BLANK_STARTING_STATE,
                //              ┌[Union Tokens treat Nolla as empty sets]
                Evaluate = Core.tMultiOf(RHint<ro.Bool>.Hint(),
            //                  └['tMultiOf', 'tToMulti', 'tUnion', and 'tFlatten' all construct Union Tokens]
                [
                //             ┌['tExists' evaluates False iff it's arguement evaluates to Nolla]
                    1.tFixed().tExists(),
                //              ┌[the arguement for 'tExists' can be of any ResolutionType]
                    Core.tNolla(RHint<ResObj>.Hint()).tExists(),
                //                    └['ResObj' is equiv. to C# 'object']
                    Core.tNolla(RHint<ro.Bool>.Hint())
                ]),
                Expect = new() {
                    Resolution = Iter.Over(true, false).Map(x => x.rAsRes()).rAsRes()
                }
            })
            .Named("Nolla catch methods"),


            //                         ┌[when a 'r.Multi' Resolution evaluates, all Resolutions within are applied in-order]
            MkRuntime().MakeTest(RHint<r.Multi<r.Instructions.Assign<ro.Number>>>.Hint(), hint => async () => new() {
            //                         └[in this case, the r.Multi of variable assignments stores multiple variables]
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tMultiOf(RHint<r.Instructions.Assign<ro.Number>>.Hint(),
                [
                //              ┌[stores the variable in the State, output pointer is just unused]
                    11.tFixed().tAsVariable(out var _),
                    22.tFixed().tAsVariable(out var _),
                    33.tFixed().tAsVariable(out var _),
                ]),
                Assert = new() {
                    State = startState => endState => endState.Objects.Count() - startState.Objects.Count() == 3
                }
            })
            .Named("Multi resolution"),

            
            MkRuntimeWithAuto([[2]]).MakeTest(RHint<r.Multi<ro.Number>>.Hint(), hint => async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tSubEnvironment(hint, new() {
                    Environment = Core.tMultiOf(RHint<ResObj>.Hint(), 
                    [
                    //  ## this Multi will be multiplied by the selected number ##
                        Iter.Over(1, 2, 3, 4, 5).Map(x => x.tFixed()).t_ToConstMulti()
                            .tAsVariable(out var baseArray),
                        Iter.Over(0, 1, 2, 4).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()
                            .tAsVariable(out var selectedNum),
                    ]),
            //                   ┌[iteration is only possible through recursion]
                    Value = Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<ro.Number>>.Hint(),
                    (thisFunc, i) =>
                    i.tRef().tIsGreaterThan(baseArray.tRef().tCount()).tIfTrue(hint, new() {
                        Then = Core.tMultiOf(RHint<ro.Number>.Hint(), []).tMetaBoxed(),
            //                                  ┌[indexing starts at 1; cry about it]
                        Else = baseArray.tRef().tGetIndex(i.tRef()).tMultiply(selectedNum.tRef())
                        .tYield().tUnion(thisFunc.tRef().tExecuteWith(new() {
                            A = i.tRef().tAdd(1.tFixed())
                        })).tMetaBoxed()
                    }).tExecute())
                    .tExecuteWith(new() {
                        A = 1.tFixed()
                    })
                }),
                Assert = new() {
                    Resolution = res => res is Some<r.Multi<ro.Number>> value &&
                    Iter.Over(
                        Iter.Over(0, 0, 0, 0, 0),
                        Iter.Over(1, 2, 3, 4, 5),
                        Iter.Over(2, 4, 6, 8, 10),
                        Iter.Over(4, 8, 12, 16, 20))
                    .Map(x => new r.Multi<ro.Number>() {Values = x.Map(x => (ro.Number)x)})
                    .Contains(value.Unwrap())
                }
            })
            .Named("iteration/mapping"),


            // ## functionally equiv. to previous test ##
            MkRuntimeWithAuto([[2]]).MakeTest(RHint<r.Multi<ro.Number>>.Hint(), hint => async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tSubEnvironment(hint, new() {
                    Environment = Core.tMultiOf(RHint<ResObj>.Hint(),
                    [
                        Iter.Over(1, 2, 3, 4, 5).Map(x => x.tFixed()).t_ToConstMulti()
                            .tAsVariable(out var baseArray),
                        Iter.Over(0, 1, 2, 4).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()
                            .tAsVariable(out var selectedNum),
                    ]),
            //                               ┌[some Tokens such as 'tMap' are 'Macros', which expand into other Tokens on evaluation]
                    Value = baseArray.tRef().tMap(x => x.tRef().tMultiply(selectedNum.tRef()))
            //                               └[the previous test reflects how 'tMap' is implemented]
                }),
                Assert = new() {
                    Resolution = res => res is Some<r.Multi<ro.Number>> result &&
                    Iter.Over(
                        Iter.Over(0, 0, 0, 0, 0),
                        Iter.Over(1, 2, 3, 4, 5),
                        Iter.Over(2, 4, 6, 8, 10),
                        Iter.Over(4, 8, 12, 16, 20))
                    .Map(x => new r.Multi<ro.Number>() {Values = x.Map(x => (ro.Number)x)})
                    .Contains(result.Unwrap())
                }
            })
            .Named("tMap Macro"),


            //                ┌[2 selections are required]
            MkRuntimeWithAuto([[0], [1]]).MakeTest(RHint<ro.Number>.Hint(), hint => async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tSubEnvironment(hint, new() {
                    Environment = MakeProxy.AsRule<t.Number.Multiply, ro.Number>("hook", P =>
            //        ┌[will fully evaluate argA Token]
                    P.pOriginalA().pMultiply(P.pOriginalA())).tAddRule(),
            //                                 └[will fully evaluate argA Token (again)]
            //                                                                    ┌[argA of 'tMultiply']
                    Value = Iter.Over(4, 8).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne()
                    .tMultiply(
            //             ┌[argB of 'tMultiply']
                        10.tFixed()).WithHooks("hook")
                }),
                Assert = new() {
                    Resolution = res => res is Some<ro.Number> result &&
                        (result.Unwrap().Value is 4*4 or 4*8 or 8*8)
                }
            })
            .Named("Token duplication through proxies"),


            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), hint => async () => new() {
                State = BLANK_STARTING_STATE,
                Evaluate = Core.tSubEnvironment(hint, new() {
                    Environment = MakeProxy.AsRule<t.Fixed<ro.Number>, ro.Number>("duplicate_me", P =>
            //        ┌['pThis' refers to the original Token itself, preserving Hooks]
                    P.pThis().pAdd(P.pThis())).tAddRule()
            //       ┌[create 4 duplicates of this rule]
                    .Yield(4).t_ToConstMulti(),
                    Value = 1.tFixed().WithHooks("duplicate_me")
                }),
                Expect = new() {
                    Resolution = 8.rAsRes()
                }
            })
        ];
        // make better later
        foreach (var testGroup in testGroups)
        {
            var groupList = testGroup.Value;
            Console.WriteLine($"=== GROUP: \"{testGroup.Key}\" ===");
            if (testGroup.Key != "Advanced Examples")
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