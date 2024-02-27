#pragma once

#include <iostream>
#include <vector>
#include <map>
#include <glad/glad.h>

namespace GraphicsHostApp
{
	class GLObject
	{
	public:
		GLuint Handle() const { return m_handle; }

	protected:
		GLuint m_handle;
	};
}