using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using GraphicsHostApp.Helpers;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public class Renderer : OpenGlControlBase, IGraphicsHost<GL>
{
    public static readonly StyledProperty<int> SamplesProperty = AvaloniaProperty.Register<Renderer, int>("Samples", 4);

    private readonly Stopwatch _stopwatch = new();

    private GL? context;
    private Frame? frame;

    #region Pipelines
    private RenderPipeline? canvasPipeline;
    #endregion

    #region Meshes
    private Mesh[]? canvasMeshes;
    #endregion

    public event Action? OnLoad;
    public event Action? OnUnload;
    public event DeltaAction? OnUpdate;
    public event DeltaAction? OnRender;
    public event SizeAction? OnResize;

    public int Samples
    {
        get { return GetValue(SamplesProperty); }
        set { SetValue(SamplesProperty, value); }
    }

    protected override void OnOpenGlInit(GlInterface gl)
    {
        _stopwatch.Start();

        // Initialize the OpenGL context
        {
            context ??= GL.GetApi(gl.GetProcAddress);
            frame ??= new Frame(this);

            using Shader vs = new(this, ShaderType.VertexShader, File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Resources", "Shaders", "Canvas.vert")));
            using Shader fs = new(this, ShaderType.FragmentShader, File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Resources", "Shaders", "Canvas.frag")));
            canvasPipeline = new RenderPipeline(this, vs, fs);

            MeshFactory.GetCanvas(out Vertex[] vertices, out uint[] indices);
            canvasMeshes = [new(this, vertices, indices)];
        }

        OnLoad?.Invoke();

        OnResize?.Invoke((int)Bounds.Width, (int)Bounds.Height);
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        _stopwatch.Stop();

        OnUnload?.Invoke();

        foreach (Mesh mesh in canvasMeshes!)
        {
            mesh.Dispose();
        }
        canvasPipeline!.Dispose();
        frame!.Dispose();
        context!.Dispose();

        frame = null;
        context = null;
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        if (context == null || frame == null || canvasPipeline == null || canvasMeshes == null)
        {
            throw new InvalidOperationException("The OpenGL context has not been initialized yet.");
        }

        frame.Update((int)Bounds.Width, (int)Bounds.Height, Samples);

        frame.Bind();
        {
            OnUpdate?.Invoke(_stopwatch.Elapsed.TotalSeconds);

            OnRender?.Invoke(_stopwatch.Elapsed.TotalSeconds);
        }
        frame.Unbind();

        {
            context.BindFramebuffer(GLEnum.Framebuffer, (uint)fb);
            context.Viewport(0, 0, (uint)Bounds.Width, (uint)Bounds.Height);

            context.Clear((uint)GLEnum.ColorBufferBit | (uint)GLEnum.DepthBufferBit | (uint)GLEnum.StencilBufferBit);

            foreach (Mesh mesh in canvasMeshes)
            {
                canvasPipeline.Bind();

                canvasPipeline.SetUniform("Tex", 0, frame.Texture);

                mesh.VertexAttributePointer((uint)canvasPipeline.GetAttribLocation("In_Position"), 3, nameof(Vertex.Position));
                mesh.VertexAttributePointer((uint)canvasPipeline.GetAttribLocation("In_Normal"), 3, nameof(Vertex.Normal));
                mesh.VertexAttributePointer((uint)canvasPipeline.GetAttribLocation("In_Tangent"), 3, nameof(Vertex.Tangent));
                mesh.VertexAttributePointer((uint)canvasPipeline.GetAttribLocation("In_Bitangent"), 3, nameof(Vertex.Bitangent));
                mesh.VertexAttributePointer((uint)canvasPipeline.GetAttribLocation("In_Color"), 4, nameof(Vertex.Color));
                mesh.VertexAttributePointer((uint)canvasPipeline.GetAttribLocation("In_TexCoord"), 2, nameof(Vertex.TexCoord));

                mesh.Draw();

                canvasPipeline.Unbind();
            }

            context.BindFramebuffer(GLEnum.Framebuffer, 0);
        }

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
