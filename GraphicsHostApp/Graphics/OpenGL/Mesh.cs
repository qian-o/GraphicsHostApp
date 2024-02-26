using System.Runtime.InteropServices;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public unsafe class Mesh : GraphicsResource
{
    public Mesh(IGraphicsHost<GL> graphicsHost, Vertex[] vertices, uint[] indices) : base(graphicsHost)
    {
        Handle = GL.GenVertexArray();
        ArrayBuffer = GL.GenBuffer();
        IndexBuffer = GL.GenBuffer();
        IndexLength = indices.Length;

        GL.BindVertexArray(Handle);

        GL.BindBuffer(GLEnum.ArrayBuffer, ArrayBuffer);
        GL.BufferData<Vertex>(GLEnum.ArrayBuffer, (uint)(vertices.Length * sizeof(Vertex)), vertices, GLEnum.StaticDraw);

        GL.BindBuffer(GLEnum.ElementArrayBuffer, IndexBuffer);
        GL.BufferData<uint>(GLEnum.ElementArrayBuffer, (uint)(indices.Length * sizeof(uint)), indices, GLEnum.StaticDraw);

        GL.BindVertexArray(0);
    }

    public uint ArrayBuffer { get; }

    public uint IndexBuffer { get; }

    public int IndexLength { get; }

    protected override void Destroy(bool disposing = false)
    {
        GL.DeleteVertexArray(Handle);
        GL.DeleteBuffer(ArrayBuffer);
        GL.DeleteBuffer(IndexBuffer);
    }

    public void VertexAttributePointer(uint index, int size, string fieldName)
    {
        GL.BindVertexArray(Handle);

        GL.BindBuffer(GLEnum.ArrayBuffer, ArrayBuffer);
        GL.VertexAttribPointer(index, size, GLEnum.Float, false, (uint)sizeof(Vertex), (void*)Marshal.OffsetOf<Vertex>(fieldName));
        GL.EnableVertexAttribArray(index);
        GL.BindBuffer(GLEnum.ArrayBuffer, 0);

        GL.BindVertexArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(Handle);
        GL.DrawElements(GLEnum.Triangles, (uint)IndexLength, GLEnum.UnsignedInt, null);
        GL.BindVertexArray(0);
    }
}
