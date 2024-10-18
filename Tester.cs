
using System;
using System.Collections;
using System.Collections.Generic;
using FourZeroOne.Core.Syntax;
using t = FourZeroOne.Core.Tokens;
using p = FourZeroOne.Core.Proxies;
using r = FourZeroOne.Core.Resolutions;
using a = FourZeroOne.Libraries.Axiom;
using ro = FourZeroOne.Core.Resolutions.Objects;
using FourZeroOne.Libraries.Axiom.TokenSyntax;
using System.Threading.Tasks;
using Perfection;
using ControlledFlows;
namespace PROTO_ZeroxFour_1;
public class Tester
{
    private FourZeroOne.Runtime.IRuntime _runtime;
    // Start is called before the first frame update
    public async Task Run()
    {
        // 401 is just an interpreter for 'Tokens'.
        // Tokens resolve to 'Resolutions', which change the state of the game.
        // A 'Game' of 401 can be expressed via a sequence of resolutions.
        // ALL user input is gathered via IO tokens.
        // tutorial on how to make tokens to get you up to speed:
        // (tokens for in-game usage will include resolutions of actual objects/actions, but work exactly the same)
        var token_tutorial_1 = 5.tFixed().tAdd(10.tFixed()); // 5 + 10 
        var token_tutorial_2 = Iter.Over(1, 2, 3, 4).Map(x => x.tFixed()).t_ToConstMulti(); // [1, 2, 3, 4]
        var token_tutorial_3 = token_tutorial_2.tIOSelectOne(); //prompt user to select one from [1, 2, 3, 4], and return it
        var token_tutorial_4 = Core.tSubEnvironment(RHint<ro.Number>.Hint(), new()
        {
            Environment = token_tutorial_2.tIOSelectOne().tAsVariable(out var mySelection).tYield(),
            SubToken = mySelection.tRef().tMultiply(mySelection.tRef())
        }); //creates a Sub-Environment (aka scope) where 'mySelection' stores the resolution of a user selection, then references it twice to multiply it by itself.
        // is different than just calling 'token_tutorial_2.tIO_SelectOne()' twice, that would prompt the user selection 2 times, possibly resolving different values each time (because the user could select 2 different things obv.).
        // 'Rules' can be made and applied to tokens to replace certain types of tokens with other tokens.
        // Rules are expressed by 'Proxies', which are basically just tokens, but have the ability to reference information about the token they are meant to replace (such as arguements).
        // logically, the replaced token and replacing token must both have the same resolution type.
        // MakeProxy.RuleFor<{token type to replace}, {resolution type}>({proxy statement specifying the replacement})
        var rule_tutorial_1 = ProxyStatement.BuildAsRule<t.Fixed<ro.Number>, ro.Number>(P => 4.tFixed().pDirect(P)); // makes ALL constant number tokens ('t.Fixed<ro.Number>') turn into 4 (as a constant number token).
        var rule_tutorial_2 = ProxyStatement.BuildAsRule<t.Number.Add, ro.Number>(P => P.pOriginalA().pAdd(P.pOriginalA()).pSubtract(P.pOriginalB())); // makes ALL add(A, B) tokens ('t.Number.Add') turn into subtract(add(A, A), B).
        //var rule_illogical = MakeProxy.RuleFor<t.Number.Add, ro.Bool>(P => P.pOriginalA().pIsGreaterThan(P.pOriginalB()) -- consider applying this rule to subtract(add(<number>, <number>), <number>), it would become subtract(<bool>, <number>), which does not make sense.

        /*
        var token_complicated = Core.tRecursive<ro.Number, r.Multi<ro.Number>, ro.Number>(new() // if you can figure out what this does, then you understand the language; yes its recursive (recursion is not planned to be common, but it will exist sometimes)
        {
            A = 0.tConst(),
            B = 1.Sequence(x => x + 1).Take(5).Map(x => x.tConst()).tToMulti(),
            RecursiveProxyStatement = P =>
                P.pSubEnvironment(RHint<ro.Number>.Hint(), new()
                {
                    EnvironmentProxy = P.pArrayOf(RHint<FourZeroOne.Resolution.IResolution>.Hint(), new()
                    {
                        P.pOriginalB().pAsVariable(out var pool),
                        pool.tRef().pDirect(P).pIO_SelectOne().pAsVariable(out var selection),
                        P.pOriginalA().pAsVariable(out var counter)
                    }),
                    SubProxy = P.pOriginalA().pIsGreaterThan(2.tConst().pDirect(P)).pIfTrue(RHint<ro.Number>.Hint(), new()
                    {
                        Then = P.pMetaFunction(RHint<ro.Number>.Hint(), (_) => selection.tRef().pDirect(P)),
                        Else = P.pMetaFunction(RHint<ro.Number>.Hint(), (_) => P.pRecurseWith(new()
                        {
                            A = counter.tRef().tAdd(1.tConst()).pDirect(P),
                            B = pool.tRef().tWithout(selection.tRef().tYield()).pDirect(P)
                        }).pAdd(selection.tRef().pDirect(P)))
                    }).pExecute()
                })
        });
        */
        var token_complicated = Core.tMetaRecursiveFunction(RHint<ro.Number, r.Multi<ro.Number>, ro.Number>.Hint(), (selfFunc, counter, pool) =>
        {
            return counter.tRef().tIsGreaterThan(2.tFixed()).tIfTrue(RHint<ro.Number>.Hint(), new()
            {
                Then = Core.tMetaFunction(RHint<ro.Number>.Hint(), () => { return 0.tFixed(); }),
                Else = Core.tMetaFunction(RHint<ro.Number>.Hint(), () =>
                {
                    return Core.tSubEnvironment(RHint<ro.Number>.Hint(), new()
                    {
                        Environment = pool.tRef().tIOSelectOne().tAsVariable(out var selection).tYield(),
                        SubToken = selfFunc.tRef().tExecuteWith(new()
                        {
                            A = counter.tRef().tAdd(1.tFixed()),
                            B = pool.tRef().tWithout(selection.tRef().tYield())
                        }).tAdd(selection.tRef())
                    });
                })
            }).tExecute();
        }).tExecuteWith(new()
        {
            A = 0.tFixed(),
            B = 1.Sequence(x => x + 3).Take(5).Map(x => x.tFixed()).tToMulti()
        });
        var token_test_1 = token_tutorial_2.tIOSelectMany(Iter.Over(1, 2, 3, 4).Map(x => x.tFixed()).t_ToConstMulti().tIOSelectOne());
        var token_test_2 = token_tutorial_1.tAdd(1.tFixed());
        var token_test_3 = Core.tMetaFunction(RHint<ro.Number, ro.Number, ro.Number>.Hint(), (a, b) => a.tRef().tMultiply(b.tRef())).tExecuteWith(new()
        {
            A = token_tutorial_3,
            B = token_complicated
        });
        var token_test_4 = token_tutorial_2.tMap(x => x.tRef().tMultiply(2.tFixed()));
        var token_tester = token_test_4;
        var rule_test = ProxyStatement.BuildAsRule<t.Number.Add, ro.Number>(P => P.pOriginalA().pAdd(P.pOriginalB().pAdd(1.tFixed().pDirect(P))));

        FourZeroOne.IState startState = new FourZeroOne.StateModels.Minimal();
        _runtime = new FourZeroOne.Runtimes.FrameSaving.Gebug(startState, token_tester);
        var o = await _runtime.Run();
        Console.WriteLine($"FINAL: {o}");
    }
}