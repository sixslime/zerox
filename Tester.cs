
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
    public readonly static RHint<ro.Number> NUMBER = new RHint<ro.Number>();
    public readonly static FZ.StateModels.Minimal BLANKSTATE = new() { };
    public async Task Run()
    {
        var t1 = MkRuntime().MakeTest(NUMBER, () => new()
        {
            State = BLANKSTATE,
            Evaluate = 2.tFixed().tAdd(3.tFixed()),
            Expect = new()
            {
                Resolution = 5.Res().AsSome(),
                State = x => BLANKSTATE
            }
        });
        await t1.EvaluateMustPass();
    }

    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime(params int[]?[] selections)
    {
        return new FZ.Runtimes.FrameSaving.Gebug().Mut(x => x.SetAutoSelections(selections));
    }
    private static FZ.Runtimes.FrameSaving.Gebug MkRuntime(int[]?[] selections, int?[] rewinds)
    {
        return new FZ.Runtimes.FrameSaving.Gebug().Mut(x => { x.SetAutoSelections(selections); x.SetAutoRewinds(rewinds); });
    }
}