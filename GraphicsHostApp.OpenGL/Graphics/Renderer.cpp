#include "Renderer.h"

namespace GraphicsHostApp
{
#pragma region Simple Shader
	const std::string simpleVert = R"(#version 300 es

layout(location = 0) in vec3 In_Position;
layout(location = 1) in vec3 In_Normal;
layout(location = 2) in vec3 In_Tangent;
layout(location = 3) in vec3 In_Bitangent;
layout(location = 4) in vec4 In_Color;
layout(location = 5) in vec2 In_TexCoord;

out vec4 VS_Color;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform mat4 ObjectToWorld;
uniform mat4 ObjectToClip;
uniform mat4 WorldToObject;

void main()
{
    VS_Color = vec4(In_Normal * 0.5 + vec3(0.5), 1.0);

    gl_Position = ObjectToClip * vec4(In_Position, 1.0);
}
)";
	const std::string simpleFrag = R"(#version 300 es

precision highp float;

in vec4 VS_Color;

layout(location = 0) out vec4 Out_Color;

uniform vec4 Color;

void main()
{
    Out_Color = VS_Color * Color;
}
)";
#pragma endregion

#pragma region Solid Color Shader
	const std::string solidColorVert = R"(#version 300 es

layout(location = 0) in vec3 In_Position;
layout(location = 1) in vec3 In_Normal;
layout(location = 2) in vec3 In_Tangent;
layout(location = 3) in vec3 In_Bitangent;
layout(location = 4) in vec4 In_Color;
layout(location = 5) in vec2 In_TexCoord;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform mat4 ObjectToWorld;
uniform mat4 ObjectToClip;
uniform mat4 WorldToObject;

void main()
{
    gl_Position = ObjectToClip * vec4(In_Position, 1.0);
}
)";
	const std::string solidColorFrag = R"(#version 300 es

precision highp float;

layout(location = 0) out vec4 Out_Color;

uniform vec4 Color;

void main()
{
    Out_Color = Color;
}
)";
#pragma endregion

	static std::map<long, Renderer*> renderers;

	Renderer::Renderer(long id)
	{
		this->id = id;

		camera = new Camera();
		camera->SetPosition(glm::vec3(0.0f, 2.0f, 8.0f));
		camera->SetFov(45.0f);

		simplePipeline = nullptr;
		solidColorPipeline = nullptr;
	}

	void Renderer::MakeContext(GLADloadproc getProcAddress)
	{
		gladLoadGLES2Loader(getProcAddress);
	}

	void Renderer::LoadScene()
	{
		Shader vs1 = Shader(GL_VERTEX_SHADER, simpleVert);
		Shader fs1 = Shader(GL_FRAGMENT_SHADER, simpleFrag);
		simplePipeline = new RenderPipeline(vs1, fs1);

		Shader vs2 = Shader(GL_VERTEX_SHADER, solidColorVert);
		Shader fs2 = Shader(GL_FRAGMENT_SHADER, solidColorFrag);
		solidColorPipeline = new RenderPipeline(vs2, fs2);

		cubeMeshes = { MeshFactory::CreateCube() };
	}

	void Renderer::UpdateScene(double deltaSeconds, glm::vec2 size)
	{
		model = glm::rotate(glm::mat4(1.0f), (float)deltaSeconds, glm::vec3(0.0f, 1.0f, 0.0f));
		color = glm::mix(glm::vec4(1.0f), glm::vec4(1.0f, 0.0f, 0.0f, 1.0f), (float)sin(deltaSeconds));

		camera->SetWidth((int)size.x);
		camera->SetHeight((int)size.y);
	}

	void Renderer::DrawScene(double deltaSeconds)
	{
		glClearColor(0.2f, 0.2f, 0.2f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);

		// Cube
		{
			glm::mat4 m = model;

			for (Mesh* mesh : cubeMeshes)
			{
				simplePipeline->Bind();

				simplePipeline->SetUniform("Model", m);
				simplePipeline->SetUniform("View", camera->View());
				simplePipeline->SetUniform("Projection", camera->Projection());
				simplePipeline->SetUniform("ObjectToWorld", m);
				simplePipeline->SetUniform("ObjectToClip", camera->Projection() * camera->View() * m);
				simplePipeline->SetUniform("WorldToObject", glm::inverse(m));
				simplePipeline->SetUniform("Color", glm::vec4(1.0f));

				mesh->VertexAttributePointer(simplePipeline->GetAttribLocation("In_Position"), 3, "Position");
				mesh->VertexAttributePointer(simplePipeline->GetAttribLocation("In_Normal"), 3, "Normal");
				mesh->VertexAttributePointer(simplePipeline->GetAttribLocation("In_Tangent"), 3, "Tangent");
				mesh->VertexAttributePointer(simplePipeline->GetAttribLocation("In_Bitangent"), 3, "Bitangent");
				mesh->VertexAttributePointer(simplePipeline->GetAttribLocation("In_Color"), 4, "Color");
				mesh->VertexAttributePointer(simplePipeline->GetAttribLocation("In_TexCoord"), 2, "TexCoord");

				mesh->Draw();

				simplePipeline->Unbind();
			}
		}
	}

	Renderer* Renderer::GetInstance(long id)
	{
		if (id < 0)
		{
			id = (long)renderers.size();
		}

		if (renderers.find(id) == renderers.end())
		{
			renderers[id] = new Renderer(id);
		}

		return renderers[id];
	}
}
