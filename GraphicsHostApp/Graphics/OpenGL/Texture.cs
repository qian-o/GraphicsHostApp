using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public unsafe class Texture : GraphicsResource
{
    public Texture(IGraphicsHost<GL> graphicsHost) : base(graphicsHost)
    {
        Handle = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, Handle);

        GL.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Nearest);
        GL.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Nearest);
        GL.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)GLEnum.ClampToEdge);
        GL.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)GLEnum.ClampToEdge);

        GL.BindTexture(GLEnum.Texture2D, 0);
    }

    protected override void Destroy(bool disposing = false)
    {
        GL.DeleteTexture(Handle);
    }

    public void Write(uint width, uint height, byte* data, bool isAlpha = false)
    {
        GL.BindTexture(GLEnum.Texture2D, Handle);

        if (isAlpha)
        {
            GL.TexImage2D(GLEnum.Texture2D, 0, (int)GLEnum.Rgba, width, height, 0, GLEnum.Rgba, GLEnum.UnsignedByte, data);
        }
        else
        {
            GL.TexImage2D(GLEnum.Texture2D, 0, (int)GLEnum.Rgb, width, height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, data);
        }

        GL.BindTexture(GLEnum.Texture2D, 0);
    }

    public void Clear(uint width, uint height, bool isAlpha = false)
    {
        GL.BindTexture(GLEnum.Texture2D, Handle);

        if (isAlpha)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        else
        {
            GL.TexImage2D(GLEnum.Texture2D, 0, (int)GLEnum.Rgb8, width, height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, null);
        }

        GL.BindTexture(GLEnum.Texture2D, 0);
    }
}
