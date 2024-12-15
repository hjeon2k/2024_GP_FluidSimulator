#version 330 core

out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D densTex;
uniform sampler2D tempTex;
uniform sampler2D wallTex;

// Input parameters
uniform float bMod = 1.0;
uniform float sMod = 1.0;

uniform vec2 lightPos[10];    // Position of the light in normalized device coordinates (-1 to 1)
uniform float lightIntensity; // Maximum brightness of the light
uniform float lightRadius;    // Radius where the light fully fades out


const float sbConstant = 0.000000000001;
const float minTemp = 300.0;
const float maxTemp = 6000.0;
const float smConstant = 2.0;

void main()
{
    // Read texutres
    float temp = texture(tempTex, TexCoord).x;
    float dens = texture(densTex, TexCoord).x * smConstant;
    vec3 wallColor = texture(wallTex, TexCoord).rgb;

    // Blackbody intensity
    float intensity = sbConstant * temp * temp * temp * temp / smConstant;

    // BLackbody color temperature
    float t = (temp - minTemp) / (maxTemp - minTemp);
    float red = 1.0;
    float green = (147.0 + (255.0 - 147.0) * t) / 255.0;
    float blue = (41.0 + (251.0 - 41.0) * t) / 255.0;

    // Final color as alpha mix of blackbody and density
    vec3 outColor = dens * bMod * vec3(sMod + red * intensity, sMod + green * intensity, sMod + blue * intensity);

    FragColor = vec4(outColor, 0.7);
    for (int i = 0; i < 10; ++i) {
        if (lightPos[i][0] == 0) continue;
        float distance = length(TexCoord - lightPos[i]);
        float attenuation = pow(clamp(1.0 - (distance / lightRadius), 0.1, 1.0), 2);
        vec3 lightEffect = (1 - dens) * lightIntensity * attenuation * wallColor;

        // gl_FragColor = vec4(outColor, 0.7) + vec4(lightEffect, 0.3);
        FragColor = FragColor + vec4(lightEffect, 0.3);
    }
}
