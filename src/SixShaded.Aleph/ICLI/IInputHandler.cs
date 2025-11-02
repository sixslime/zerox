namespace SixShaded.Aleph.ICLI;

internal interface IInputHandler
{
    /// <summary>
    /// return true if handled, false if should pass to next handler.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="actionContext"></param>
    /// <returns></returns>
    public bool HandleInput(Config.AlephKeyPress key, IProgramActions actionContext);
}