using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace GraphEditor.Tools;

public sealed class LineTool : DrawingTool
{
    private Line? _line;

    public override void OnPointerPressed(Point point, Canvas canvas)
    {
        StartPoint = point;
        _line = new Line
        {
            StartPoint = point,
            EndPoint = point,
            Stroke = CreateBrush(),
            StrokeThickness = Thickness
        };
        canvas.Children.Add(_line);
    }

    public override void OnPointerMoved(Point point, Canvas canvas)
    {
        if (_line is null) return;
        if (!IsInsideCanvas(point, canvas)) return;
        _line.EndPoint = point;
    }

    public override void OnPointerReleased(Point point, Canvas canvas)
    {
        if (_line is null) return;
        if (IsInsideCanvas(point, canvas))
            _line.EndPoint = point;
        _line = null;
    }

    private static bool IsInsideCanvas(Point p, Canvas canvas) =>
        p.X >= 0 && p.Y >= 0 && p.X <= canvas.Bounds.Width && p.Y <= canvas.Bounds.Height;
}