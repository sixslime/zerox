namespace SixShaded.Zerox
{
    internal class Program
    {
        public async static Task Main(string[] args)
        {

            var tester = new Tester();
            //game = new MyGameClass(GameSettings.StretchMode, WindowSettings.Default);
            //game.Run();
            await tester.Run();
        }
    }
}
