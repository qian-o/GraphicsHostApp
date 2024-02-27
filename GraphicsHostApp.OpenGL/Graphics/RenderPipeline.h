#pragma once

#include <glm/glm.hpp>

#include "GLObject.h"
#include "Shader.h"

namespace GraphicsHostApp
{
	class RenderPipeline : public GLObject
	{
	public:
		RenderPipeline(Shader vs, Shader fs);
		~RenderPipeline();

		GLint GetAttribLocation(std::string name);
		GLint GetUniformLocation(std::string name);
		void SetUniform(std::string name, GLint value);
		void SetUniform(std::string name, GLfloat value);
		void SetUniform(std::string name, glm::vec2 value);
		void SetUniform(std::string name, glm::vec3 value);
		void SetUniform(std::string name, glm::vec4 value);
		void SetUniform(std::string name, glm::mat2 value);
		void SetUniform(std::string name, glm::mat3 value);
		void SetUniform(std::string name, glm::mat4 value);
		void Bind();
		void Unbind();
	};
}
