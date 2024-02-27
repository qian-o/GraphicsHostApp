#pragma once

#include <cmath>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

namespace GraphicsHostApp
{
	class Camera
	{
	public:
		int Width() const { return width; }
		int Height() const { return height; }
		glm::vec3 Position() const { return position; }
		glm::vec3 Front() const { return front; }
		glm::vec3 Up() const { return up; }
		glm::vec3 Right() const { return right; }
		float Pitch() const { return glm::degrees(pitch); }
		float Yaw() const { return glm::degrees(yaw); }
		float Fov() const { return glm::degrees(fov); }
		glm::mat4 View() const { return glm::lookAt(position, position + front, up); }
		glm::mat4 Projection() const { return glm::perspective(fov, (float)width / (float)height, 0.1f, 1000.0f); }

		void SetWidth(int width);
		void SetHeight(int height);
		void SetPosition(glm::vec3 position);
		void SetPitch(float pitch);
		void SetYaw(float yaw);
		void SetFov(float fov);

	private:
		int width = 800;
		int height = 600;
		glm::vec3 position = glm::vec3(0.0f, 0.0f, 0.0f);
		glm::vec3 front = -glm::vec3(0.0f, 0.0f, 1.0f);
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f);
		glm::vec3 right = glm::vec3(1.0f, 0.0f, 0.0f);
		float pitch = 0.0f;
		float yaw = -glm::radians(90.0f);
		float fov = glm::radians(90.0f);

		void UpdateVectors();
	};
}
