namespace SixShaded.Aleph.ICLI;

public interface IAlephICLIHandle
{
    public Task Finish { get; }
    public void AddSession(IStateFZO rootState);
    public Task Stop();
}