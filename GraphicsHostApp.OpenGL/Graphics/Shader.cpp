#include "Shader.h"

namespace GraphicsHostApp
{
	Shader::Shader(GLenum shaderType, std::string source)
	{
		const char* c_source = source.c_str();

		m_handle = glCreateShader(shaderType);
		glShaderSource(m_handle, 1, &c_source, nullptr);
		glCompileShader(m_handle);

		// Check for shader compile errors.
		GLint length;
		glGetShaderiv(m_handle, GL_INFO_LOG_LENGTH, &length);
		GLchar* m_infoLog = new GLchar[length];
		glGetShaderInfoLog(m_handle, length * 2, nullptr, m_infoLog);

		if (length > 0)
		{
			glDeleteShader(m_handle);

			throw std::runtime_error(m_infoLog);
		}
	}

	Shader::~Shader()
	{
		glDeleteShader(m_handle);
	}
}
