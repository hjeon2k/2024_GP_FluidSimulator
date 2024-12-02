#version 330 core

out vec4 FragColor;

in vec2 TexCoord;

uniform sampler2D wallTex;

void main()
{
    // Return black
    //gl_FragColor = vec4(TexCoord, 0.0, 1.0);
    vec4 wallColor = texture(wallTex, TexCoord);
    gl_FragColor = wallColor;
}

