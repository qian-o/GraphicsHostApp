using System;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public class Shader : GraphicsResource
{
    public Shader(IGraphicsHost<GL> graphicsHost, ShaderType shaderType, string source) : base(graphicsHost)
    {
        Handle = GL.CreateShader(shaderType);

        GL.ShaderSource(Handle, source);
        GL.CompileShader(Handle);

        string error = GL.GetShaderInfoLog(Handle);

        if (!string.IsNullOrEmpty(error))
        {
            GL.DeleteShader(Handle);

            throw new Exception($"{shaderType}: {error}");
        }
    }

    protected override void Destroy(bool disposing = false)
    {
        GL.DeleteShader(Handle);
    }
}
