using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GraphicsHostApp.Contracts.Services;
using GraphicsHostApp.Graphics.OpenGL;
using Silk.NET.Maths;

namespace GraphicsHostApp.Services;

public unsafe partial class ExternalDrawingService : IDrawingService
{
    #region External Drawing Service
    public delegate void* GetProcAddress(string proc);

    [LibraryImport("Resources/Dependencies/GraphicsHostApp.OpenGL.dll")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    private static partial void MakeContext(GetProcAddress getProcAddress, out long id);

    [LibraryImport("Resources/Dependencies/GraphicsHostApp.OpenGL.dll")]
    private static partial void LoadScene(long id);

    [DllImport("Resources/Dependencies/GraphicsHostApp.OpenGL.dll")]
    private static extern void UpdateScene(long id, double deltaSeconds, [MarshalAs(UnmanagedType.Struct)] Vector2D<float> size);

    [LibraryImport("Resources/Dependencies/GraphicsHostApp.OpenGL.dll")]
    private static partial void DrawScene(long id, double deltaSeconds);
    #endregion

    private Renderer renderer = null!;
    private long rendererId;

    public void Load(object[] args)
    {
        renderer = (Renderer)args[0];

        MakeContext((proc) => (void*)renderer.GetContext().Context.GetProcAddress(proc), out rendererId);

        LoadScene(rendererId);
    }

    public void Update(double deltaSeconds)
    {
        Vector2D<float> size = new((float)renderer.Bounds.Width, (float)renderer.Bounds.Height);

        UpdateScene(rendererId, deltaSeconds, size);
    }

    public void Render(double deltaSeconds)
    {
        DrawScene(rendererId, deltaSeconds);
    }
}
