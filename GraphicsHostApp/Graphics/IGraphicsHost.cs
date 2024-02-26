using System;
using Silk.NET.Core.Native;

namespace GraphicsHostApp.Graphics;

public delegate void DeltaAction(double deltaSeconds);

public delegate void SizeAction(int width, int height);

public interface IGraphicsHost<TContext> where TContext : NativeAPI
{
    event Action? OnLoad;

    event Action? OnUnload;

    event DeltaAction? OnUpdate;

    event DeltaAction? OnRender;

    event SizeAction? OnResize;

    TContext GetContext();
}
