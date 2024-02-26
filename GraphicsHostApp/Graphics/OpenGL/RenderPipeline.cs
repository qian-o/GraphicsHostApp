using System;
using System.Reflection;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public unsafe class RenderPipeline : GraphicsResource
{
    public RenderPipeline(IGraphicsHost<GL> graphicsHost, Shader vs, Shader fs) : base(graphicsHost)
    {
        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, vs.Handle);
        GL.AttachShader(Handle, fs.Handle);
        GL.LinkProgram(Handle);

        string error = GL.GetProgramInfoLog(Handle);

        if (!string.IsNullOrEmpty(error))
        {
            GL.DeleteProgram(Handle);

            throw new Exception($"Link: {error}");
        }
    }

    protected override void Destroy(bool disposing = false)
    {
        GL.DeleteProgram(Handle);
    }

    public int GetAttribLocation(string name)
    {
        int location = GL.GetAttribLocation(Handle, name);

        if (location < 0)
        {

        }

        return GL.GetAttribLocation(Handle, name);
    }

    public int GetUniformLocation(string name)
    {
        int location = GL.GetUniformLocation(Handle, name);

        if (location < 0)
        {

        }

        return GL.GetUniformLocation(Handle, name);
    }

    public void SetUniform(string name, int value)
    {
        GL.Uniform1(GetUniformLocation(name), value);
    }

    public void SetUniform(string name, float value)
    {
        GL.Uniform1(GetUniformLocation(name), value);
    }

    public void SetUniform(string name, Vector2D<float> value)
    {
        GL.Uniform2(GetUniformLocation(name), value.X, value.Y);
    }

    public void SetUniform(string name, Vector3D<float> value)
    {
        GL.Uniform3(GetUniformLocation(name), value.X, value.Y, value.Z);
    }

    public void SetUniform(string name, Vector4D<float> value)
    {
        GL.Uniform4(GetUniformLocation(name), value.X, value.Y, value.Z, value.W);
    }

    public void SetUniform(string name, Matrix2X2<float> value)
    {
        GL.UniformMatrix2(GetUniformLocation(name), 1, false, (float*)&value);
    }

    public void SetUniform(string name, Matrix3X3<float> value)
    {
        GL.UniformMatrix3(GetUniformLocation(name), 1, false, (float*)&value);
    }

    public void SetUniform(string name, Matrix4X4<float> value)
    {
        GL.UniformMatrix4(GetUniformLocation(name), 1, false, (float*)&value);
    }

    public void SetUniform<T>(string name, T value) where T : struct
    {
        if (!string.IsNullOrEmpty(name))
        {
            name = $"{name}.";
        }

        foreach (FieldInfo field in value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            Type fieldType = field.FieldType;

            if (fieldType == typeof(int))
            {
                SetUniform($"{name}{field.Name}", (int)field.GetValue(value)!);
            }
            else if (fieldType == typeof(float))
            {
                SetUniform($"{name}{field.Name}", (float)field.GetValue(value)!);
            }
            else if (fieldType == typeof(Vector2D<float>))
            {
                SetUniform($"{name}{field.Name}", (Vector2D<float>)field.GetValue(value)!);
            }
            else if (fieldType == typeof(Vector3D<float>))
            {
                SetUniform($"{name}{field.Name}", (Vector3D<float>)field.GetValue(value)!);
            }
            else if (fieldType == typeof(Vector4D<float>))
            {
                SetUniform($"{name}{field.Name}", (Vector4D<float>)field.GetValue(value)!);
            }
            else if (fieldType == typeof(Matrix2X2<float>))
            {
                SetUniform($"{name}{field.Name}", (Matrix2X2<float>)field.GetValue(value)!);
            }
            else if (fieldType == typeof(Matrix3X3<float>))
            {
                SetUniform($"{name}{field.Name}", (Matrix3X3<float>)field.GetValue(value)!);
            }
            else if (fieldType == typeof(Matrix4X4<float>))
            {
                SetUniform($"{name}{field.Name}", (Matrix4X4<float>)field.GetValue(value)!);
            }
            else
            {
                throw new NotSupportedException($"不支持的类型：{fieldType}。");
            }
        }
    }

    public void Bind()
    {
        GL.UseProgram(Handle);
    }

    public void Unbind()
    {
        GL.UseProgram(0);
    }
}
