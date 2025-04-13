using System.Diagnostics;
using CShroudApp.Infrastructure.Data.Config;

namespace CShroudApp.Application.Factories;

public interface IProcessFactory
{
    void Create(ProcessStartInfo processStartInfo, DebugMode debug = DebugMode.None);
}