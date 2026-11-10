using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace GraphEditor.Tools;

public sealed class PencilTool : DrawingTool
{
    private Polyline? _polyline;

    public override void OnPointerPressed(Point point, Canvas canvas)
    {
        StartPoint = point;
        _polyline = new Polyline
        {
            Stroke = CreateBrush(),
            StrokeThickness = Thickness,
            StrokeLineCap = PenLineCap.Round,
            StrokeJoin = PenLineJoin.Round
        };
        _polyline.Points.Add(point);
        canvas.Children.Add(_polyline);
    }

    public override void OnPointerMoved(Point point, Canvas canvas)
    {
        if (_polyline is null) return;
        if (!IsInsideCanvas(point, canvas)) return;
        _polyline.Points.Add(point);
    }

    public override void OnPointerReleased(Point point, Canvas canvas)
    {
        _polyline = null;
    }

    private static bool IsInsideCanvas(Point p, Canvas canvas) =>
        p.X >= 0 && p.Y >= 0 && p.X <= canvas.Bounds.Width && p.Y <= canvas.Bounds.Height;
}