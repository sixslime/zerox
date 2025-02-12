
using System;
using System.Collections.Generic;
namespace PROTO_ZeroxFour_1;
using FourZeroOne.Core.Syntax;
using DeTes.Syntax;
using Minima.FZO;
using FourZeroOne.FZOSpec;
using Perfection;
using CatGlance;
using ro = FourZeroOne.Core.Resolutions.Objects;
public class Tester
{
    static readonly DeTesFZOSupplier RUN_IMPLEMENTATION = new()
    {
        Processor = new MinimaProcessorFZO(),
        UnitializedState = new MinimaStateFZO()
    };
    static readonly IMemoryFZO MEMORY_IMPLEMENTATION = new MinimaMemoryFZO();
    public async Task Run()
    {

        var shouldPass = new Glancer
        {
            Name = "Should Pass",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 5.tFixed().AssertResolution(C, x => x.Value == 5)
                },
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        4.tFixed().AssertResolution(C, x => x.Value == 4)
                        .tDuplicate(4.tFixed())
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
                        .AssertResolution(C, x => x.Count == 4)
                },
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        Core.tSubEnvironment(RHint<ro.Number>.HINT, new()
                        {
                            Environment = Core.t_Env(
                                10.tFixed().tAsVariable(out var ten),
                                4.tFixed().tAsVariable(out var _),
                                true.tFixed()
                                )
                                .AssertMemory(C, x => x.GetObject(ten).Check(out var v) && v.Value == 10)
                                .AssertResolution(C, x => x.Count == 3)
                                .AssertMemory(C, x => x.Objects.Count() == 2),
                            Value = ten.tRef().tAdd(1.tFixed())
                        })
                        .AssertResolution(C, x => x.Value == 11)
                        .AssertMemory(C, x => !x.Objects.Any())
                }
            }
        };
        await shouldPass.Glance();

        var shouldFail = new Glancer
        {
            Name = "Should Fail",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 5.tFixed().AssertResolution(C, x => x.Value == 0)
                },
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 
                        4.tFixed().AssertResolution(C, x => x.Value == 4)
                        .AssertResolution(C, _ => false)
                        .tDuplicate(4.tFixed())
                        .AssertResolution(C, _ => false)
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
                    //LEFTOFF: assertions arent recognized on macros/meta executes. something with token map.
                },
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        4.tFixed().AssertResolution(C, x => x.Value == 4).Yield(4).tToMulti()
                        .AssertResolution(C, _ => false)
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
                }
            }
        };
        await shouldFail.Glance();
    }

    /* CatGlance spec
     * 
     * var glance = new CatGlance();
     * var test = new DeTest
     * {
     *  ...
     * }.Glance("mytest")
     *
     *  var test = new GlancableTest
     *  {
     *      string Name = "mytest"
     *      ...
     *  }
     *
     * Resolution methods should automatically assume and unwrap Some(), failing if None().
     * these methods have 'Unstable' counterparts that do not do this, ex: 'AssertResolutionUnstable()'
     * 
     * selection domains apply to exactly 1 selection, the next IO selection.
     * additional selection domains are queued past the first
     * err on selection without a domain.
     */
}

/* NOTES
 *# Boxed Reference Issues (captures)
 * given 'let { a } in { () => &a }',
 * '&a' in the boxed function will be dangling when it's passed upwards.
 * me when variable captures exist for a reason!
 * I don't even know if capturing is feasable conceptually.
 * The "solution" is just to be careful with boxed functions :P
 */