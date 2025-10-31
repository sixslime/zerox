namespace SixShaded.Aleph.ICLI;

internal interface IInputHandler
{
    public void HandleInput(Config.AlephKeyPress key, IProgramActions actionContext);
}