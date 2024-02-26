#version 300 es

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