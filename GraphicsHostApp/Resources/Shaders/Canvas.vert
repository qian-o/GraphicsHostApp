#version 300 es

layout(location = 0) in vec3 In_Position;
layout(location = 1) in vec3 In_Normal;
layout(location = 2) in vec3 In_Tangent;
layout(location = 3) in vec3 In_Bitangent;
layout(location = 4) in vec4 In_Color;
layout(location = 5) in vec2 In_TexCoord;

out vec2 VS_UV;

void main()
{
    VS_UV = In_TexCoord;

    gl_Position = vec4(In_Position, 1.0);
}