namespace SixShaded.Aleph.ICLI;
using State;
internal static class Extensions
{
    public static void BackToTopLevel(this IProgramActions actions)
    {
        actions.SetState(
        state =>
            state.WithCurrentSession(
            session =>
                session with
                {
                    UIContext = new ESessionUIContext.TopLevel()
                }));
    }
}