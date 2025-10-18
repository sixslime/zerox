namespace SixShaded.Aleph.ICLI;

public interface IAlephICLIHandle
{
    public Task AddSession(IStateFZO rootState);
    public Task Stop();
}