using System.Drawing;

public static class ScreenCaptureService
{
    public static Bitmap Capture(
        Rectangle region)
    {
        Bitmap bitmap =
            new Bitmap(
                region.Width,
                region.Height);


        using Graphics graphics =
            Graphics.FromImage(bitmap);


        graphics.CopyFromScreen(
            region.Location,
            Point.Empty,
            region.Size);


        return bitmap;
    }
}