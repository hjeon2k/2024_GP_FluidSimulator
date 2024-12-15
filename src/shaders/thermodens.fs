#version 330 core

out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D densTex;
uniform sampler2D tempTex;

// Input parameters
uniform float maxTemp = 1000.0;
uniform float minTemp = 300.0;
uniform float bMod = 1.0;
const float smConstant = 2.0;

void main()
{
    // Get density
    float dens = texture(densTex, TexCoord).x * smConstant;

    // Convert temperature to (0, 1)
    float temp = texture(tempTex, TexCoord).x;
    float t = (temp - minTemp) / (maxTemp - minTemp);
    t = pow(clamp(t, 0.0, 1.0), 0.5);

    // Return lerped color between blue and red
    //gl_FragColor = bMod * dens * vec4(t, 0.0, 1.0 - t, 1.0);
    FragColor = bMod * dens * vec4(t, 0.0, 1.0 - t, 1.0);
}
