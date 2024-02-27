#include "GraphicsHostApp.OpenGL.h"

void MakeContext(GLADloadproc getProcAddress, long* id)
{
	Renderer* renderer = Renderer::GetInstance();
	renderer->MakeContext(getProcAddress);

	*id = renderer->Id();
}

void LoadScene(long id)
{
	Renderer::GetInstance(id)->LoadScene();
}

void UpdateScene(long id, double deltaSeconds, glm::vec2 size)
{
	Renderer::GetInstance(id)->UpdateScene(deltaSeconds, size);
}

void DrawScene(long id, double deltaSeconds)
{
	Renderer::GetInstance(id)->DrawScene(deltaSeconds);
}
