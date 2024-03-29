﻿#pragma once

#include <iostream>
#include <glad/glad.h>
#include <glm/glm.hpp>

#include "Graphics/Renderer.h"

using namespace GraphicsHostApp;

#if WIN32
#define EXPORT extern "C" __declspec(dllexport)
#else
#define EXPORT extern "C"
#endif

EXPORT void MakeContext(GLADloadproc getProcAddress, long* id);

EXPORT void LoadScene(long id);

EXPORT void UpdateScene(long id, double deltaSeconds, const glm::vec2* size);

EXPORT void DrawScene(long id, double deltaSeconds);
