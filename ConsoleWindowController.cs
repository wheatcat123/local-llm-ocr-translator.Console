using System;
using System.Runtime.InteropServices;

public static class ConsoleWindowController
{
    
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(
        IntPtr hWnd,
        int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(
        IntPtr hWnd);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetSystemMenu(
        IntPtr hWnd,
        bool bRevert);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DeleteMenu(
        IntPtr hMenu,
        uint nPosition,
        uint wFlags);

    private const uint SC_CLOSE = 0xF060;
    private const uint MF_BYCOMMAND = 0x00000000;
    public static void Hide()
    {
        IntPtr handle = GetConsoleWindow();

        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_HIDE);
        }
    }

    public static void Show()
    {
        IntPtr handle = GetConsoleWindow();

        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_SHOW);
            SetForegroundWindow(handle);
        }
    }
    public static void DisableCloseButton()
    {
        IntPtr consoleWindow =
            GetConsoleWindow();

        if (consoleWindow == IntPtr.Zero)
        {
            return;
        }

        IntPtr systemMenu =
            GetSystemMenu(
                consoleWindow,
                false);

        if (systemMenu == IntPtr.Zero)
        {
            return;
        }

        DeleteMenu(
            systemMenu,
            SC_CLOSE,
            MF_BYCOMMAND);
    }
}