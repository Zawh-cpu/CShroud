using System.Diagnostics;
using CShroud.Infrastructure.Interfaces;

namespace CShroud.Infrastructure.Services;

public class BaseProcess : IProcess
{
    private readonly Process _process;
    public bool IsRunning => _process.HasExited;

    public event EventHandler ProcessExited = delegate { };
    public event EventHandler ProcessStarted = delegate { };

    public BaseProcess(ProcessStartInfo processStartInfo)
    {
        _process = new Process();
        _process.StartInfo = processStartInfo;
        _process.Exited += OnProcessExited!;


        _process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine("[STDOUT] " + e.Data); // Print each line of standard output
            }
        };
    }

    public void Start()
    {
        ProcessStarted?.Invoke(this, EventArgs.Empty);
        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
    }

    public void Kill()
    {
        if (!_process.HasExited)
        {
            _process.Kill();
        }
    }

    private void OnProcessExited(object sender, EventArgs e)
    {
        ProcessExited?.Invoke(this, EventArgs.Empty);
    }
}