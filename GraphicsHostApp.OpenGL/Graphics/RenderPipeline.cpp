#include "RenderPipeline.h"

namespace GraphicsHostApp
{
	RenderPipeline::RenderPipeline(Shader vs, Shader fs)
	{
		m_handle = glCreateProgram();
		glAttachShader(m_handle, vs.Handle());
		glAttachShader(m_handle, fs.Handle());
		glLinkProgram(m_handle);

		// Check for linking errors.
		GLint length;
		glGetProgramiv(m_handle, GL_INFO_LOG_LENGTH, &length);
		GLchar* m_infoLog = new GLchar[length];
		glGetProgramInfoLog(m_handle, length * 2, nullptr, m_infoLog);

		if (length > 0)
		{
			glDeleteProgram(m_handle);

			throw std::runtime_error(m_infoLog);
		}
	}

	RenderPipeline::~RenderPipeline()
	{
		glDeleteProgram(m_handle);
	}

	GLint RenderPipeline::GetAttribLocation(std::string name)
	{
		return glGetAttribLocation(m_handle, name.c_str());
	}

	GLint RenderPipeline::GetUniformLocation(std::string name)
	{
		return glGetUniformLocation(m_handle, name.c_str());
	}

	void RenderPipeline::SetUniform(std::string name, GLint value)
	{
		glUniform1i(GetUniformLocation(name), value);
	}

	void RenderPipeline::SetUniform(std::string name, GLfloat value)
	{
		glUniform1f(GetUniformLocation(name), value);
	}

	void RenderPipeline::SetUniform(std::string name, glm::vec2 value)
	{
		glUniform2fv(GetUniformLocation(name), 1, &value[0]);
	}

	void RenderPipeline::SetUniform(std::string name, glm::vec3 value)
	{
		glUniform3fv(GetUniformLocation(name), 1, &value[0]);
	}

	void RenderPipeline::SetUniform(std::string name, glm::vec4 value)
	{
		glUniform4fv(GetUniformLocation(name), 1, &value[0]);
	}

	void RenderPipeline::SetUniform(std::string name, glm::mat2 value)
	{
		glUniformMatrix2fv(GetUniformLocation(name), 1, GL_FALSE, &value[0][0]);
	}

	void RenderPipeline::SetUniform(std::string name, glm::mat3 value)
	{
		glUniformMatrix3fv(GetUniformLocation(name), 1, GL_FALSE, &value[0][0]);
	}

	void RenderPipeline::SetUniform(std::string name, glm::mat4 value)
	{
		glUniformMatrix4fv(GetUniformLocation(name), 1, GL_FALSE, &value[0][0]);
	}

	void RenderPipeline::Bind()
	{
		glUseProgram(m_handle);

		glEnable(GL_DEPTH_TEST);
	}

	void RenderPipeline::Unbind()
	{
		glUseProgram(0);
	}
}
