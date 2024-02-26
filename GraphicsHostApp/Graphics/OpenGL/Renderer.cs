using System;
using System.Diagnostics;
using System.Timers;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public class Renderer : OpenGlControlBase, IGraphicsHost<GL>
{
    private readonly Stopwatch _stopwatch = new();
    private readonly Timer _updateTimer = new(16);

    private GL? context;

    public event Action? OnLoad;
    public event Action? OnUnload;
    public event DeltaAction? OnUpdate;
    public event DeltaAction? OnRender;
    public event SizeAction? OnResize;

    public Renderer()
    {
        _updateTimer.Elapsed += (sender, args) =>
        {
            if (context != null)
            {
                Dispatcher.UIThread.Invoke(() => RequestNextFrameRendering());
            }
        };
    }

    protected override void OnOpenGlInit(GlInterface gl)
    {
        _stopwatch.Start();
        _updateTimer.Start();

        context ??= GL.GetApi(gl.GetProcAddress);

        OnLoad?.Invoke();

        OnResize?.Invoke((int)Bounds.Width, (int)Bounds.Height);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        _stopwatch.Stop();
        _updateTimer.Stop();

        OnUnload?.Invoke();

        context!.Dispose();
        context = null;
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        OnUpdate?.Invoke(_stopwatch.Elapsed.TotalSeconds);

        OnRender?.Invoke(_stopwatch.Elapsed.TotalSeconds);
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
