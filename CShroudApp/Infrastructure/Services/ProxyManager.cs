using System;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using CShroudApp.Infrastructure.Interfaces;

namespace CShroudApp.Infrastructure.Services;

[SupportedOSPlatform("windows")]
public class ProxyManager : IProxyManager
{
    private const string REGISTRY_PATH = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

    private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
    private const int INTERNET_OPTION_REFRESH = 37;

    public void Enable(string proxyAddress)
    {
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                if (key == null) throw new Exception("undefined_regedit_key");
                key.SetValue("ProxyEnable", 1);
                key.SetValue("ProxyServer", proxyAddress);
            }
            
            ApplySettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при включении прокси: {ex.Message}");
        }
    }

    public void Disable()
    {
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_PATH, true))
            {
                if (key == null) throw new Exception("undefined_regedit_key");
                key.SetValue("ProxyEnable", 0);
                key.DeleteValue("ProxyServer", false);
            }
            
            ApplySettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отключении прокси: {ex.Message}");
        }
    }

    private static void ApplySettings()
    {
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
    }
}