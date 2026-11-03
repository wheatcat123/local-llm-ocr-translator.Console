using System;
using System.Drawing;
using System.Windows.Forms;

public class TranslationOverlayForm : Form
{
    private readonly string translation;
    private readonly int fontSize;

    public TranslationOverlayForm(
        Rectangle region,
        string translation,
        int fontSize)
    {
        this.translation = translation;
        this.fontSize = fontSize;

        // 改成普通窗口
        FormBorderStyle = FormBorderStyle.Sizable;

        StartPosition = FormStartPosition.Manual;

        // 初始大小仍然使用鼠标选择框
        Bounds = region;

        TopMost = true;

        ShowInTaskbar = false;

        KeyPreview = true;

        // 标题栏
        Text = "翻译结果";

        // 允许最小化、最大化、关闭
        MinimizeBox = true;
        MaximizeBox = true;
        ControlBox = true;


        // 用 RichTextBox 承载文字
        RichTextBox textBox = new RichTextBox();

        textBox.Dock = DockStyle.Fill;

        textBox.ReadOnly = true;

        textBox.BorderStyle = BorderStyle.None;

        textBox.BackColor = Color.Black;

        textBox.ForeColor = Color.White;

        textBox.Font = new Font(
            "Microsoft YaHei UI",
            fontSize);

        textBox.Text = translation;

        textBox.WordWrap = true;

        textBox.ScrollBars = RichTextBoxScrollBars.Both;

        textBox.Padding = new Padding(10);

        Controls.Add(textBox);
    }


    protected override void OnKeyDown(
        KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }

        base.OnKeyDown(e);
    }
}