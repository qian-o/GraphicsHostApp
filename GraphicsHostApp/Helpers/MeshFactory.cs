using System;
using System.Collections.Generic;
using System.Linq;
using GraphicsHostApp.Graphics;
using Silk.NET.Assimp;
using Silk.NET.Maths;

namespace GraphicsHostApp.Helpers;

public static unsafe class MeshFactory
{
    public static void GetCube(out Vertex[] vertices, out uint[] indices, float size = 0.5f)
    {
        vertices =
        [
            // Front face
            new(new(-size, -size, size), new(0.0f, 0.0f, 1.0f), texCoord: new(0.0f, 0.0f)),
            new(new(size, -size, size), new(0.0f, 0.0f, 1.0f), texCoord: new(1.0f, 0.0f)),
            new(new(size, size, size), new(0.0f, 0.0f, 1.0f), texCoord: new(1.0f, 1.0f)),
            new(new(size, size, size), new(0.0f, 0.0f, 1.0f), texCoord: new(1.0f, 1.0f)),
            new(new(-size, size, size), new(0.0f, 0.0f, 1.0f), texCoord: new(0.0f, 1.0f)),
            new(new(-size, -size, size), new(0.0f, 0.0f, 1.0f), texCoord: new(0.0f, 0.0f)),

            // Back face
            new(new(-size, -size, -size), new(0.0f, 0.0f, -1.0f), texCoord: new(1.0f, 0.0f)),
            new(new(-size, size, -size), new(0.0f, 0.0f, -1.0f), texCoord: new(1.0f, 1.0f)),
            new(new(size, size, -size), new(0.0f, 0.0f, -1.0f), texCoord: new(0.0f, 1.0f)),
            new(new(size, size, -size), new(0.0f, 0.0f, -1.0f), texCoord: new(0.0f, 1.0f)),
            new(new(size, -size, -size), new(0.0f, 0.0f, -1.0f), texCoord: new(0.0f, 0.0f)),
            new(new(-size, -size, -size), new(0.0f, 0.0f, -1.0f), texCoord: new(1.0f, 0.0f)),

            // Top face
            new(new(-size, size, -size), new(0.0f, 1.0f, 0.0f), texCoord: new(0.0f, 1.0f)),
            new(new(-size, size, size), new(0.0f, 1.0f, 0.0f), texCoord: new(0.0f, 0.0f)),
            new(new(size, size, size), new(0.0f, 1.0f, 0.0f), texCoord: new(1.0f, 0.0f)),
            new(new(size, size, size), new(0.0f, 1.0f, 0.0f), texCoord: new(1.0f, 0.0f)),
            new(new(size, size, -size), new(0.0f, 1.0f, 0.0f), texCoord: new(1.0f, 1.0f)),
            new(new(-size, size, -size), new(0.0f, 1.0f, 0.0f), texCoord: new(0.0f, 1.0f)),

            // Bottom face
            new(new(-size, -size, -size), new(0.0f, -1.0f, 0.0f), texCoord: new(0.0f, 0.0f)),
            new(new(size, -size, -size), new(0.0f, -1.0f, 0.0f), texCoord: new(1.0f, 0.0f)),
            new(new(size, -size, size), new(0.0f, -1.0f, 0.0f), texCoord: new(1.0f, 1.0f)),
            new(new(size, -size, size), new(0.0f, -1.0f, 0.0f), texCoord: new(1.0f, 1.0f)),
            new(new(-size, -size, size), new(0.0f, -1.0f, 0.0f), texCoord: new(0.0f, 1.0f)),
            new(new(-size, -size, -size), new(0.0f, -1.0f, 0.0f), texCoord: new(0.0f, 0.0f)),

            // Right face
            new(new(size, -size, -size), new(1.0f, 0.0f, 0.0f), texCoord: new(1.0f, 0.0f)),
            new(new(size, size, -size), new(1.0f, 0.0f, 0.0f), texCoord: new(1.0f, 1.0f)),
            new(new(size, size, size), new(1.0f, 0.0f, 0.0f), texCoord: new(0.0f, 1.0f)),
            new(new(size, size, size), new(1.0f, 0.0f, 0.0f), texCoord: new(0.0f, 1.0f)),
            new(new(size, -size, size), new(1.0f, 0.0f, 0.0f), texCoord: new(0.0f, 0.0f)),
            new(new(size, -size, -size), new(1.0f, 0.0f, 0.0f), texCoord: new(1.0f, 0.0f)),

            // Left face
            new(new(-size, -size, -size), new(-1.0f, 0.0f, 0.0f), texCoord: new(0.0f, 0.0f)),
            new(new(-size, -size, size), new(-1.0f, 0.0f, 0.0f), texCoord: new(1.0f, 0.0f)),
            new(new(-size, size, size), new(-1.0f, 0.0f, 0.0f), texCoord: new(1.0f, 1.0f)),
            new(new(-size, size, size), new(-1.0f, 0.0f, 0.0f), texCoord: new(1.0f, 1.0f)),
            new(new(-size, size, -size), new(-1.0f, 0.0f, 0.0f), texCoord: new(0.0f, 1.0f)),
            new(new(-size, -size, -size), new(-1.0f, 0.0f, 0.0f), texCoord: new(0.0f, 0.0f))
        ];

        indices = vertices.Select((a, b) => (uint)b).ToArray();
    }

