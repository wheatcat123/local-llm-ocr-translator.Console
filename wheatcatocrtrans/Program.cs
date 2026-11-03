using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;


        MenuManager.ExitRequested +=
            () =>
            {
                AppShutdown.Shutdown();
            };


        Thread consoleThread =
            new Thread(
                () =>
                {
                    MenuManager.Load(
                        MenuBuilder.Build());
                });

        consoleThread.IsBackground = true;

        consoleThread.Start();


        ApplicationConfiguration.Initialize();


        ConsoleWindowController.Show();

        ConsoleWindowController.DisableCloseButton();


        Application.Run(
            new TrayApplicationContext());
    }
}
/*class Program
{
    static async Task Main()
    {
        // =========================
        // Console 编码
        // =========================

        Console.OutputEncoding =
            Encoding.UTF8;

        Console.InputEncoding =
            Encoding.UTF8;


        // =========================
        // 读取配置
        // =========================

        AppSettings settings =
            SettingsManager.LoadIn();


        // =========================
        // 启动 llama
        // =========================

        bool llamaStarted =
            LlamaManager.LlamaStart(
                settings);


        if (!llamaStarted)
        {
            return;
        }


        // =========================
        // 等待模型上线
        // =========================

        bool llamaReady =
            await LlamaManager.LlamaWait();


        if (!llamaReady)
        {
            LlamaManager.LlamaStop();
            return;
        }


        // =========================
        // Console 菜单退出
        // → 整个程序退出
        // =========================

        MenuManager.ExitRequested +=
            () =>
            {
                Application.Exit();
            };


        // =========================
        // 启动 Console 菜单线程
        // =========================

        Thread consoleThread =
            new Thread(
                () =>
                {
                    MenuManager.Load(
                        MenuBuilder.Build());
                });


        consoleThread.IsBackground = true;

        consoleThread.Start();


        // =========================
        // 启动 WinForms
        // =========================

        ApplicationConfiguration.Initialize();
        ConsoleWindowController.Show();
        ConsoleWindowController.DisableCloseButton();

        using TrayApplicationContext tray =new TrayApplicationContext();


        
        // WinForms 主消息循环
        Application.Run(tray);


        // =========================
        // WinForms 结束
        // → 关闭 llama
        // =========================

        LlamaManager.LlamaStop();
    }
}
*/
