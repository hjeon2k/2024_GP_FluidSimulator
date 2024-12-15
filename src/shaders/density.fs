#version 330 core

out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D densTex;
uniform sampler2D tempTex;

// Input parameters
uniform float bMod = 1.0;
const float smConstant = 2.0;

void main()
{
    // Return grayscale density, scaled by brightness modifier
    float texPoint = bMod * texture(densTex, TexCoord).x * smConstant;
    //gl_FragColor = vec4(texPoint, texPoint, texPoint, 1.0);
    FragColor = vec4(texPoint, texPoint, texPoint, 1.0);
}
