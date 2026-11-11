using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace GraphEditor.Tools;

public sealed class RectangleTool : DrawingTool
{
    private Rectangle? _rect;

    public override void OnPointerPressed(Point point, Canvas canvas)
    {
        StartPoint = point;
        _rect = new Rectangle
        {
            Stroke = CreateBrush(),
            StrokeThickness = Thickness,
            Width = 0,
            Height = 0
        };
        Canvas.SetLeft(_rect, point.X);
        Canvas.SetTop(_rect, point.Y);
        canvas.Children.Add(_rect);
    }

    public override void OnPointerMoved(Point point, Canvas canvas)
    {
        if (_rect is null || !IsInsideCanvas(point, canvas)) return;
        UpdateRect(point);
    }

    public override void OnPointerReleased(Point point, Canvas canvas)
    {
        if (_rect is null) return;
        if (IsInsideCanvas(point, canvas)) UpdateRect(point);
        _rect = null;
    }

    private void UpdateRect(Point current)
    {
        var x = Math.Min(StartPoint.X, current.X);
        var y = Math.Min(StartPoint.Y, current.Y);
        var w = Math.Abs(current.X - StartPoint.X);
        var h = Math.Abs(current.Y - StartPoint.Y);

        Canvas.SetLeft(_rect!, x);
        Canvas.SetTop(_rect!, y);
        _rect!.Width = w;
        _rect.Height = h;
    }

    private static bool IsInsideCanvas(Point p, Canvas canvas) =>
        p.X >= 0 && p.Y >= 0 && p.X <= canvas.Bounds.Width && p.Y <= canvas.Bounds.Height;
}