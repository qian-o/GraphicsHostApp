#pragma once

#include "GLObject.h"
#include "Vertex.h"

namespace GraphicsHostApp
{
	class Mesh : public GLObject
	{
	public:
		Mesh(std::vector<Vertex> vertices, std::vector<GLuint> indices);
		~Mesh();

		GLuint ArrayBuffer() const { return m_vbo; }
		GLuint ElementBuffer() const { return m_ebo; }
		GLuint IndexLength() const { return m_indexLength; }

		void VertexAttributePointer(GLuint index, GLint size, std::string fieldName);
		void Draw();

	private:
		GLuint m_vbo;
		GLuint m_ebo;
		GLuint m_indexLength;
	};
}