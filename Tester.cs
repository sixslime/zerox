
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

/* NOTES
 *# Boxed Reference Issues (captures)
 * given 'let { a = ... } in { boxed(() => a.tRef)) }',
 * the boxed function will have a nullptr when it's passed up.
 * me when variable captures exist for a reason!
 * I don't even know if capturing is feasable conceptually.
 * The "solution" is just to be careful with boxed functions :P
 */
public class Tester
{
    public readonly static RHint<ro.Number> NUMBER = new RHint<ro.Number>();
    public readonly static FZ.StateModels.Minimal BLANKSTATE = new() { };
    public async Task Run()
    {
        List<ITest<FZ.Runtimes.FrameSaving.Gebug, ResObj>> tests =
        [
            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANKSTATE,
                Evaluate = 2.tFixed().tAdd(3.tFixed()),
                Expect = new()
                {
                    Resolution = 5.Res(),
                    State = x => x
                },
                Assert = new()
                {
                    Resolution = x => true
                }
            })
            .Use(out var test_0)
            .Named("2 + 3"),

            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANKSTATE,
                Evaluate = (await test_0.GetToken()).tMultiply(2.tFixed()),
                Expect = new() {
                    Resolution = ((await test_0.GetResolution()).Unwrap() with { dValue = Q => Q * 2}).AsSome()
                }
            })
            .Named("(Test 0) * 2"),

            MkRuntime().MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
                State = BLANKSTATE,
                Evaluate = 1.Sequence(x => x + 1).Take(10).Map(x => x.tFixed()).t_ToConstMulti(),
                Expect = new() {
                    Resolution = 1.Sequence(x => x + 1).Take(10).Map(x => x.Res()).Res()
                }
            })
            .Use(out var ten_arr)
            .Named("[1..10]"),

            MkRuntime().MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
                State = BLANKSTATE,
                Evaluate = 1.Sequence(x => x + 1).Take(4).Map(x => x.tFixed()).tToMulti(),
                Expect = new() {
                    Resolution = 1.Sequence(x => x + 1).Take(4).Map(x => x.Res()).Res()
                }
            })
            .Named("[1..4] with Yield"),

            MkRuntime([8]).MakeTest(RHint<ro.Number>.Hint(), async () => new() {
                State = BLANKSTATE,
                Evaluate = (await ten_arr.GetToken()).tIOSelectOne(),
                Expect = new() {
                    Resolution = (await ten_arr.GetResolution()).Unwrap().Values.ElementAt(8).AsSome()
                }
            })
            .Use(out var select_one)
            .Named("SelectOne"),

            MkRuntime([8, 6, 1]).MakeTest(RHint<r.Multi<ro.Number>>.Hint(), async () => new() {
                State = BLANKSTATE,
                Evaluate = (await ten_arr.GetToken()).tIOSelectMany(3.tFixed()),
                Expect = new() {
                    Resolution = (await ten_arr.GetResolution()).Unwrap().Values.ExprAs(arr => Iter.Over(8, 6, 1).Map(x => arr.ElementAt(x).AsSome())).Res()
                }
            })
            .Named("SelectMany"),

            MkRuntime().MakeTest(RHint<ro.Number>.Hint(), hint => async () => new() {
                State = BLANKSTATE,
                Evaluate = Core.tSubEnvironment(hint, new() {
                    Environment = 
                })
                Expect = new() {
                    Resolution = ((await test_0.GetResolution()).Unwrap() with { dValue = Q => Q * 2}).AsSome()
                }
            })
        ];
        
        // make better later
        for (int i = 0;  i < tests.Count; i++)
        {
            Console.WriteLine($"--[{i}] TEST: \"{tests[i].Name}\" --");
            await tests[i].EvaluateMustPass();
        }
    }

    private List<ITest<FZ.Runtimes.FrameSaving.Gebug, ResObj>> tests = new();
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime(params int[]?[] selections)
    {
        return new FZ.Runtimes.FrameSaving.Gebug().Mut(x => x.SetAutoSelections(selections));
    }
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime(int[]?[] selections, int?[] rewinds)
    {
        return new FZ.Runtimes.FrameSaving.Gebug().Mut(x => { x.SetAutoSelections(selections); x.SetAutoRewinds(rewinds); });
    }
}
public static class TesterExtensions
{

}