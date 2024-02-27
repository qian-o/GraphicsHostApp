using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public class Renderer : OpenGlControlBase, IGraphicsHost<GL>
{
    private readonly Stopwatch _stopwatch = new();

    private GL? context;

    public event Action? OnLoad;
    public event Action? OnUnload;
    public event DeltaAction? OnUpdate;
    public event DeltaAction? OnRender;
    public event SizeAction? OnResize;

    public Renderer()
    {
    }

    protected override void OnOpenGlInit(GlInterface gl)
    {
        _stopwatch.Start();

        context ??= GL.GetApi(gl.GetProcAddress);

        OnLoad?.Invoke();

        OnResize?.Invoke((int)Bounds.Width, (int)Bounds.Height);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        _stopwatch.Stop();

        OnUnload?.Invoke();

        context!.Dispose();
        context = null;
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        context?.Viewport(0, 0, (uint)Bounds.Width, (uint)Bounds.Height);

        OnUpdate?.Invoke(_stopwatch.Elapsed.TotalSeconds);

        OnRender?.Invoke(_stopwatch.Elapsed.TotalSeconds);

        Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Render);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        if (context != null)
        {
            OnResize?.Invoke((int)e.NewSize.Width, (int)e.NewSize.Height);
        }
    }

    public GL GetContext()
    {
        if (context == null)
        {
            throw new InvalidOperationException("The OpenGL context has not been initialized yet.");
        }

        return context;
    }
}
