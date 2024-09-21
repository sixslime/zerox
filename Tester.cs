
using System;
using System.Collections;
using System.Collections.Generic;
using FourZeroOne.Core.ProxySyntax;
using FourZeroOne.Core.TokenSyntax;
using t = FourZeroOne.Core.Tokens;
using p = FourZeroOne.Core.Proxies;
using r = FourZeroOne.Core.Resolutions;
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
        var token_tutorial_1 = 1.tConst().tAdd(2.tConst().tAdd(3.tConst())); // 5 + 10 
        var token_tutorial_2 = Iter.Over(1, 2, 3, 4).Map(x => x.tConst()).tToMulti(); // [1, 2, 3, 4]
        var token_tutorial_3 = token_tutorial_2.tIO_SelectOne(); //prompt user to select one from [1, 2, 3, 4], and return it
        var token_tutorial_4 = MakeToken.tSubEnvironment<r.Number>(new()
        {
            Environment = token_tutorial_2.tIO_SelectOne().tAs(out var mySelection).tYield(),
            SubToken = mySelection.tRef().tMultiply(mySelection.tRef())
        }); //creates a Sub-Environment (aka scope) where 'mySelection' stores the resolution of a user selection, then references it twice to multiply it by itself.
        // is different than just calling 'token_tutorial_2.tIO_SelectOne()' twice, that would prompt the user selection 2 times, possibly resolving different values each time (because the user could select 2 different things obv.).

        // 'Rules' can be made and applied to tokens to replace certain types of tokens with other tokens.
        // Rules are expressed by 'Proxies', which are basically just tokens, but have the ability to reference information about the token they are meant to replace (such as arguements).
        // logically, the replaced token and replacing token must both have the same resolution type.
        // MakeProxy.AsRuleFor<{token type to replace}, {resolution type}>({proxy statement specifying the replacement})
        var rule_tutorial_1 = MakeProxy.AsRuleFor<t.Fixed<r.Number>, r.Number>(P => 4.tConst().pDirect(P)); // makes ALL constant number tokens ('t.Fixed<r.Number>') turn into 4 (as a constant number token).
        var rule_tutorial_2 = MakeProxy.AsRuleFor<t.Number.Add, r.Number>(P => P.pOriginalA().pAdd(P.pOriginalA()).pSubtract(P.pOriginalB())); // makes ALL add(A, B) tokens ('t.Number.Add') turn into subtract(add(A, A), B).
        //var rule_illogical = MakeProxy.AsRuleFor<t.Number.Add, r.Bool>(P => P.pOriginalA().pIsGreaterThan(P.pOriginalB()) -- consider applying this rule to subtract(add(<number>, <number>), <number>), it would become subtract(<bool>, <number>), which does not make sense.

        var token_complicated = MakeToken.tRecursive<r.Number, r.Multi<r.Number>, r.Number>(new() // if you can figure out what this does, then you understand the language; yes its recursive (recursion is not planned to be common, but it will exist sometimes)
        {
            A = 0.tConst(),
            B = 1.Sequence(x => x + 1).Take(5).Map(x => x.tConst()).tToMulti(),
            RecursiveProxyStatement = P =>
                P.pSubEnvironment(RHint<r.Number>.Hint(), new()
                {
                    EnvironmentProxy = P.pArrayOf(RHint<FourZeroOne.Resolution.IResolution>.Hint(), new()
                    {
                        P.pOriginalB().pAsVariable(out var pool),
                        pool.tRef().pDirect(P).pIO_SelectOne().pAsVariable(out var selection),
                        P.pOriginalA().pAsVariable(out var counter)
                    }),
                    SubProxy = P.pOriginalA().pIsGreaterThan(2.tConst().pDirect(P)).pIfTrue(RHint<r.Number>.Hint(), new()
                    {
                        Then = selection.tRef().pDirect(P).pAsAction(),
                        Else = P.pRecurseWith(new()
                        {
                            A = counter.tRef().tAdd(1.tConst()).pDirect(P),
                            B = pool.tRef().tWithout(selection.tRef().tYield()).pDirect(P)
                        }).pAdd(selection.tRef().pDirect(P)).pAsAction()
                    }).pPerform()
                })
        });
        var token_test = token_tutorial_1;
        var rule_test = MakeProxy.AsRuleFor<t.Number.Add, r.Number>(P => P.pOriginalA().pAdd(P.pOriginalB().pAdd(1.tConst().pDirect(P))));

        var startState = new FourZeroOne.State()
        {
            Rules = new() { Elements = [rule_test] },
            Variables = new(7),
            Board = new() { }
        };
        _runtime = new FourZeroOne.Runtimes.FrameSaving.Gebug(startState, token_test);
        //DEV - Consider reworking how RecieveToken/RecieveResolution work, they are not called in the same order they used to be.
        var o = await _runtime.Run();
        Console.WriteLine($"FINAL: {o}");
    }
}