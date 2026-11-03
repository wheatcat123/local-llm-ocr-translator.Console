using System;
using System.Drawing;
using System.Windows.Forms;

public class RegionSelectorForm : Form
{
    private Point startPoint;
    private Rectangle selectionRectangle;

    private bool selecting;


    public Rectangle SelectedRegion
    {
        get;
        private set;
    }


    public RegionSelectorForm()
    {
        FormBorderStyle =
            FormBorderStyle.None;

        ShowInTaskbar = false;

        TopMost = true;

        Cursor = Cursors.Cross;

        DoubleBuffered = true;

        KeyPreview = true;

        BackColor = Color.Black;

        Opacity = 0.35;


        Rectangle screen =
            SystemInformation.VirtualScreen;

        Bounds = screen;

        StartPosition =
            FormStartPosition.Manual;
    }


    protected override void OnShown(
        EventArgs e)
    {
        base.OnShown(e);

        Activate();

        Focus();
    }


    protected override void OnMouseDown(
        MouseEventArgs e)
    {
        if (e.Button ==
            MouseButtons.Left)
        {
            selecting = true;

            startPoint = e.Location;

            selectionRectangle =
                new Rectangle(
                    startPoint,
                    Size.Empty);

            Invalidate();
        }

        base.OnMouseDown(e);
    }


    protected override void OnMouseMove(
        MouseEventArgs e)
    {
        if (!selecting)
        {
            return;
        }


        selectionRectangle =
            MakeRectangle(
                startPoint,
                e.Location);


        Invalidate();

        base.OnMouseMove(e);
    }


    protected override void OnMouseUp(
        MouseEventArgs e)
    {
        if (e.Button !=
            MouseButtons.Left)
        {
            return;
        }


        selecting = false;


        selectionRectangle =
            MakeRectangle(
                startPoint,
                e.Location);


        if (selectionRectangle.Width < 5 ||
            selectionRectangle.Height < 5)
        {
            DialogResult =
                DialogResult.Cancel;

            Close();

            return;
        }


        Rectangle screen =
            SystemInformation.VirtualScreen;


        SelectedRegion =
            new Rectangle(
                selectionRectangle.X +
                    screen.X,

                selectionRectangle.Y +
                    screen.Y,

                selectionRectangle.Width,

                selectionRectangle.Height);


        DialogResult =
            DialogResult.OK;

        Close();
    }


    protected override void OnKeyDown(
        KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            DialogResult =
                DialogResult.Cancel;

            Close();
        }

        base.OnKeyDown(e);
    }


    protected override void OnPaint(
        PaintEventArgs e)
    {
        base.OnPaint(e);


        if (!selecting)
        {
            return;
        }


        using Pen pen =
            new Pen(
                Color.Red,
                2);


        e.Graphics.DrawRectangle(
            pen,
            selectionRectangle);
    }


    private static Rectangle MakeRectangle(
        Point a,
        Point b)
    {
        int x =
            Math.Min(a.X, b.X);

        int y =
            Math.Min(a.Y, b.Y);

        int width =
            Math.Abs(a.X - b.X);

        int height =
            Math.Abs(a.Y - b.Y);


        return new Rectangle(
            x,
            y,
            width,
            height);
    }
}