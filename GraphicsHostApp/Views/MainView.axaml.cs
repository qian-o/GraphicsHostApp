using Avalonia.Controls;
using GraphicsHostApp.Contracts.Services;
using GraphicsHostApp.Services;

namespace GraphicsHostApp.Views;

public partial class MainView : UserControl
{
    private readonly IDrawingService _drawingService;

    public MainView()
    {
        _drawingService = new SimpleDrawingService();

        InitializeComponent();

        glRenderer.OnLoad += () => { _drawingService.Load([glRenderer]); };
        glRenderer.OnUpdate += _drawingService.Update;
        glRenderer.OnRender += _drawingService.Render;
    }
}
