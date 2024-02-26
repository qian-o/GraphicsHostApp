#version 300 es

precision highp float;

layout(location = 0) out vec4 Out_Color;

uniform vec4 Color;

void main()
{
    Out_Color = Color;
}