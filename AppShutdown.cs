using System;
using System.Windows.Forms;

public static class AppShutdown
{
    public static void Shutdown()
    {
        try
        {
            OCRManager.Stop();
        }
        catch
        {
        }

        try
        {
            Application.Exit();
        }
        catch
        {
        }
    }
}