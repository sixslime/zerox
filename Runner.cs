


namespace PROTO_ZeroxFour_1
{
    using System.Drawing;
    using ShapeEngine.Color;
    using ShapeEngine.Core;
    using ShapeEngine.Core.Structs;
    using ShapeEngine.Lib;
    using ControlledTasks;
    using Util;
    public static class Program
    {
        public static bool Test(out int a, out int b)
        {
            (a, b) = (1, 1);
            return true;
        }
        public async static Task Main(string[] args)
        {
            var tester = new Tester();
            //game = new MyGameClass(GameSettings.StretchMode, WindowSettings.Default);
            //game.Run();
            await tester.Run();
            /* DESIRED TESTING SPEC
             * var tester = new Tester(testableRuntime, baseState);
             * var test_1 = tester.AddTest("test name", testContext => new()
             * {
             *      State = baseState => baseState; // assumed
             *      Evaluate = 5.tFixed().tAdd(10.tFixed()),
             *      Selections = [] // optional
             *      Expect = new() // optional
             *      {
             *          Resolution = 15.tFixed(),
             *          State = prevState => prevState,
             *      }
             *      Assert = new() // optional
             *      {
             *          Token = x => x is IToken<ro.Number>;
             *          Resolution = x => x is ResObj;
             *          State = x => x is IState;
             *      }
             * });
             * 
             * tester.AddTest(testContext => new()
             * {
             *      Name = "use_example"
             *      // this test will be skipped if 'test_1' fails because this test uses 'test_1'
             *      Evaluate = test_1.Use(testContext).Token.tSubtract(1.tFixed()); //this re-uses the previous test's "Evaluate" token.
             *      Selections = [] // if 'test_1' had selections, they should appropriately be inserted internally.
             *      Expect = new()
             *      {
             *          Resolution = test_1.Use(testContext).Resolution.Mut(x => x with { Value = x.Value - 1 }); // this should be valid.
             *      }
             *      Assert = new()
             *      {
             *          Resolution = x => x.ResEquals(test_1.Use(testContext).Resolution)); // even though this is bad practice, this test should still be marked as "skipped" (even after evalutaion) if 'test_1' failed.
             *          // this means that tests should be run ENTIRELY before any output is given.
             *      }
             * }).UseAs(out var test_2);
             * 
             *
             * TestResult[] all_results = tester.RunAll();
             * TestResult[] select_results = tester.RunOnly([test_1, test_2], true);
             * TestResult[] one_result = tester.RunOnly([test_2]); // this would also implicitly evaluate test_1 (hidden).
             * record TestResult {
             *      Base =
             *      {
             *          IRuntime Runtime
             *          IState State
             *      }
             *      Test =
             *      {
             *          <test spec>...
             *      }
             *      bool Passed; // true if all Assertions and Expectations are met (and no exception is thrown).
             *      IOption<Exception> Exception;
             *      IOption<> Eval =
             *      {
             *          IState State;
             *          ResObj Resolution;
             *          Assertions =
             *          {
             *              IOption<bool> Token;
             *              IOption<bool> Resolution;
             *              IOption<bool> State;
             *          }
             *      }
             *      
             * }
             */
        }

    }
    public class MyGameClass : Game
    {
        public MyGameClass(GameSettings gameSettings, WindowSettings windowSettings) : base(gameSettings, windowSettings) { }
        protected override void DrawGame(ScreenInfo game)
        {
            Console.WriteLine("draw");
            game.Area.Draw(new ColorRgba(Color.DarkOliveGreen));
            game.Area.DrawLines(12f, new ColorRgba(Color.AntiqueWhite));
            game.MousePos.Draw(24f, new ColorRgba(Color.Lime), 36);
        }
    }
}
