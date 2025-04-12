using System.Diagnostics;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Infrastructure.Services;

public class BaseProcess : IProcess
{
    private readonly Process _process;
    public bool IsRunning => _isRunning;
    private bool _isRunning = false;


    public event EventHandler ProcessExited = delegate { };
    public event EventHandler ProcessStarted = delegate { };
    public StreamWriter StandardInput => _process.StandardInput;

    public BaseProcess(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None)
    {
        _process = new Process();
        _process.StartInfo = processStartInfo;
        _process.EnableRaisingEvents = true;
        _process.Exited += OnProcessExited!;
        // _process.Disposed += OnProcessExited!;

        if (debug != DebugMode.None)
        {
            _process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("[STDOUT] " + e.Data); // Print each line of standard output
                }
            };

            _process.ErrorDataReceived += ((sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("[ERROR] " + e.Data);
                }
            });

        }
    }

    public void Start()
    {
        _isRunning = true;
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

    public async Task KillAsync()
    {
        if (_process.HasExited) return;
        _process.Kill();
        await _process.WaitForExitAsync();
    }
    
    private void OnProcessExited(object sender, EventArgs e)
    {
        Console.WriteLine($"PROCESS EXITED CODE -> {_process.ExitCode}");
        _isRunning = false;
        ProcessExited?.Invoke(this, EventArgs.Empty);
    }
}