#version 300 es

precision highp float;

in vec4 VS_Color;

layout(location = 0) out vec4 Out_Color;

uniform vec4 Color;

void main()
{
    Out_Color = VS_Color * Color;
}