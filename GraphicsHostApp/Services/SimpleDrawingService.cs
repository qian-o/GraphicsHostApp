﻿using System;
using System.IO;
using GraphicsHostApp.Contracts.Services;
using GraphicsHostApp.Graphics;
using GraphicsHostApp.Graphics.OpenGL;
using GraphicsHostApp.Helpers;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using Shader = GraphicsHostApp.Graphics.OpenGL.Shader;

namespace GraphicsHostApp.Services;

public class SimpleDrawingService : IDrawingService
{
    #region Uniforms
    private struct UniTransforms
    {
        public Matrix4X4<float> Model;

        public Matrix4X4<float> View;

        public Matrix4X4<float> Projection;

        public Matrix4X4<float> ObjectToWorld;

        public Matrix4X4<float> ObjectToClip;

        public Matrix4X4<float> WorldToObject;
    }

    private struct UniParameters
    {
        public Vector4D<float> Color;
    }
    #endregion

    private Renderer renderer = null!;
    private Camera camera = null!;

    #region Pipelines
    private RenderPipeline simplePipeline = null!;
    private RenderPipeline solidColorPipeline = null!;
    #endregion

    #region Meshes
    private Mesh[] cubeMeshes = null!;
    #endregion

    private Matrix4X4<float> model = Matrix4X4<float>.Identity;
    private Vector4D<float> color = new(1.0f, 0.0f, 0.0f, 1.0f);

    public void Load(object[] args)
    {
        renderer = (Renderer)args[0];
        camera = new Camera()
        {
            Position = new Vector3D<float>(0.0f, 2.0f, 8.0f),
            Fov = 45.0f
        };

        using Shader vs1 = new(renderer, ShaderType.VertexShader, File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Resources", "Shaders", "Simple.vert")));
        using Shader fs1 = new(renderer, ShaderType.FragmentShader, File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Resources", "Shaders", "Simple.frag")));
        simplePipeline = new RenderPipeline(renderer, vs1, fs1);

        using Shader vs2 = new(renderer, ShaderType.VertexShader, File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Resources", "Shaders", "SolidColor.vert")));
        using Shader fs2 = new(renderer, ShaderType.FragmentShader, File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Resources", "Shaders", "SolidColor.frag")));

        solidColorPipeline = new RenderPipeline(renderer, vs2, fs2);

        MeshFactory.GetCube(out Vertex[] vertices, out uint[] indices);
        cubeMeshes = [new(renderer, vertices, indices)];
    }

    public void Update(double deltaSeconds)
    {
        model = Matrix4X4.CreateFromAxisAngle(new Vector3D<float>(0.0f, 1.0f, 0.0f), (float)deltaSeconds);
        color = Vector4D.Lerp(Vector4D<float>.One, new Vector4D<float>(1.0f, 0.0f, 0.0f, 1.0f), (float)Math.Sin(deltaSeconds));

        camera.Width = (int)renderer.Bounds.Width;
        camera.Height = (int)renderer.Bounds.Height;
    }

    public void Render(double deltaSeconds)
    {
        GL gl = renderer.GetContext();

        gl.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
        gl.Clear((uint)GLEnum.ColorBufferBit | (uint)GLEnum.DepthBufferBit | (uint)GLEnum.StencilBufferBit);

        // Cube
        {
            Matrix4X4<float> m = model * Matrix4X4.CreateTranslation(new Vector3D<float>(0.0f, 0.0f, 0.0f));

            foreach (Mesh mesh in cubeMeshes)
            {
                solidColorPipeline.Bind();

                solidColorPipeline.SetUniform(string.Empty, new UniTransforms()
                {
                    Model = m,
                    View = camera.View,
                    Projection = camera.Projection,
                    ObjectToWorld = m,
                    ObjectToClip = m * camera.View * camera.Projection,
                    WorldToObject = m.Invert()
                });

                solidColorPipeline.SetUniform(string.Empty, new UniParameters()
                {
                    Color = color
                });

                mesh.VertexAttributePointer((uint)solidColorPipeline.GetAttribLocation("In_Position"), 3, nameof(Vertex.Position));
                mesh.VertexAttributePointer((uint)solidColorPipeline.GetAttribLocation("In_Normal"), 3, nameof(Vertex.Normal));
                mesh.VertexAttributePointer((uint)solidColorPipeline.GetAttribLocation("In_Tangent"), 3, nameof(Vertex.Tangent));
                mesh.VertexAttributePointer((uint)solidColorPipeline.GetAttribLocation("In_Bitangent"), 3, nameof(Vertex.Bitangent));
                mesh.VertexAttributePointer((uint)solidColorPipeline.GetAttribLocation("In_Color"), 4, nameof(Vertex.Color));
                mesh.VertexAttributePointer((uint)solidColorPipeline.GetAttribLocation("In_TexCoord"), 2, nameof(Vertex.TexCoord));

                mesh.Draw();

                solidColorPipeline.Unbind();
            }
        }

        // Cube 1
        {
            Matrix4X4<float> m = model * Matrix4X4.CreateTranslation(new Vector3D<float>(-10.0f, 4.0f, -16.0f));

            foreach (Mesh mesh in cubeMeshes)
            {
                simplePipeline.Bind();

                simplePipeline.SetUniform(string.Empty, new UniTransforms()
                {
                    Model = m,
                    View = camera.View,
                    Projection = camera.Projection,
                    ObjectToWorld = m,
                    ObjectToClip = m * camera.View * camera.Projection,
                    WorldToObject = m.Invert()
                });

                simplePipeline.SetUniform(string.Empty, new UniParameters()
                {
                    Color = Vector4D<float>.One
                });

                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Position"), 3, nameof(Vertex.Position));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Normal"), 3, nameof(Vertex.Normal));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Tangent"), 3, nameof(Vertex.Tangent));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Bitangent"), 3, nameof(Vertex.Bitangent));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Color"), 4, nameof(Vertex.Color));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_TexCoord"), 2, nameof(Vertex.TexCoord));

                mesh.Draw();

                simplePipeline.Unbind();
            }
        }

        // Cube 2
        {
            Matrix4X4<float> m = model * Matrix4X4.CreateTranslation(new Vector3D<float>(8.0f, 0.0f, -10.0f));

            foreach (Mesh mesh in cubeMeshes)
            {
                simplePipeline.Bind();

                simplePipeline.SetUniform(string.Empty, new UniTransforms()
                {
                    Model = m,
                    View = camera.View,
                    Projection = camera.Projection,
                    ObjectToWorld = m,
                    ObjectToClip = m * camera.View * camera.Projection,
                    WorldToObject = m.Invert()
                });

                simplePipeline.SetUniform(string.Empty, new UniParameters()
                {
                    Color = Vector4D<float>.One
                });

                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Position"), 3, nameof(Vertex.Position));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Normal"), 3, nameof(Vertex.Normal));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Tangent"), 3, nameof(Vertex.Tangent));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Bitangent"), 3, nameof(Vertex.Bitangent));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_Color"), 4, nameof(Vertex.Color));
                mesh.VertexAttributePointer((uint)simplePipeline.GetAttribLocation("In_TexCoord"), 2, nameof(Vertex.TexCoord));

                mesh.Draw();

                simplePipeline.Unbind();
            }
        }
    }
}
