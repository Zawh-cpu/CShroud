namespace CShroudApp.Infrastructure.Interfaces;

public interface IProcessManager
{
    void Register(IProcess process);
    void KillAll();
}