using Avalonia.Controls;
using GraphicsHostApp.Contracts.Services;
using GraphicsHostApp.Services;

namespace GraphicsHostApp.Views;

public partial class MainView : UserControl
{
    private readonly IDrawingService _drawingService1;
    private readonly IDrawingService _drawingService2;

    public MainView()
    {
        _drawingService1 = new SimpleDrawingService();
        _drawingService2 = new ExternalDrawingService();

        InitializeComponent();

        glRenderer1.OnLoad += () => { _drawingService1.Load([glRenderer1]); };
        glRenderer1.OnUpdate += _drawingService1.Update;
        glRenderer1.OnRender += _drawingService1.Render;

        glRenderer2.OnLoad += () => { _drawingService2.Load([glRenderer2]); };
        glRenderer2.OnUpdate += _drawingService2.Update;
        glRenderer2.OnRender += _drawingService2.Render;
    }
}
