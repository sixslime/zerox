


namespace PROTO_ZeroxFour_1
{
    using System.Drawing;
    using ShapeEngine.Color;
    using ShapeEngine.Core;
    using ShapeEngine.Core.Structs;
    using ShapeEngine.Lib;
    using ControlledTasks;
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            var tester = new Tester();
            //game = new MyGameClass(GameSettings.StretchMode, WindowSettings.Default);
            //game.Run();
            await tester.Run();
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
