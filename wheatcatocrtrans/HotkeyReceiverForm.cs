using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class HotkeyReceiverForm : Form
{
    private const int WM_HOTKEY = 0x0312;

    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint MOD_NOREPEAT = 0x4000;

    private const int HOTKEY_ID = 100;

    // 这就是我们说的 HotkeyPressed
    public event EventHandler? HotkeyPressed;


    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(
        IntPtr hWnd,
        int id,
        uint fsModifiers,
        uint vk);


    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(
        IntPtr hWnd,
        int id);


    public HotkeyReceiverForm()
    {
        FormBorderStyle = FormBorderStyle.None;

        ShowInTaskbar = false;

        StartPosition = FormStartPosition.Manual;

        Location = new Point(-10000, -10000);

        Size = new Size(1, 1);

        Opacity = 0;
    }


    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        bool registered =
            RegisterHotKey(
                Handle,
                HOTKEY_ID,
                MOD_CONTROL |
                MOD_SHIFT |
                MOD_NOREPEAT,
                (uint)Keys.T);

        if (!registered)
        {
            throw new Exception(
                "Ctrl + Shift + T 注册失败，可能已经被其他程序占用。");
        }
    }


    protected override void OnHandleDestroyed(EventArgs e)
    {
        UnregisterHotKey(
            Handle,
            HOTKEY_ID);

        base.OnHandleDestroyed(e);
    }


    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_HOTKEY)
        {
            if (m.WParam.ToInt32() == HOTKEY_ID)
            {
                // 这里就是“发出事件”
                HotkeyPressed?.Invoke(
                    this,
                    EventArgs.Empty);
            }
        }

        base.WndProc(ref m);
    }
}
