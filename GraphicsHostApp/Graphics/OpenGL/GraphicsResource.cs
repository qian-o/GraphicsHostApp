using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public abstract class GraphicsResource(IGraphicsHost<GL> graphicsHost) : Disposable
{
    protected IGraphicsHost<GL> GraphicsHost { get; } = graphicsHost;

    protected GL GL => GraphicsHost.GetContext();

    public uint Handle { get; protected set; }
}
