// Matrix
float4x4 World;
float4x4 View;
float4x4 Projection;

// Light related
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.01;

float3 LightPosition1 = float3(10, 0, 0);
float4 LightColor1 = float4(1, 1, 1, 1);

float3 LightPosition2 = float3(2, 1, -9);
float4 LightColor2 = float4(1, 0.5f, 0, 1);

float3 LightPosition3 = float3(11, 3, 21.5);
float4 LightColor3 = float4(0.8f, 0.8f, 1, 1);


float DiffuseIntensity = 1;

float Shininess = 200;
float SpecularIntensity = 1;
float3 CameraPosition;

// Texture
texture ModelTexture;
sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

// The input for the VertexShader
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL;
	float2 TextureCoordinate : TEXCOORD0;
};

// The output from the vertex shader, used for later processing
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 PosOut : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float2 TextureCoordinate : TEXCOORD2;
};

// The VertexShader.
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float3 normal = normalize(mul(input.Normal, World));
	output.Normal = normal;
	output.PosOut = worldPosition;

	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 DiffuseFunction(VertexShaderOutput input, float3 light, float3 normal, float4 color, float dist)
{
	float diffuseIntensity = (1 - dist) * DiffuseIntensity;
	if (diffuseIntensity < 0) diffuseIntensity = 0;

	float diffuse = saturate(dot(light, normal));

	return diffuseIntensity*color*diffuse;
}

float4 SpecularFunction(VertexShaderOutput input, float3 light, float3 normal, float4 color, float dist)
{
	float specularIntensity = (1 - dist) * SpecularIntensity;
	if (specularIntensity < 0) specularIntensity = 0;

	float3 v = normalize(input.PosOut - CameraPosition);
	float3 h = normalize(-v + light);

	float dotProduct = saturate(dot(h, normal));
	return specularIntensity * color * max(pow(dotProduct, Shininess), 0);
}

// The Pixel Shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;
	float3 light1 = normalize(LightPosition1 - input.PosOut);
	float3 light2 = normalize(LightPosition2 - input.PosOut);
	float3 light3 = normalize(LightPosition3 - input.PosOut);
	float3 normal = normalize(input.Normal);
	float dist1 = pow(distance(input.PosOut, LightPosition1), 2) / 70;
	float dist2 = pow(distance(input.PosOut, LightPosition2), 2) / 300;
	float dist3 = pow(distance(input.PosOut, LightPosition3), 2) / 50;

	float4 diffuseEfect = 0;
	float4 specular = 0;

	//specular += SpecularFunction(input, light2, normal, LightColor2, dist2);

	if (dist1 != 0)
	{
		diffuseEfect += DiffuseFunction(input, light1, normal, LightColor1, dist1);
		specular += SpecularFunction(input, light1, normal, LightColor1, dist1);
	}

	if (input.PosOut.z > -10.5f && input.PosOut.x > -8 && input.PosOut.x < 12 && dist2 != 0) diffuseEfect += DiffuseFunction(input, light2, normal, LightColor2, dist2);

	if (dist3 != 0 && input.PosOut.z > 16.5)
	{
		diffuseEfect += DiffuseFunction(input, light3, normal, LightColor3, dist3);
		specular += SpecularFunction(input, light3, normal, LightColor3, dist3);
	}

	if (diffuseEfect.x < 0.01 && diffuseEfect.y < 0.01 && diffuseEfect.z < 0.01)
		return saturate(AmbientColor*AmbientIntensity + textureColor * float4(0.01, 0.01, 0.01, 0.01) + specular);
	else
		return saturate(AmbientColor*AmbientIntensity + textureColor * diffuseEfect + specular);
}

// Our Techinique
technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
