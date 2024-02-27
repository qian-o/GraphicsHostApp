#pragma once

#include <glm/glm.hpp>

namespace GraphicsHostApp
{
	struct Vertex
	{
		glm::vec3 Position;
		glm::vec3 Normal;
		glm::vec3 Tangent;
		glm::vec3 Bitangent;
		glm::vec4 Color;
		glm::vec2 TexCoord;

		Vertex() = default;

		Vertex(const glm::vec3& position, const glm::vec3& normal = glm::vec3(0.0f), const glm::vec3& tangent = glm::vec3(0.0f), const glm::vec3& bitangent = glm::vec3(0.0f), const glm::vec4& color = glm::vec4(1.f), const glm::vec2& texCoord = glm::vec2(0.0f)) : Position(position), Normal(normal), Tangent(tangent), Bitangent(bitangent), Color(color), TexCoord(texCoord)
		{
		}
	};
}