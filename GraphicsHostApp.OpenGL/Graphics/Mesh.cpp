#include "Mesh.h"

namespace GraphicsHostApp
{
	static std::map<std::string, ptrdiff_t> offsetMap;

	Mesh::Mesh(std::vector<Vertex> vertices, std::vector<GLuint> indices)
	{
		if (offsetMap.empty())
		{
			offsetMap["Position"] = offsetof(Vertex, Position);
			offsetMap["Normal"] = offsetof(Vertex, Normal);
			offsetMap["Tangent"] = offsetof(Vertex, Tangent);
			offsetMap["Bitangent"] = offsetof(Vertex, Bitangent);
			offsetMap["Color"] = offsetof(Vertex, Color);
			offsetMap["TexCoord"] = offsetof(Vertex, TexCoord);
		}

		glGenVertexArrays(1, &m_handle);
		glBindVertexArray(m_handle);

		glGenBuffers(1, &m_vbo);
		glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
		glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), vertices.data(), GL_STATIC_DRAW);

		glGenBuffers(1, &m_ebo);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_ebo);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size() * sizeof(GLuint), indices.data(), GL_STATIC_DRAW);

		m_indexLength = (GLuint)indices.size();

		glBindVertexArray(0);
	}

	Mesh::~Mesh()
	{
		glDeleteBuffers(1, &m_vbo);
		glDeleteBuffers(1, &m_ebo);
		glDeleteVertexArrays(1, &m_handle);
	}

	void Mesh::VertexAttributePointer(GLuint index, GLint size, std::string fieldName)
	{
		if (offsetMap.find(fieldName) == offsetMap.end())
		{
			throw std::runtime_error("Field name not found in vertex struct.");
		}

		glBindVertexArray(m_handle);

		glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
		glVertexAttribPointer(index, size, GL_FLOAT, GL_FALSE, sizeof(Vertex), (void*)offsetMap[fieldName]);
		glEnableVertexAttribArray(index);
		glBindBuffer(GL_ARRAY_BUFFER, 0);

		glBindVertexArray(0);
	}

	void Mesh::Draw()
	{
		glBindVertexArray(m_handle);
		glDrawElements(GL_TRIANGLES, m_indexLength, GL_UNSIGNED_INT, nullptr);
		glBindVertexArray(0);
	}
}
