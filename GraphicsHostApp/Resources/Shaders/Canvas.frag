#version 300 es

precision highp float;

in vec2 VS_UV;

layout(location = 0) out vec4 Out_Color;

uniform sampler2D Tex;

void main()
{
    Out_Color = texture(Tex, VS_UV);
}