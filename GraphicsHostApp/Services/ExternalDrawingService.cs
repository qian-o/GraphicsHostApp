using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GraphicsHostApp.Contracts.Services;
using GraphicsHostApp.Graphics.OpenGL;
using Silk.NET.Maths;

namespace GraphicsHostApp.Services;

public unsafe partial class ExternalDrawingService : IDrawingService
{
    static ExternalDrawingService()
    {
        NativeLibrary.SetDllImportResolver(typeof(ExternalDrawingService).Assembly, (libraryName, assembly, searchPath) =>
        {
            libraryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{libraryName}.dll" : $"lib{libraryName}.so";

            string libPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Dependencies", libraryName);

            NativeLibrary.TryLoad(libPath, out nint handle);

            return handle;
        });
    }

    #region External Drawing Service
    public delegate void* GetProcAddress(string proc);

    [LibraryImport("GraphicsHostApp.OpenGL")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    private static partial void MakeContext(GetProcAddress getProcAddress, out long id);

    [LibraryImport("GraphicsHostApp.OpenGL")]
    private static partial void LoadScene(long id);

    [LibraryImport("GraphicsHostApp.OpenGL")]
    private static partial void UpdateScene(long id, double deltaSeconds, Vector2D<float>* size);

    [LibraryImport("GraphicsHostApp.OpenGL")]
    private static partial void DrawScene(long id, double deltaSeconds);
    #endregion

    private Renderer renderer = null!;
    private long rendererId;

    public void Load(object[] args)
    {
        renderer = (Renderer)args[0];

        MakeContext((proc) =>
        {
            if (renderer.GetContext().Context.TryGetProcAddress(proc, out nint addr))
            {
                return (void*)addr;
            }

            return (void*)0;

        }, out rendererId);

        LoadScene(rendererId);
    }

    public void Update(double deltaSeconds)
    {
        Vector2D<float> size = new((float)renderer.Bounds.Width, (float)renderer.Bounds.Height);

        UpdateScene(rendererId, deltaSeconds, &size);
    }

    public void Render(double deltaSeconds)
    {
        DrawScene(rendererId, deltaSeconds);
    }
}
