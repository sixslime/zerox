
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
using t = FourZeroOne.Core.Tokens;
using Rule = FourZeroOne.Rule;
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

        
        var shouldInvalid = new Glancer
        {
            Name = "Should Invalid",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new("give 1D expect 2D domain")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        (1..10).tFixed()
                        .tIOSelectMany(2.tFixed())
                        .DefineSelectionDomain(C, (0..10).RangeIter(), out var domain, "bad domain")
                }
            }
        };

        var shouldFail = new Glancer
        {
            Name = "Should Fail",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new("trivial t")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 0.tFixed()
                    .AssertToken(C, u => u is not t.Fixed<ro.Number>)
                },
                new("trivial m")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 0.tFixed()
                    .AssertMemory(C, u => u.Objects.Any())
                },
                new("trivial r")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 0.tFixed()
                    .AssertResolution(C, u => u.Value != 0)
                },
                new("assertion within macro")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        5.tFixed()
                        .AssertResolution(C, _ => false)
                        .tDuplicate(2.tFixed())
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
                },
                new("3 duplicate resolution fails")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        3.tFixed().AssertResolution(C, _ => false).Yield(3).tToMulti()
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
                },
                new("fail on index 1 & 4")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        (1..5).tFixed()
                        .tIOSelectOne()
                        .DefineSelectionDomain(C, (0..5).RangeIter(), out var domain, "failhere")
                        .AssertResolution(C, _ => domain.SelectedIndex() is not 1 and not 4)
                },
            }
        };

        var shouldPass = new Glancer
        {
            Name = "Should Pass",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new("trivial t")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        400.tFixed().tAdd(1.tFixed())
                        .AssertToken(C, u => u is t.Number.Add add && add.Arg1 is t.Fixed<ro.Number> a && a.Resolution.Value == 400)
                },
                new("trivial r")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        400.tFixed().tAdd(1.tFixed())
                        .AssertResolution(C, u => u.Value == 401)
                },
                new("trivial m")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        400.tFixed().tAdd(1.tFixed())
                        .AssertMemory(C, u => !u.Objects.Any())
                },
                new("4 duplicate")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        4.tFixed().AssertResolution(C, u => u.Value == 4)
                        .tDuplicate(4.tFixed())
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
                        .AssertResolution(C, u => u.Count == 4)
                },
                new("subenv")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        Core.tSubEnvironment<ro.Number>(new()
                        {
                            Environment = Core.t_Env(
                                10.tFixed().tAsVariable(out var ten),
                                4.tFixed().tAsVariable(out var _),
                                true.tFixed()
                                )
                                .AssertMemory(C, u => u.GetObject(ten).Check(out var v) && v.Value == 10)
                                .AssertResolution(C, u => u.Count == 3)
                                .AssertMemory(C, u => u.Objects.Count() == 2),
                            Value = ten.tRef().tAdd(1.tFixed())
                        })
                        .AssertResolution(C, u => u.Value == 11)
                        .AssertMemory(C, u => !u.Objects.Any())
                },
                new("10x mult map")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        (1..10).tFixed()
                        .tMap(
                        x =>
                            x.tRef()
                            .AssertResolution(C, u => u.Value <= 10)
                            .tMultiply(10.tFixed()))
                        .AssertResolution(C, u => u.Elements.Enumerate().All(y => (y.index+1)*10 == y.value.Value))
                },
                new("1..8 domain")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        (1..10).tFixed()
                        .tIOSelectOne()
                        .DefineSelectionDomain(C, (0..8).RangeIter(), out var domain)
                        .AssertResolution(C, u => u.Value <= 8)
                        .AssertResolution(C, u => u.Value == domain.SelectedIndex() + 1)
                },
                new("4x4 selection domain")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        (1..10).tFixed()
                        .tIOSelectMany(4.tFixed())
                        .DefineSelectionDomain(C, Iter.Over(0, 2, 4, 6).Map(x => (x..(x+3)).RangeIter(true)), out var domain)
                        .AssertResolution(C, u => u.Count == 4)
                },
                new("trivial rule test")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        Core.tSubEnvironment<ro.Number>(new()
                        {
                            Environment = Core.t_Env(
                                Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                                {
                                    Matches = x => x.mIsType<t.Number.Add>(),
                                    Definition = (origin, a, b) =>
                                        origin.tRef().tRealize().tSubtract(1.tFixed())
                                })),
                            Value = 2.tFixed().tAdd(2.tFixed())
                            .AssertToken(C, u => u is t.Number.Subtract)
                            .AssertResolution(C, u => u.Value == 3)
                        })
                        .AssertResolution(C, u => u.Value == 3)
                },
                new("trivial meta excute test")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        Core.tMetaFunction<ro.Number, ro.Number>(
                            x =>
                            x.tRef().tAdd(10.tFixed()))
                        .tExecuteWith(new() { A = 5.tFixed() })
                        .AssertToken(C, u => u is t.MetaExecuted<ro.Number> exe && exe.Arg1 is t.Number.Add)
                        .AssertResolution(C, u => u.Value == 15)
                        .tAdd(1.tFixed())
                        .AssertResolution(C, u => u.Value == 16)
                }
            }
        };
        await shouldInvalid.Glance();
        await shouldFail.Glance();
        await shouldPass.Glance();

    }

    // WARNING:
    // reliance on metafunctions is bringing attention to the lack of variable capturing.

    // *ACTUALLY* INSANE IDEA
    // 'Type<R>' is a resolution type.
    // Generic macros are pointers to constant functions that take types, returning the actual function: (TypeA, TypeB) => (a, b)<TypeA, TypeB> => ...
    // The evil 'Cast<R>' token makes this possible.

    // labels are specifications that match tokens:
    interface ILabel { }

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
     * err on selection without u domain.
     */
}

/* NOTES
 *# Boxed Reference Issues (captures)
 * given 'let { u } in { () => &u }',
 * '&u' in the boxed function will be dangling when it'u passed upwards.
 * me when variable captures exist for u reason!
 * I don't even know if capturing is feasable conceptually.
 * The "solution" is just to be careful with boxed functions :P
 */