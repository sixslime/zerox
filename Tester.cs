
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
                new("5")
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        5.tFixed().AssertResolution(C, u => u.Value == 5)
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
                        Core.tSubEnvironment(RHint<ro.Number>.HINT, new()
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
                }
            }
        };
        var shouldFail = new Glancer
        {
            Name = "Should Fail",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 0.tFixed().AssertMemory(C, x => false)
                },
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C => 0.tFixed().AssertResolution(C, x => false)
                },
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        5.tFixed()
                        .AssertResolution(C, _ => false)
                        .tDuplicate(2.tFixed())
                        .AssertResolution(C, _ => true)
                        .AssertMemory(C, _ => true)
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

        var shouldInvalid = new Glancer
        {
            Name = "Should Invalid",
            Supplier = RUN_IMPLEMENTATION,
            Tests = new GlancableTest[]
            {
                new()
                {
                    InitialMemory = MEMORY_IMPLEMENTATION,
                    Token = C =>
                        (1..10).tFixed()
                        .tIOSelectMany(2.tFixed())
                        .DefineSelectionDomain(C, (0..10).RangeIter(), out var domain)
                }
            }
        };
        await shouldInvalid.Glance();
        await shouldFail.Glance();
        await shouldPass.Glance();

    }

    // WARNING:
    // reliance on metafunctions is bringing attention to the lack of variable capturing.

    // ACTUALLY
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