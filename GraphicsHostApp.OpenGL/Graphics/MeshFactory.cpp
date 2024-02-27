#include "MeshFactory.h"

namespace GraphicsHostApp
{
	Mesh* MeshFactory::CreateTriangle(float size)
	{
		std::vector<Vertex> vertices = {
			Vertex(glm::vec3(-size, -size, 0.0f), glm::vec3(1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(size, -size, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f)),
			Vertex(glm::vec3(0.0f, size, 0.0f), glm::vec3(0.0f, 0.0f, 1.0f))
		};

		std::vector<GLuint> indices = {
			0, 1, 2
		};

		return new Mesh(vertices, indices);
	}

	Mesh* MeshFactory::CreateCube(float size)
	{
		std::vector<Vertex> vertices = {
			// Front face
			Vertex(glm::vec3(-size, -size, size), glm::vec3(0.0f, 0.0f, 1.0f)),
			Vertex(glm::vec3(size, -size, size), glm::vec3(0.0f, 0.0f, 1.0f)),
			Vertex(glm::vec3(size, size, size), glm::vec3(0.0f, 0.0f, 1.0f)),
			Vertex(glm::vec3(size, size, size), glm::vec3(0.0f, 0.0f, 1.0f)),
			Vertex(glm::vec3(-size, size, size), glm::vec3(0.0f, 0.0f, 1.0f)),
			Vertex(glm::vec3(-size, -size, size), glm::vec3(0.0f, 0.0f, 1.0f)),

			// Back face
			Vertex(glm::vec3(-size, -size, -size), glm::vec3(0.0f, 0.0f, -1.0f)),
			Vertex(glm::vec3(-size, size, -size), glm::vec3(0.0f, 0.0f, -1.0f)),
			Vertex(glm::vec3(size, size, -size), glm::vec3(0.0f, 0.0f, -1.0f)),
			Vertex(glm::vec3(size, size, -size), glm::vec3(0.0f, 0.0f, -1.0f)),
			Vertex(glm::vec3(size, -size, -size), glm::vec3(0.0f, 0.0f, -1.0f)),
			Vertex(glm::vec3(-size, -size, -size), glm::vec3(0.0f, 0.0f, -1.0f)),

			// Top face
			Vertex(glm::vec3(-size, size, -size), glm::vec3(0.0f, 1.0f, 0.0f)),
			Vertex(glm::vec3(-size, size, size), glm::vec3(0.0f, 1.0f, 0.0f)),
			Vertex(glm::vec3(size, size, size), glm::vec3(0.0f, 1.0f, 0.0f)),
			Vertex(glm::vec3(size, size, size), glm::vec3(0.0f, 1.0f, 0.0f)),
			Vertex(glm::vec3(size, size, -size), glm::vec3(0.0f, 1.0f, 0.0f)),
			Vertex(glm::vec3(-size, size, -size), glm::vec3(0.0f, 1.0f, 0.0f)),

			// Bottom face
			Vertex(glm::vec3(-size, -size, -size), glm::vec3(0.0f, -1.0f, 0.0f)),
			Vertex(glm::vec3(size, -size, -size), glm::vec3(0.0f, -1.0f, 0.0f)),
			Vertex(glm::vec3(size, -size, size), glm::vec3(0.0f, -1.0f, 0.0f)),
			Vertex(glm::vec3(size, -size, size), glm::vec3(0.0f, -1.0f, 0.0f)),
			Vertex(glm::vec3(-size, -size, size), glm::vec3(0.0f, -1.0f, 0.0f)),
			Vertex(glm::vec3(-size, -size, -size), glm::vec3(0.0f, -1.0f, 0.0f)),

			// Right face
			Vertex(glm::vec3(size, -size, -size), glm::vec3(1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(size, size, -size), glm::vec3(1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(size, size, size), glm::vec3(1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(size, size, size), glm::vec3(1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(size, -size, size), glm::vec3(1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(size, -size, -size), glm::vec3(1.0f, 0.0f, 0.0f)),

			// Left face
			Vertex(glm::vec3(-size, -size, -size), glm::vec3(-1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(-size, -size, size), glm::vec3(-1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(-size, size, size), glm::vec3(-1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(-size, size, size), glm::vec3(-1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(-size, size, -size), glm::vec3(-1.0f, 0.0f, 0.0f)),
			Vertex(glm::vec3(-size, -size, -size), glm::vec3(-1.0f, 0.0f, 0.0f))
		};

		std::vector<GLuint> indices = {
			0, 1, 2, 3, 4, 5, // Front face
			6, 7, 8, 9, 10, 11, // Back face
			12, 13, 14, 15, 16, 17, // Top face
			18, 19, 20, 21, 22, 23, // Bottom face
			24, 25, 26, 27, 28, 29, // Right face
			30, 31, 32, 33, 34, 35 // Left face
		};

		return new Mesh(vertices, indices);
	}
}
