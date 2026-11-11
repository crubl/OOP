using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace GraphEditor.Tools;

public sealed class EllipseTool : DrawingTool
{
    private Ellipse? _ellipse;

    public override void OnPointerPressed(Point point, Canvas canvas)
    {
        StartPoint = point;
        _ellipse = new Ellipse
        {
            Stroke = CreateBrush(),
            StrokeThickness = Thickness,
            Width = 0,
            Height = 0
        };
        Canvas.SetLeft(_ellipse, point.X);
        Canvas.SetTop(_ellipse, point.Y);
        canvas.Children.Add(_ellipse);
    }

    public override void OnPointerMoved(Point point, Canvas canvas)
    {
        if (_ellipse is null || !IsInsideCanvas(point, canvas)) return;
        UpdateEllipse(point);
    }

    public override void OnPointerReleased(Point point, Canvas canvas)
    {
        if (_ellipse is null) return;
        if (IsInsideCanvas(point, canvas)) UpdateEllipse(point);
        _ellipse = null;
    }

    private void UpdateEllipse(Point current)
    {
        var x = Math.Min(StartPoint.X, current.X);
        var y = Math.Min(StartPoint.Y, current.Y);
        var w = Math.Abs(current.X - StartPoint.X);
        var h = Math.Abs(current.Y - StartPoint.Y);

        Canvas.SetLeft(_ellipse!, x);
        Canvas.SetTop(_ellipse!, y);
        _ellipse!.Width = w;
        _ellipse.Height = h;
    }

    private static bool IsInsideCanvas(Point p, Canvas canvas) =>
        p.X >= 0 && p.Y >= 0 && p.X <= canvas.Bounds.Width && p.Y <= canvas.Bounds.Height;
}