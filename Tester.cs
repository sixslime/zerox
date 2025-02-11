
using System;
using System.Collections.Generic;
namespace PROTO_ZeroxFour_1;
using FourZeroOne.Core.Syntax;
using DeTes.Syntax;
using Minima.FZO;
using FourZeroOne.FZOSpec;
using Perfection;
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
        List<DeTest> tests = new();

        var test = new DeTest
        {
            InitialMemory = MEMORY_IMPLEMENTATION,
            Token = C =>
                (1..5).tFixed()
                .ReferenceAs(C, out var range)
                .tIOSelectOne()
                .DefineSelectionDomain(C, (0..4).Iterate(), out var domain)
                .tMultiply(2.tFixed())
                .tAdd(1.tFixed())
        };
        
    }


    /*
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