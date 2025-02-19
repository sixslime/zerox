
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
using r = FourZeroOne.Core.Resolutions;
using t = FourZeroOne.Core.Tokens;
using Rule = FourZeroOne.Rule;
using LookNicePls;
using GlanceResult = Perfection.IResult<Perfection.RecursiveEvalTree<DeTes.Analysis.IDeTesResult, bool>, DeTes.Analysis.EDeTesInvalidTest>;
public class Tester
{
    static readonly DeTesFZOSupplier RUN_IMPLEMENTATION = new()
    {
        Processor = new MinimaProcessorFZO(),
        UnitializedState = new MinimaStateFZO()
    };
    static readonly IMemoryFZO MEMORY_IMPLEMENTATION = new MinimaMemoryFZO();
    static readonly GlancableTest[] SANITY_CHECKS_PASS = new GlancableTest[]
    {
        new("t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertToken(C, u => u is t.Number.Add add && add.Arg1 is t.Fixed<ro.Number> a && a.Resolution.Value == 400)
        },
        new("r")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertResolution(C, u => u.Value == 401)
        },
        new("m")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertMemory(C, u => !u.Objects.Any())
        },
        new("meta execute internal assert")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertResolution(C, u => u.Value == 401)
                .tMetaBoxed()
                .tExecute()
        },
        new("rule")
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
                })
                .AssertResolution(C, u => u.Value == 3)
        },
        new("rule macro")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<r.Multi<ro.Number>>(new()
                {
                    Environment = Core.t_Env(
                        Core.tAddRule<ro.Number, ro.Number, r.Multi<ro.Number>>(new()
                        {
                            Matches = x => x.mIsMacro("core", "duplicate"),
                            Definition = (_, a, b) =>
                                a.tRef().tRealize().tDuplicate(b.tRef().tRealize().tAdd(1.tFixed()))
                        })),
                    Value = 401.tFixed().tDuplicate(3.tFixed())
                })
                .AssertResolution(C, u => u.Count == 4)
        },
        new("rule stacking")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<ro.Number>(new()
                {
                    Environment = Core.t_Env(
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Add>(),
                            Definition = (_, a, b) =>
                                a.tRef().tRealize().tSubtract(b.tRef().tRealize())
                        }),
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Subtract>(),
                            Definition = (_, _, _) =>
                                999.tFixed()
                        })),
                    Value = 400.tFixed().tAdd(1.tFixed())
                        .AssertToken(C, u => u is t.Fixed<ro.Number> num && num.Resolution.Value == 999)
                })
                .AssertResolution(C, u => u.Value == 999)
        },
        new("env var")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<ro.Number>(new()
                {
                    Environment = 10.tFixed().tAsVariable(out var xVar),
                    Value =
                        xVar.tRef().tAdd(1.tFixed())
                        .AssertMemory(C, u => u.Objects.Count() == 1)
                        .AssertMemory(C, u => u.GetObject(xVar).Unwrap().Value is 10)
                })
                .AssertResolution(C, u => u.Value is 11)
        },
        new("meta execute")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tMetaFunction<ro.Number, ro.Number>(
                    x =>
                    x.tRef().tAdd(10.tFixed()))
                .tExecuteWith(new() { A = 5.tFixed() })
                .AssertToken(C, u => u is t.MetaExecuted<ro.Number> exe && exe.Arg1 is t.Number.Add)
                .AssertResolution(C, u => u.Value == 15)
        },
        new("size 1 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..5).RangeIter(), out var domain)
                .tMultiply(2.tFixed())
                .AssertResolution(C, u => u.Value == (domain.SelectedIndex()+1) * 2)
        },
        new("size 4 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectMany(4.tFixed())
                .DefineSelectionDomain(C, (0..5).RangeIter().Map(x => (x..(x+4)).RangeIter()), out var domain)
                .tAtIndex(2.tFixed())
                .AssertResolution(C, u => u.Value == domain.SelectedIndicies()[1] + 1)
        },
        new("2D domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..5).RangeIter(), out var d1)
                .tMultiply(
                    (1..10).tFixed()
                    .tIOSelectOne()
                    .DefineSelectionDomain(C, (0..5).RangeIter(), out var d2))
                .AssertResolution(C, u => u.Value == (d1.SelectedIndex()+1) * (d2.SelectedIndex()+1))
        },
        new("reference resolution")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed()
                .ReferenceAs(C, out var reference)
                .tAdd(1.tFixed())
                .AssertResolution(C, u => u.Value == reference.Resolution.Value + 1)
        },
        new("reference token")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed()
                .ReferenceAs(C, out var reference)
                .tAdd(1.tFixed().AssertToken(C, u => u.GetType() == reference.Token.GetType()))
                .AssertToken(C, u => u is t.Number.Add add && add.Arg1 == reference.Token)
        },
        new("reference in selection")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (0..9).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..3).RangeIter(), out var d1)
                .tMultiply(10.tFixed())
                .ReferenceAs(C, out var reference)
                .tSubtract(4.tFixed())
                .AssertResolution(C, u => u.Value == reference.Resolution.Value - 4)
        },
        new("2D domain reference")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..5).RangeIter(), out var d1)
                .ReferenceAs(C, out var r1, "a")
                .tMultiply(
                    (1..8).tFixed()
                    .tIOSelectOne()
                    .DefineSelectionDomain(C, (0..5).RangeIter(), out var d2)
                    .ReferenceAs(C, out var r2, "b"))
                .AssertResolution(C, u => u.Value == r1.Resolution.Value * r2.Resolution.Value)
        },
    };
    static readonly GlancableTest[] SANITY_CHECKS_FAIL = new GlancableTest[]
    {
        new("t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C => 0.tFixed()
            .AssertToken(C, u => u is not t.Fixed<ro.Number>)
        },
        new("m")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C => 0.tFixed()
            .AssertMemory(C, u => u.Objects.Any())
        },
        new("r")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C => 0.tFixed()
            .AssertResolution(C, u => u.Value != 0)
        },
        new("size 1 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..5).RangeIter(), out var domain)
                .tMultiply(2.tFixed())
                .AssertResolution(C, u => u.Value is not 2)
        },
        new("size 4 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectMany(4.tFixed())
                .DefineSelectionDomain(C, (0..5).RangeIter().Map(x => (x..(x+4)).RangeIter()), out var domain)
                .tAtIndex(2.tFixed())
                .AssertResolution(C, u => u.Value is not 2)
        },
        new("2D domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..5).RangeIter(), out var d1, "left arg")
                .tMultiply(
                    (1..10).tFixed()
                    .tIOSelectOne()
                    .DefineSelectionDomain(C, (0..5).RangeIter(), out var d2, "right arg"))
                .AssertResolution(C, u => u.Value is not 4)
        },
        new("reference in selection")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (0..9).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..3).RangeIter(), out var d1)
                .tMultiply(10.tFixed())
                .ReferenceAs(C, out var reference)
                .tSubtract(4.tFixed())
                .AssertResolution(C, u => u.Value == reference.Resolution.Value)
        },
        new("2D domain reference")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (7..10).RangeIter(), out var d1)
                .ReferenceAs(C, out var r1, "a")
                .tMultiply(
                    (1..8).tFixed()
                    .tIOSelectOne()
                    .DefineSelectionDomain(C, (0..3).RangeIter(), out var d2)
                    .ReferenceAs(C, out var r2, "b"))
                .AssertResolution(C, u => u.Value != r1.Resolution.Value * r2.Resolution.Value)
        },
        new("meta execute r")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertResolution(C, _ => false)
                .tMetaBoxed()
                .tExecute()
                .AssertResolution(C, _ => true)
        },
        new("meta execute t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertToken(C, _ => false)
                .tMetaBoxed()
                .tExecute()
                .AssertToken(C, _ => true)
        },
        new("meta execute m")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                .AssertMemory(C, _ => false)
                .tMetaBoxed()
                .tExecute()
                .AssertMemory(C, _ => true)
        },
        new("rule t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<ro.Number>(new()
                {
                    Environment = Core.t_Env(
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Add>(),
                            Definition = (_, a, b) =>
                                a.tRef().tRealize().tSubtract(b.tRef().tRealize())
                        })),
                    Value = 400.tFixed().tAdd(1.tFixed())
                        .AssertToken(C, u => u is not t.Number.Subtract)
                })
        },
        new("rule token tracking t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<ro.Number>(new()
                {
                    Environment = Core.t_Env(
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Add>(),
                            Definition = (_, a, b) =>
                                a.tRef().tRealize().tSubtract(b.tRef().tRealize())
                        })),
                    Value = 400.tFixed().tAdd(1.tFixed())
                        .AssertToken(C, _ => false)
                })
        },
        new("stacked rule token tracking t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<ro.Number>(new()
                {
                    Environment = Core.t_Env(
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Add>(),
                            Definition = (_, a, b) =>
                                a.tRef().tRealize().tSubtract(b.tRef().tRealize())
                        }),
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Subtract>(),
                            Definition = (_, _, _) =>
                                999.tFixed()
                        })),
                    Value = 
                        400.tFixed().tAdd(1.tFixed())
                        .AssertToken(C, _ => false)
                })
        },
        new("stacked rule token tracking r")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<ro.Number>(new()
                {
                    Environment = Core.t_Env(
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Add>(),
                            Definition = (_, a, b) =>
                                a.tRef().tRealize().tSubtract(b.tRef().tRealize())
                        }),
                        Core.tAddRule<ro.Number, ro.Number, ro.Number>(new()
                        {
                            Matches = x => x.mIsType<t.Number.Subtract>(),
                            Definition = (_, _, _) =>
                                999.tFixed()
                        })),
                    Value =
                        400.tFixed().tAdd(1.tFixed())
                        .AssertResolution(C, _ => false)
                })
        },
    };
    static readonly GlancableTest[] SANITY_CHECKS_INVALID = new GlancableTest[]
    {
        new("give 1D expect 2D domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectMany(2.tFixed())
                .DefineSelectionDomain(C, (0..10).RangeIter(), out var domain, "bad domain")
        },
    };
    public async Task Run()
    {
        if (!(await SanityCheck()))
        {
            Console.WriteLine("Sanity check failed!");
            return;
        }
    }

    private async static Task<bool> SanityCheck()
    {
        var invalid = GetNotInvalid(await new Glancer
        {
            Name = "Sanity Check (Invalid)",
            Supplier = RUN_IMPLEMENTATION,
            Tests = SANITY_CHECKS_INVALID
        }.Glance());
        var fail = GetNotFailed(await new Glancer
        {
            Name = "Sanity Check (Fail)",
            Supplier = RUN_IMPLEMENTATION,
            Tests = SANITY_CHECKS_FAIL
        }.Glance());
        var pass = GetNotPassed(await new Glancer
        {
            Name = "Sanity Check (Pass)",
            Supplier = RUN_IMPLEMENTATION,
            Tests = SANITY_CHECKS_PASS
        }.Glance());

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("==[ SANITY CHECK ]==");
        Console.WriteLine("PASS: " + ((pass.Length == 0) ? "SANE" : "UNEXPECT " + pass.LookNicePls()));
        Console.WriteLine("FAIL: " + ((fail.Length == 0) ? "SANE" : "UNEXPECT " + fail.LookNicePls()));
        Console.WriteLine("INVALID: " + ((invalid.Length == 0) ? "SANE" : "UNEXPECT" + invalid.LookNicePls()));
        Console.WriteLine("====================");
        Console.ResetColor();
        return (pass.Length + fail.Length + invalid.Length == 0);
    }

    private static int[] GetNotPassed(IEnumerable<GlanceResult> glance) => GetWhereCondition(glance, x => !(x.CheckOk(out var tree) && tree.Evaluation));
    private static int[] GetNotFailed(IEnumerable<GlanceResult> glance) => GetWhereCondition(glance, x => !(x.CheckOk(out var tree) && !tree.Evaluation));
    private static int[] GetNotInvalid(IEnumerable<GlanceResult> glance) => GetWhereCondition(glance, x => !x.IsErr());
    private static int[] GetWhereCondition(IEnumerable<GlanceResult> glance, Predicate<GlanceResult> pred)
    {
        return glance.Enumerate().Where(x => pred(x.value)).Map(x => x.index + 1).ToArray();
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