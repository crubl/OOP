using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace GraphEditor.Tools;

public abstract class DrawingTool
{
    private Color _color = Colors.Black;
    private double _thickness = 2;

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public double Thickness
    {
        get => _thickness;
        set => _thickness = value <= 0 ? 1 : value;
    }

    protected Point StartPoint { get; set; }

    public abstract void OnPointerPressed(Point point, Canvas canvas);
    public abstract void OnPointerMoved(Point point, Canvas canvas);
    public abstract void OnPointerReleased(Point point, Canvas canvas);

    protected IBrush CreateBrush() => new SolidColorBrush(Color);

    protected IPen CreatePen() => new Pen(CreateBrush(), Thickness);
}