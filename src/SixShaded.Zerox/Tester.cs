namespace SixShaded.Zerox;

using FourZeroOne.Core.Syntax;
using MinimaFZO;
using FourZeroOne.FZOSpec;
using r = FourZeroOne.Core.Roggis;
using k = FourZeroOne.Core.Korssas;
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

    internal static readonly CatGlanceableTest[] SANITY_CHECKS = new CatGlanceableTest[]
    {
        new("k")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertKorssa(C,
                        u => u is k.Number.Add add && add.Arg1 is k.Fixed<r.Number> a && a.Roggi.Value == 400),
        },
        new("r")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertRoggi(C, u => u.Value == 401),
        },
        new("m")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertMemory(C, u => !u.Objects.Any()),
        },
        new("meta execute internal assert")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertRoggi(C, u => u.Value == 401)
                    .tMetaBoxed()
                    .tExecute(),
        },
        new("env var")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment = [
                            10.tFixed().tAsVariable(out var xVar)
                            ],
                        Value =
                            xVar.tRef().tAdd(1.tFixed())
                                .AssertMemory(C, u => u.Objects.Count() == 1)
                                .AssertMemory(C, u => u.GetObject(xVar).Unwrap().Value is 10),
                    })
                    .AssertRoggi(C, u => u.Value is 11),
        },
        new("meta execute")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                Core.tMetaFunction<r.Number, r.Number>(
                        x =>
                            x.tRef().tAdd(10.tFixed()))
                    .tExecuteWith(new() { A = 5.tFixed() })
                    .AssertKorssa(C, u => u is k.MetaExecuted<r.Number> exe && exe.Arg1 is k.Number.Add)
                    .AssertRoggi(C, u => u.Value == 15),
        },
        new("size 1 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .WithDomain(C, (..5).ToIter(), out var domain)
                .tMultiply(2.tFixed())
                .AssertRoggi(C, u => u.Value == (domain.SelectedIndex() + 1) * 2),
        },
        new("size 4 domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                (1..10).tFixed()
                .tIOSelectMultiple(4.tFixed())
                .WithDomain(C, (..5).ToIter().Map(x => (x..(x + 4)).ToIter()), out var domain)
                .tGetIndex(2.tFixed())
                .AssertRoggi(C, u => u.Value == domain.SelectedIndicies()[1] + 1),
        },
        new("2D domain")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .WithDomain(C, (..5).ToIter(), out var d1, "outer")
                .tMultiply(
                    (1..10).tFixed()
                    .tIOSelectOne()
                    .WithDomain(C, (..5).ToIter(), out var d2, "inner"))
                .AssertRoggi(C, u => u.Value == (d1.SelectedIndex() + 1) * (d2.SelectedIndex() + 1)),
        },
        new("reference roggi")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed()
                    .ReferenceAs(C, out var reference)
                    .tAdd(1.tFixed())
                    .AssertRoggi(C, u => u.Value == reference.Roggi.Value + 1),
        },
        new("reference korssa")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed()
                    .ReferenceAs(C, out var reference)
                    .tAdd(1.tFixed().AssertKorssa(C, u => u.GetType() == reference.Korssa.GetType()))
                    .AssertKorssa(C, u => u is k.Number.Add add && add.Arg1 == reference.Korssa),
        },
        new("reference in selection")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                (..9).tFixed()
                .tIOSelectOne()
                .WithDomain(C, (..3).ToIter(), out var d1)
                .tMultiply(10.tFixed())
                .ReferenceAs(C, out var reference)
                .tSubtract(4.tFixed())
                .AssertRoggi(C, u => u.Value == reference.Roggi.Value - 4),
        },
        new("2D domain reference")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                (1..10).tFixed()
                .tIOSelectOne()
                .WithDomain(C, (..5).ToIter(), out var d1, "outer")
                .ReferenceAs(C, out var r1, "a")
                .tMultiply(
                    (1..8).tFixed()
                    .tIOSelectOne()
                    .WithDomain(C, (..5).ToIter(), out var d2, "inner")
                    .ReferenceAs(C, out var r2, "b"))
                .AssertRoggi(C, u => u.Value == r1.Roggi.Value * r2.Roggi.Value),
        },
        new("mellsano")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment = [
                            Core.tAddMellsano<r.Number, r.Number, r.Number>(new()
                            {
                                Matches = x => x.mIsType<k.Number.Add>(),
                                Definition = (origin, a, b) =>
                                    origin.tRef().tRealize().tSubtract(1.tFixed()),
                            })
                            ],
                        Value = 2.tFixed().tAdd(2.tFixed())
                            .AssertKorssa(C, u => u is k.Number.Subtract),
                    })
                    .AssertRoggi(C, u => u.Value == 3),
        },
        new("korvessa mellsano")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                Core.tSubEnvironment<r.Multi<r.Number>>(new()
                    {
                        Environment =
                        [
                            Core.tAddMellsano<r.Number, r.Number, r.Multi<r.Number>>(new()
                            {
                                Matches = x => x.mIsKorvessa(Core.Axodu, "duplicate"),
                                Definition = (_, a, b) =>
                                    a.tRef().tRealize().tDuplicate(b.tRef().tRealize().tAdd(1.tFixed())),
                            })
                        ],
                        Value = 401.tFixed().tDuplicate(3.tFixed()),
                    })
                    .AssertRoggi(C, u => u.Count == 4),
        },
        new("mellsano stacking")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment =
                            [
                                Core.tAddMellsano<r.Number, r.Number, r.Number>(new()
                                {
                                    Matches = x => x.mIsType<k.Number.Add>(),
                                    Definition = (_, a, b) =>
                                        a.tRef().tRealize().tSubtract(b.tRef().tRealize()),
                                }),
                                Core.tAddMellsano<r.Number, r.Number, r.Number>(new()
                                {
                                    Matches = x => x.mIsType<k.Number.Subtract>(),
                                    Definition = (_, _, _) =>
                                        999.tFixed(),
                                }),
                            ],
                        Value = 400.tFixed().tAdd(1.tFixed())
                            .AssertKorssa(C, u => u is k.Fixed<r.Number> num && num.Roggi.Value == 999),
                    })
                    .AssertRoggi(C, u => u.Value == 999),
        },
        new("inactive mellsano")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                Core.tSubEnvironment<r.Number>(new()
                    {
                        Environment =
                            [
                                Core.tAddMellsano<r.Number, r.Number, r.Number>(new()
                                {
                                    Matches = x => x.mIsType<k.Number.Add>(),
                                    Definition = (_, a, b) =>
                                        a.tRef().tRealize().tMultiply(b.tRef().tRealize()),
                                }),
                                Core.tAddMellsano<r.Number, r.Number, r.Number>(new()
                                {
                                    Matches = x => x.mIsType<k.Number.Subtract>(),
                                    Definition = (_, _, _) =>
                                        999.tFixed(),
                                })
                            ],
                        Value = 400.tFixed().tAdd(2.tFixed())
                            .AssertKorssa(C, u => u is k.Number.Multiply),
                    })
                    .AssertRoggi(C, u => u.Value == 800),
        },
        new("double metaboxed")
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Declaration = C =>
                400.tFixed().tAdd(1.tFixed())
                    .AssertRoggi(C, u => u.Value is 401)
                    .tMetaBoxed()
                    .AssertKorssa(C, u => u is k.Fixed<r.MetaFunction<r.Number>>)
                    .tMetaBoxed()
                    .AssertKorssa(C, u => u is k.Fixed<r.MetaFunction<r.MetaFunction<r.Number>>>)
                    .tExecute()
                    .AssertKorssa(C, u => u is k.MetaExecuted<r.MetaFunction<r.Number>>)
                    .tExecute()
                    .AssertRoggi(C, u => u.Value is 401),
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
                    .Map(check => new CatGlanceableTest($"({original.index + 1}:{check.index + 1}) {original.value.Name}")
                    {
                        InitialMemory = check.value.InitialMemory,
                        Declaration = check.value.Declaration,
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
    // 'Type<R>' is a roggi.
    // Generic macros are pointers to constant functions that take types, returning the actual function: (TypeA, TypeB) => (a, b)<TypeA, TypeB> => ...
    // The evil 'Cast<R>' korssa makes this possible.

}

/* NOTES
 *# Boxed Reference Issues (captures)
 * given 'let { u } in { () => &u }',
 * '&u' in the boxed function will be dangling when it'u passed upwards.
 * me when variable captures exist for u reason!
 * I don't even know if capturing is feasable conceptually.
 * The "solution" is just to be careful with boxed functions :P

 */

/* LANG
 * Token -> Korssa
 * Macro -> Korvessa
 * Resolution -> Roggi
 * Composition -> Roveggi
 * CompositionType -> Roveggitu
 * ComponentIdentifier -> Rovu
 * Rule -> Mellsano
 * Matcher -> Ullasem
 * Package/Plugin -> Axoi
 *
 * 'v' ~ part
 * 'u' ~ identify
 * 've' ~ sum of parts
 * 'du' ~ data identifier
 * 'tu' ~ type identifier
 * 'sem' ~ comparison / match
 * 'no' ~ mutate
 * 'sa' ~ expression / literal relation to korssa
 * 'ro' ~ final / literal relation to roggi
 * '.n' ~ concept of
 *
 * perhaps rename
 * Korvessa -> Korssave
 * Roveggi -> Roggive
 */