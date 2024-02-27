#pragma once

#include "GLObject.h"

namespace GraphicsHostApp
{
	class Shader : public GLObject
	{
	public:
		Shader(GLenum shaderType, std::string source);
		~Shader();
	};
}