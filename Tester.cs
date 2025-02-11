
using System;
using System.Collections.Generic;
namespace PROTO_ZeroxFour_1;
using FourZeroOne.Core.Syntax;
using DeTes.Syntax;
using Minima.FZO;
using Perfection;
public class Tester
{
    public async Task Run()
    {

        var ideal1 = new DeTest
        {
            InitialMemory = new MinimaMemoryFZO(),
            Token = C =>
                (1..5).tFixed()
                .ReferenceAs(C, out var range)
                .tIOSelectOne()
                // ideal if this was made possible
                .DefineSelectionDomain(C, (0..(range.Resolution.Count-1)).ToIter(), out var domain)
        };
        var test = new DeTest
        {
            InitialMemory = new MinimaMemoryFZO(),
            Token = C =>
                (1..5).tFixed()
                .ReferenceAs(C, out var range)
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..4).ToIter(), out var domain)
                .tMultiply(2.tFixed())
                .tAdd(1.tFixed())
        };
        try
        {
            var results = await test.Realize(new()
            {
                Processor = new MinimaProcessorFZO(),
                UnitializedState = new MinimaStateFZO()
            });
            Console.WriteLine(results);
            Console.WriteLine(results.Split(out var ok, out var err) ? ok.CriticalPoint : err);
        } catch (Exception ex)
        {
            Console.WriteLine($"EXCEPTION: {ex}");
        }
        
    }
    // Test spec
    /*
     * new Test()
     * {
     *       InitialMemory = <Memory>
     *       Token = context =>
     *          [
     *            2.tFixed().tAdd(2.tFixed())
     *            .AssertResolution(x => x.Value == 4, context),
     *            8.tFixed(),
     *            12.tFixed()
     *          ].tUnion()
     *          .AssertResolution(x => x.Count == 3, context)
     *          .ReferenceAs(out var pool, context)
     *          .tIOSelectOne()
     *          .DefineSelectionDomain(0..3, out var domain, context)
     *          .tAdd(1.tFixed())
     *          .AssertResolution(x => x.Value == pool.Resolution.At(domain.SelectedIndex()).Unwrap().Value + 1, context)
     * }
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