namespace SixShaded.Aleph.ICLI;

public interface IAlephICLIHandle
{
    public void AddSession(IStateFZO rootState);
    public Task Stop();
}