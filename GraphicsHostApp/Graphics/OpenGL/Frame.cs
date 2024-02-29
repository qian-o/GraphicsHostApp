using System;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public class Frame : GraphicsResource
{
    public Frame(IGraphicsHost<GL> graphicsHost) : base(graphicsHost)
    {
        Handle = GL.GenFramebuffer();

        Framebuffer = GL.GenFramebuffer();
        ColorBuffer = GL.GenRenderbuffer();
        DepthStencilBuffer = GL.GenRenderbuffer();

        Texture = new Texture(GraphicsHost);
    }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Samples { get; private set; }

    public Texture Texture { get; }

    public uint Framebuffer { get; }

    public uint ColorBuffer { get; }

    public uint DepthStencilBuffer { get; }

    protected override void Destroy(bool disposing = false)
    {
        GL.DeleteFramebuffer(Handle);

        GL.DeleteFramebuffer(Framebuffer);
        GL.DeleteRenderbuffer(ColorBuffer);
        GL.DeleteRenderbuffer(DepthStencilBuffer);

        Texture.Dispose();
    }

    public void Update(int width, int height, int samples = 1)
    {
        if (samples < 1)
        {
            throw new Exception("The number of samples must be greater than or equal to 1.");
        }

        if (Width == width && Height == height && Samples == samples)
        {
            return;
        }

        Width = width;
        Height = height;
        Samples = samples;

        if (Handle == 0)
        {
            return;
        }

        Texture.Clear((uint)Width, (uint)Height);

        GL.BindFramebuffer(GLEnum.Framebuffer, Handle);
        GL.FramebufferTexture2D(GLEnum.Framebuffer, GLEnum.ColorAttachment0, GLEnum.Texture2D, Texture.Handle, 0);
        GL.BindFramebuffer(GLEnum.Framebuffer, 0);

        // 多重采样缓冲区
        {
            GL.BindRenderbuffer(GLEnum.Renderbuffer, ColorBuffer);
            GL.RenderbufferStorageMultisample(GLEnum.Renderbuffer, (uint)Samples, GLEnum.Rgb8, (uint)Width, (uint)Height);
            GL.BindRenderbuffer(GLEnum.Renderbuffer, 0);

            GL.BindRenderbuffer(GLEnum.Renderbuffer, DepthStencilBuffer);
            GL.RenderbufferStorageMultisample(GLEnum.Renderbuffer, (uint)Samples, GLEnum.Depth32fStencil8, (uint)Width, (uint)Height);
            GL.BindRenderbuffer(GLEnum.Renderbuffer, 0);

            GL.BindFramebuffer(GLEnum.Framebuffer, Framebuffer);
            GL.FramebufferRenderbuffer(GLEnum.Framebuffer, GLEnum.ColorAttachment0, GLEnum.Renderbuffer, ColorBuffer);
            GL.FramebufferRenderbuffer(GLEnum.Framebuffer, GLEnum.DepthStencilAttachment, GLEnum.Renderbuffer, DepthStencilBuffer);
            GL.BindFramebuffer(GLEnum.Framebuffer, 0);
        }
    }

    public void Bind()
    {
        GL.BindFramebuffer(GLEnum.Framebuffer, Framebuffer);
        GL.Viewport(0, 0, (uint)Width, (uint)Height);
    }

    public void Unbind()
    {
        GL.BindFramebuffer(GLEnum.Framebuffer, 0);

        GL.BindFramebuffer(GLEnum.ReadFramebuffer, Framebuffer);
        GL.BindFramebuffer(GLEnum.DrawFramebuffer, Handle);
        GL.BlitFramebuffer(0, 0, Width, Height, 0, 0, Width, Height, (uint)GLEnum.ColorBufferBit, GLEnum.Nearest);
        GL.BindFramebuffer(GLEnum.DrawFramebuffer, 0);
        GL.BindFramebuffer(GLEnum.ReadFramebuffer, 0);
    }
}
