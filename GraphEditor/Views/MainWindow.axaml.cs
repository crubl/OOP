using Avalonia.Controls;
using Avalonia.Input;
using GraphEditor.Services;
using GraphEditor.ViewModels;

namespace GraphEditor.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel? _vm;

    public MainWindow()
    {
        InitializeComponent();

        var canvas = this.FindControl<Canvas>("DrawingCanvas")!;
        _vm = new MainWindowViewModel(canvas, new DialogService(this));
        DataContext = _vm;
    }

    private void CanvasOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_vm is null) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;

        var point = e.GetPosition(DrawingCanvas);
        _vm.ActiveTool.OnPointerPressed(point, DrawingCanvas);
        e.Pointer.Capture(DrawingCanvas);
    }

    private void CanvasOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_vm is null) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;

        var point = e.GetPosition(DrawingCanvas);
        _vm.ActiveTool.OnPointerMoved(point, DrawingCanvas);
    }

    private void CanvasOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_vm is null) return;

        var point = e.GetPosition(DrawingCanvas);
        _vm.ActiveTool.OnPointerReleased(point, DrawingCanvas);
        e.Pointer.Capture(null);
    }
}