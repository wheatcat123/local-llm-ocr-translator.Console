using System;
using System.Drawing;
using System.Windows.Forms;

public class TrayApplicationContext
    : ApplicationContext
{
    private readonly NotifyIcon trayIcon;

    private readonly ContextMenuStrip trayMenu;

    private readonly HotkeyReceiverForm hotkeyWindow;


    public TrayApplicationContext()
    {
        // =========================
        // 托盘菜单
        // =========================

        trayMenu =
            new ContextMenuStrip();


        ToolStripMenuItem showConsole =
            new ToolStripMenuItem(
                "显示控制台");


        showConsole.Click +=
            (sender, e) =>
            {
                ConsoleWindowController.Show();
            };


        ToolStripMenuItem hideConsole =
            new ToolStripMenuItem(
                "隐藏控制台");


        hideConsole.Click +=
            (sender, e) =>
            {
                ConsoleWindowController.Hide();
            };


        ToolStripMenuItem exit =
            new ToolStripMenuItem(
                "退出程序");


        exit.Click +=
            (sender, e) =>
            {
                ExitApplication();
            };


        trayMenu.Items.Add(
            showConsole);

        trayMenu.Items.Add(
            hideConsole);

        trayMenu.Items.Add(
            new ToolStripSeparator());

        trayMenu.Items.Add(
            exit);


        // =========================
        // 托盘图标
        // =========================

        trayIcon =
            new NotifyIcon();


        trayIcon.Icon =
            SystemIcons.Application;


        trayIcon.Text =
            "Wheat_Cat OCR Translator";


        trayIcon.ContextMenuStrip =
            trayMenu;


        trayIcon.Visible =
            true;


        trayIcon.DoubleClick +=
            (sender, e) =>
            {
                ConsoleWindowController.Show();
            };


        // =========================
        // 全局热键
        // =========================

        hotkeyWindow =
            new HotkeyReceiverForm();


        hotkeyWindow.HotkeyPressed +=
            async (sender, e) =>
            {
                await OCRManager.TranslateOnceAsync();
            };


        // Show 一次是为了真正创建 HWND
        hotkeyWindow.Show();

        hotkeyWindow.Hide();
    }


    private void ExitApplication()
    {
            trayIcon.Visible = false;

            trayIcon.Dispose();

            AppShutdown.Shutdown();
    }


    protected override void Dispose(
        bool disposing)
    {
        if (disposing)
        {
            // 关闭 OCR 新任务
            OCRManager.Stop();

            // 关闭所有翻译窗口
            OCRManager.CloseAllOverlays();


            // 销毁快捷键窗口
            if (!hotkeyWindow.IsDisposed)
            {
                hotkeyWindow.Close();
            }


            // 销毁托盘
            trayIcon.Visible = false;

            trayIcon.Dispose();

            trayMenu.Dispose();
        }


        base.Dispose(disposing);
    }
}
