using System;
using Silk.NET.OpenGLES;

namespace GraphicsHostApp.Graphics.OpenGL;

public class Buffer(IGraphicsHost<GL> graphicsHost, BufferTargetARB bufferTarget, BufferUsageARB bufferUsage, uint length = 1) : GraphicsResource(graphicsHost)
{
    public uint Length { get; } = length;

    public BufferTargetARB BufferTarget { get; } = bufferTarget;

    public BufferUsageARB BufferUsage { get; } = bufferUsage;

    protected override void Destroy(bool disposing = false)
    {
        GL.DeleteBuffer(Handle);
    }
}

public unsafe class Buffer<TDataType> : Buffer where TDataType : unmanaged
{
    public Buffer(IGraphicsHost<GL> graphicsHost, BufferTargetARB bufferTarget, BufferUsageARB bufferUsage, uint length = 1) : base(graphicsHost, bufferTarget, bufferUsage, length)
    {
        Handle = GL.GenBuffer();

        GL.BindBuffer(BufferTarget, Handle);
        GL.BufferData(BufferTarget, (uint)(Length * sizeof(TDataType)), null, BufferUsage);
        GL.BindBuffer(BufferTarget, 0);
    }

    public void SetData(TDataType[] data, uint offset = 0)
    {
        if (data.Length != Length)
        {
            throw new Exception("数据长度必须等于缓冲区长度。");
        }

        fixed (TDataType* dataPtr = data)
        {
            SetData(dataPtr, offset);
        }
    }

    public void SetData(TDataType* data, uint offset = 0)
    {
        GL.BindBuffer(BufferTarget, Handle);
        GL.BufferSubData(BufferTarget, (int)(offset * sizeof(TDataType)), (uint)(Length * sizeof(TDataType)), data);
        GL.BindBuffer(BufferTarget, 0);
    }

    public void SetData(TDataType data, uint offset = 0)
    {
        GL.BindBuffer(BufferTarget, Handle);
        GL.BufferSubData(BufferTarget, (int)(offset * sizeof(TDataType)), (uint)sizeof(TDataType), &data);
        GL.BindBuffer(BufferTarget, 0);
    }

    public TDataType[] GetData()
    {
        TDataType[] result = new TDataType[Length];

        GL.BindBuffer(BufferTarget, Handle);
        void* mapBuffer = GL.MapBufferRange(BufferTarget, 0, (uint)(Length * sizeof(TDataType)), (uint)GLEnum.MapReadBit);

        Span<TDataType> resultSpan = new(mapBuffer, (int)Length);
        resultSpan.CopyTo(result);

        GL.UnmapBuffer(BufferTarget);
        GL.BindBuffer(BufferTarget, 0);

        return result;
    }
}
