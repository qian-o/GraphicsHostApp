#pragma once

#include <iostream>
#include <vector>
#include <glad/glad.h>
#include <glm/glm.hpp>

#include "Camera.h"
#include "RenderPipeline.h"
#include "Shader.h"
#include "Mesh.h"
#include "MeshFactory.h"

namespace GraphicsHostApp
{
	class Renderer
	{
	public:
		Renderer(long id);

		long Id() const { return id; }

		void MakeContext(GLADloadproc getProcAddress);
		void LoadScene();
		void UpdateScene(double deltaSeconds, glm::vec2 size);
		void DrawScene(double deltaSeconds);

		static Renderer* GetInstance(long id = -1);

	private:
		long id;
		Camera* camera;
		RenderPipeline* simplePipeline;
		RenderPipeline* solidColorPipeline;
		std::vector<Mesh*> cubeMeshes = {};
		glm::mat4 model = glm::mat4(1.0f);
		glm::vec4 color = glm::vec4(1.0f, 0.0f, 0.0f, 1.0f);
	};
}