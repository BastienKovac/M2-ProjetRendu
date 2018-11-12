#version 330 core
uniform sampler2D ssaoInput;

out vec4 FragColor;

in vec2 TexCoords;

uniform int width;
uniform int height;

float normpdf(in float x, in float sigma)
{
	return 0.39894 * exp(-0.5 * x * x / (sigma * sigma)) / sigma;
}

void main(void)
{
    //declare stuff
	const int mSize = 7;
	const int kSize = (mSize - 1) / 2;
	float kernel[mSize];
	vec3 final_colour = vec3(0.0);
	
	//create the 1-D kernel
	float sigma = 1.0;
	float Z = 0.0;
	for (int j = 0; j <= kSize; ++j)
	{
		kernel[kSize + j] = kernel[kSize - j] = normpdf(float(j), sigma);
	}
		
	//get the normalization factor (as the gaussian has been clamped)
	for (int j = 0; j < mSize; ++j)
	{
		Z += kernel[j];
	}
	
	vec2 texelSize = 1.0 / vec2(textureSize(ssaoInput, 0));
	//read out the texels
	for (int i = - kSize; i <= kSize; ++i)
	{
		for (int j = - kSize; j <= kSize; ++j)
		{
            vec2 offset = vec2(float(i), float(j)) * texelSize;
			final_colour += kernel[kSize + j] * kernel[kSize + i] * texture(ssaoInput, TexCoords + offset).rgb;
		}
	}
	
	
	FragColor = vec4(final_colour / (Z * Z), 1.0);
}
