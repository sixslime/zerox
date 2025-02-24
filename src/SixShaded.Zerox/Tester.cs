namespace SixShaded.Zerox;

using FourZeroOne.Core.Syntax;
using MinimaFZO;
using FourZeroOne.FZOSpec;
using r = FourZeroOne.Core.Resolutions;
using t = FourZeroOne.Core.Tokens;
using DeTesAssertIntegrity;
using GlanceResult = IResult<RecursiveEvalTree<DeTes.Analysis.IDeTesResult, bool>, DeTes.Analysis.EDeTesInvalidTest>;
using CatGlance;
using NotRust;
using SixLib.ICEE;
using DeTes;

public class Tester
{
    internal static readonly bool DEBUG_SANITY_CHECKS_MODE = false;

    internal static readonly DeTesFZOSupplier RUN_IMPLEMENTATION = new()
    {
        Processor = new MinimaProcessorFZO(),
        UnitializedState = new MinimaStateFZO(),
    };

    internal static readonly IMemoryFZO MEMORY_IMPLEMENTATION = new MinimaMemoryFZO();

    internal static readonly GlancableTest[] SANITY_CHECKS = new GlancableTest[]
    {
        new("t")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertToken(C,
                        u => u is t.Number.Add add && add.Arg1 is t.Fixed<r.Number> a && a.Resolution.Value == 400),
        },
        new("r")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertResolution(C, u => u.Value == 401),
        },
        new("m")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertMemory(C, u => !u.Objects.Any()),
        },
        new("meta execute internal assert")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertResolution(C, u => u.Value == 401)
                    .tMetaBoxed()
                    .tExecute(),
        },
        new("env var")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment = 10.tFixed().tAsVariable(out var xVar),
                        Value =
                            xVar.tRef().tAdd(1.tFixed())
                                .AssertMemory(C, u => u.Objects.Count() == 1)
                                .AssertMemory(C, u => u.GetObject(xVar).Unwrap().Value is 10),
                    })
                    .AssertResolution(C, u => u.Value is 11),
        },
        new("meta execute")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tMetaFunction<r.Number, r.Number>(
                        x =>
                            x.tRef().tAdd(10.tFixed()))
                    .tExecuteWith(new() { A = 5.tFixed() })
                    .AssertToken(C, u => u is t.MetaExecuted<r.Number> exe && exe.Arg1 is t.Number.Add)
                    .AssertResolution(C, u => u.Value == 15),
        },
        new("size 1 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (..5).ToIter(), out var domain)
                .tMultiply(2.tFixed())
                .AssertResolution(C, u => u.Value == (domain.SelectedIndex() + 1) * 2),
        },
        new("size 4 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectMany(4.tFixed())
                .DefineSelectionDomain(C, (..5).ToIter().Map(x => (x..(x + 4)).ToIter()), out var domain)
                .tAtIndex(2.tFixed())
                .AssertResolution(C, u => u.Value == domain.SelectedIndicies()[1] + 1),
        },
        new("2D domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (..5).ToIter(), out var d1)
                .tMultiply(
                    (1..10).tFixed()
                    .tIOSelectOne()
                    .DefineSelectionDomain(C, (..5).ToIter(), out var d2))
                .AssertResolution(C, u => u.Value == (d1.SelectedIndex() + 1) * (d2.SelectedIndex() + 1)),
        },
        new("reference resolution")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed()
                    .ReferenceAs(C, out var reference)
                    .tAdd(1.tFixed())
                    .AssertResolution(C, u => u.Value == reference.Resolution.Value + 1),
        },
        new("reference token")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed()
                    .ReferenceAs(C, out var reference)
                    .tAdd(1.tFixed().AssertToken(C, u => u.GetType() == reference.Token.GetType()))
                    .AssertToken(C, u => u is t.Number.Add add && add.Arg1 == reference.Token),
        },
        new("reference in selection")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (..9).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (..3).ToIter(), out var d1)
                .tMultiply(10.tFixed())
                .ReferenceAs(C, out var reference)
                .tSubtract(4.tFixed())
                .AssertResolution(C, u => u.Value == reference.Resolution.Value - 4),
        },
        new("2D domain reference")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .DefineSelectionDomain(C, (..5).ToIter(), out var d1)
                .ReferenceAs(C, out var r1, "a")
                .tMultiply(
                    (1..8).tFixed()
                    .tIOSelectOne()
                    .DefineSelectionDomain(C, (..5).ToIter(), out var d2)
                    .ReferenceAs(C, out var r2, "b"))
                .AssertResolution(C, u => u.Value == r1.Resolution.Value * r2.Resolution.Value),
        },
        new("rule")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment = Core.t_Env(
                            Core.tAddRule<r.Number, r.Number, r.Number>(new()
                            {
                                Matches = x => x.mIsType<t.Number.Add>(),
                                Definition = (origin, a, b) =>
                                    origin.tRef().tRealize().tSubtract(1.tFixed()),
                            })),
                        Value = 2.tFixed().tAdd(2.tFixed())
                            .AssertToken(C, u => u is t.Number.Subtract),
                    })
                    .AssertResolution(C, u => u.Value == 3),
        },
        new("rule macro")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<r.Multi<r.Number>>(new()
                    {
                        Environment = Core.t_Env(
                            Core.tAddRule<r.Number, r.Number, r.Multi<r.Number>>(new()
                            {
                                Matches = x => x.mIsMacro("core", "duplicate"),
                                Definition = (_, a, b) =>
                                    a.tRef().tRealize().tDuplicate(b.tRef().tRealize().tAdd(1.tFixed())),
                            })),
                        Value = 401.tFixed().tDuplicate(3.tFixed()),
                    })
                    .AssertResolution(C, u => u.Count == 4),
        },
        new("rule stacking")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment = Core.t_Env(
                            Core.tAddRule<r.Number, r.Number, r.Number>(new()
                            {
                                Matches = x => x.mIsType<t.Number.Add>(),
                                Definition = (_, a, b) =>
                                    a.tRef().tRealize().tSubtract(b.tRef().tRealize()),
                            }),
                            Core.tAddRule<r.Number, r.Number, r.Number>(new()
                            {
                                Matches = x => x.mIsType<t.Number.Subtract>(),
                                Definition = (_, _, _) =>
                                    999.tFixed(),
                            })),
                        Value = 400.tFixed().tAdd(1.tFixed())
                            .AssertToken(C, u => u is t.Fixed<r.Number> num && num.Resolution.Value == 999),
                    })
                    .AssertResolution(C, u => u.Value == 999),
        },
        new("inactive rule")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment = Core.t_Env(
                            Core.tAddRule<r.Number, r.Number, r.Number>(new()
                            {
                                Matches = x => x.mIsType<t.Number.Add>(),
                                Definition = (_, a, b) =>
                                    a.tRef().tRealize().tMultiply(b.tRef().tRealize()),
                            }),
                            Core.tAddRule<r.Number, r.Number, r.Number>(new()
                            {
                                Matches = x => x.mIsType<t.Number.Subtract>(),
                                Definition = (_, _, _) =>
                                    999.tFixed(),
                            })),
                        Value = 400.tFixed().tAdd(2.tFixed())
                            .AssertToken(C, u => u is t.Number.Multiply),
                    })
                    .AssertResolution(C, u => u.Value == 800),
        },
        new("double metaboxed")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertResolution(C, u => u.Value is 401)
                    .tMetaBoxed()
                    .AssertToken(C, u => u is t.Fixed<r.MetaFunction<r.Number>>)
                    .tMetaBoxed()
                    .AssertToken(C, u => u is t.Fixed<r.MetaFunction<r.MetaFunction<r.Number>>>)
                    .tExecute()
                    .AssertToken(C, u => u is t.MetaExecuted<r.MetaFunction<r.Number>>)
                    .tExecute()
                    .AssertResolution(C, u => u.Value is 401),
        },
    };

    public async Task Run()
    {
        if (DEBUG_SANITY_CHECKS_MODE)
        {
            await new Glancer
            {
                Name = "Sanity Tests Debug",
                Supplier = RUN_IMPLEMENTATION,
                Tests = SANITY_CHECKS[4..7],
            }.Glance();
            return;
        }

        if (!await SanityCheck())
        {
            Console.WriteLine("Sanity check failed!");
        }
    }

    private static async Task<bool> SanityCheck()
    {
        int[] pass = GetNotPassed(await new Glancer
        {
            Name = "Sanity Tests",
            Supplier = RUN_IMPLEMENTATION,
            Tests = SANITY_CHECKS,
        }.Glance());

        int[] fail = GetNotFailed(await new Glancer
        {
            Name = "Sanity Integrity Checks",
            Supplier = RUN_IMPLEMENTATION,
            Tests = SANITY_CHECKS.Enumerate()
                .Map(original => original.value.GenerateAssertIntegrityTests().Enumerate()
                    .Map(check => new GlancableTest($"({original.index + 1}:{check.index + 1}) {original.value.Name}")
                    {
                        InitialMemory = check.value.InitialMemory,
                        Token = check.value.Token,
                    }))
                .Flatten()
                .ToArray(),
        }.Glance());

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("==[ SANITY CHECK ]==");
        Console.WriteLine("TESTS: " + (pass.Length == 0 ? "GOOD" : "UNEXPECT " + pass.ICEE()));
        Console.WriteLine("INTEGRITY: " + (fail.Length == 0 ? "GOOD" : "UNEXPECT " + fail.ICEE()));
        Console.WriteLine("====================");
        Console.ResetColor();
        return pass.Length + fail.Length == 0;
    }

    private static int[] GetNotPassed(IEnumerable<GlanceResult> glance) =>
        GetWhereCondition(glance, x => !(x.CheckOk(out var tree) && tree.Evaluation));

    private static int[] GetNotFailed(IEnumerable<GlanceResult> glance) =>
        GetWhereCondition(glance, x => !(x.CheckOk(out var tree) && !tree.Evaluation));

    private static int[] GetNotInvalid(IEnumerable<GlanceResult> glance) => GetWhereCondition(glance, x => !x.IsErr());

    private static int[] GetWhereCondition(IEnumerable<GlanceResult> glance, Predicate<GlanceResult> pred) =>
        glance.Enumerate().Where(x => pred(x.value)).Map(x => x.index + 1).ToArray();
    // WARNING:
    // reliance on metafunctions is bringing attention to the lack of variable capturing.

    // *ACTUALLY* INSANE IDEA
    // 'Type<R>' is a resolution type.
    // Generic macros are pointers to constant functions that take types, returning the actual function: (TypeA, TypeB) => (a, b)<TypeA, TypeB> => ...
    // The evil 'Cast<R>' token makes this possible.

    // labels are specifications that match tokens:
    private interface ILabel
    { }

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