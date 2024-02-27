#include "Camera.h"

namespace GraphicsHostApp
{
	void Camera::SetWidth(int width)
	{
		this->width = width;
	}

	void Camera::SetHeight(int height)
	{
		this->height = height;
	}

	void Camera::SetPosition(glm::vec3 position)
	{
		this->position = position;
	}

	void Camera::SetPitch(float pitch)
	{
		this->pitch = glm::radians(pitch);
		UpdateVectors();
	}

	void Camera::SetYaw(float yaw)
	{
		this->yaw = glm::radians(yaw);
		UpdateVectors();
	}

	void Camera::SetFov(float fov)
	{
		this->fov = glm::radians(fov);
	}

	void Camera::UpdateVectors()
	{
		front.x = cos(yaw) * cos(pitch);
		front.y = sin(pitch);
		front.z = sin(yaw) * cos(pitch);

		front = glm::normalize(front);

		right = glm::normalize(glm::cross(front, up));
		up = glm::normalize(glm::cross(right, front));
	}
}