    public static void AssimpParsing(string file, out Vertex[] vertices, out uint[] indices)
    {
        const PostProcessSteps steps = PostProcessSteps.CalculateTangentSpace
                                       | PostProcessSteps.Triangulate
                                       | PostProcessSteps.GenerateNormals
                                       | PostProcessSteps.GenerateSmoothNormals
                                       | PostProcessSteps.GenerateUVCoords
                                       | PostProcessSteps.FlipUVs
                                       | PostProcessSteps.PreTransformVertices;

        using Assimp importer = Assimp.GetApi();
        Scene* scene = importer.ImportFile(file, (uint)steps);

        if (scene == null)
        {
            throw new Exception($"Assimp parsing failed. Error: {importer.GetErrorStringS()}");
        }

        List<Vertex> t1 = [];
        List<uint> t2 = [];

        ProcessNode(scene->MRootNode);

        vertices = [.. t1];
        indices = [.. t2];

        void ProcessNode(Node* node)
        {
            for (uint i = 0; i < node->MNumMeshes; i++)
            {
                Mesh* mesh = scene->MMeshes[node->MMeshes[i]];

                ProcessMesh(mesh, out Vertex[] t3, out uint[] t4);

                t1.AddRange(t3);
                t2.AddRange(t4);
            }

            for (uint i = 0; i < node->MNumChildren; i++)
            {
                ProcessNode(node->MChildren[i]);
            }
        }

        void ProcessMesh(Mesh* mesh, out Vertex[] t3, out uint[] t4)
        {
            t3 = new Vertex[mesh->MNumVertices];

            for (uint i = 0; i < mesh->MNumVertices; i++)
            {
                t3[i].Position = (*&mesh->MVertices[i]).ToGeneric();
                t3[i].Normal = (*&mesh->MNormals[i]).ToGeneric();
                t3[i].Tangent = (*&mesh->MTangents[i]).ToGeneric();
                t3[i].Bitangent = (*&mesh->MBitangents[i]).ToGeneric();

                if (mesh->MColors[0] != null)
                {
                    t3[i].Color = (*&mesh->MColors[0][i]).ToGeneric();
                }

                if (mesh->MTextureCoords[0] != null)
                {
                    Vector3D<float> texCoord = (*&mesh->MTextureCoords[0][i]).ToGeneric();

                    t3[i].TexCoord = new Vector2D<float>(texCoord.X, texCoord.Y);
                }
            }

            t4 = new uint[mesh->MNumFaces * 3];

            for (uint i = 0; i < mesh->MNumFaces; i++)
            {
                Face face = mesh->MFaces[i];

                for (uint j = 0; j < face.MNumIndices; j++)
                {
                    t4[i * 3 + j] = face.MIndices[j];
                }
            }
        }
    }
}
