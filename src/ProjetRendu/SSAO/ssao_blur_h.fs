
#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D ssaoInput;

uniform float offset[3] = float[]( 0.0, 1.3846153846, 3.2307692308 );
uniform float weight[3] = float[]( 0.2270270270, 0.3162162162, 0.0702702703 );

void main() 
{
    FragColor = texture2D(ssaoInput, vec2(gl_FragCoord) / 1280.) * weight[0];
    for (int i=1; i<3; i++) {
        FragColor +=
            texture2D(ssaoInput, (vec2(gl_FragCoord) + vec2(offset[i], 0.0)) / 1280.0)
                * weight[i];
        FragColor +=
            texture2D(ssaoInput, (vec2(gl_FragCoord) - vec2(offset[i], 0.0)) / 1280.0)
                * weight[i];
    }
}
