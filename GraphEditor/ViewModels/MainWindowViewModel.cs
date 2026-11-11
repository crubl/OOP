using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using GraphEditor.Models;
using GraphEditor.Services;
using GraphEditor.Tools;

namespace GraphEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly Canvas _canvas;
    private readonly IDialogService _dialogService;

    private DrawingTool _activeTool;
    public DrawingTool ActiveTool
    {
        get => _activeTool;
        private set => SetField(ref _activeTool, value);
    }

    private DrawingToolType _activeToolType = DrawingToolType.Pencil;
    public DrawingToolType ActiveToolType
    {
        get => _activeToolType;
        private set
        {
            if (SetField(ref _activeToolType, value))
            {
                OnPropertyChanged(nameof(IsPencilSelected));
                OnPropertyChanged(nameof(IsLineSelected));
                OnPropertyChanged(nameof(IsRectSelected));
                OnPropertyChanged(nameof(IsEllipseSelected));
            }
        }
    }

    public bool IsPencilSelected => ActiveToolType == DrawingToolType.Pencil;
    public bool IsLineSelected => ActiveToolType == DrawingToolType.Line;
    public bool IsRectSelected => ActiveToolType == DrawingToolType.Rectangle;
    public bool IsEllipseSelected => ActiveToolType == DrawingToolType.Ellipse;

    private Color _selectedColor = Colors.Black;
    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (SetField(ref _selectedColor, value))
            {
                ActiveTool.Color = value;
                OnPropertyChanged(nameof(SelectedColorBrush));
            }
        }
    }
    public IBrush SelectedColorBrush => new SolidColorBrush(SelectedColor);

    private double _thickness = 2;
    public double Thickness
    {
        get => _thickness;
        set
        {
            if (SetField(ref _thickness, value))
                ActiveTool.Thickness = value;
        }
    }

    public ICommand SelectToolCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand SaveCommand { get; }

    public MainWindowViewModel(Canvas canvas, IDialogService dialogService)
    {
        _canvas = canvas;
        _dialogService = dialogService;

        _activeTool = CreateTool(DrawingToolType.Pencil);

        SelectToolCommand = new RelayCommand(param =>
        {
            if (param is string s && Enum.TryParse<DrawingToolType>(s, out var type))
                SelectTool(type);
        });

        ClearCommand = new RelayCommand(_ => _canvas.Children.Clear());
        SaveCommand = new RelayCommand(async _ => await SaveAsync());
    }

    public void SelectTool(DrawingToolType type)
    {
        ActiveToolType = type;
        ActiveTool = CreateTool(type);
    }

    private DrawingTool CreateTool(DrawingToolType type) => type switch
    {
        DrawingToolType.Pencil => new PencilTool { Color = SelectedColor, Thickness = Thickness },
        DrawingToolType.Line => new LineTool { Color = SelectedColor, Thickness = Thickness },
        DrawingToolType.Rectangle => new RectangleTool { Color = SelectedColor, Thickness = Thickness },
        DrawingToolType.Ellipse => new EllipseTool { Color = SelectedColor, Thickness = Thickness },
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    private async Task SaveAsync()
    {
        var path = await _dialogService.ShowSaveFileDialogAsync();
        if (string.IsNullOrWhiteSpace(path)) return;

        var bounds = _canvas.Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0) return;

        var pixelSize = new PixelSize((int)bounds.Width, (int)bounds.Height);
        using var rtb = new RenderTargetBitmap(pixelSize, new Vector(96, 96));

        rtb.Render(_canvas);

        await using var fs = System.IO.File.Create(path);
        rtb.Save(fs, 100); 
    }
}