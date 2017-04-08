// Matrix
float4x4 World;
float4x4 View;
float4x4 Projection;

// Light related
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.01;

float3 LightPosition1 = float3(10, 0, 0);
float4 LightColor1 = float4(1, 1, 1, 1);

float3 LightPosition2 = float3(0, 1, 5);
float4 LightColor2 = float4(1, 1, 1, 1);


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

	float3 ref = normalize(reflect(light, normal));
	float3 v = normalize(input.PosOut - CameraPosition);

	float dotProduct = saturate(dot(ref, v));
	return specularIntensity * color * max(pow(dotProduct, Shininess), 0);
}

// The Pixel Shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;
	float3 light1 = normalize(LightPosition1 - input.PosOut);
	float3 normal = normalize(input.Normal);
	float dist = pow(distance(input.PosOut, LightPosition1), 2) / 50;

	float4 diffuseEfect = DiffuseFunction(input, light1, normal, LightColor1, dist);
	float4 specular = SpecularFunction(input, light1, normal, LightColor1, dist);

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
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}


//// Matrix
//float4x4 World;
//float4x4 View;
//float4x4 Projection;
//
//// Light related
//float4 AmbientColor = float4(1, 1, 1, 1);
//float AmbientIntensity = 0.01;
//
//float3 LightPosition1 = float3(10, 0, 0);
//float4 LightColor1 = float4(1, 1, 1, 1);
//
//
//
//
//float DiffuseIntensity = 1;
//
//float Shininess = 200;
//float SpecularIntensity = 1;
//float3 CameraPosition;
//
//// Texture
//texture ModelTexture;
//sampler2D textureSampler = sampler_state {
//	Texture = (ModelTexture);
//	MagFilter = Linear;
//	MinFilter = Linear;
//	AddressU = Clamp;
//	AddressV = Clamp;
//};
//
//// The input for the VertexShader
//struct VertexShaderInput
//{
//	float4 Position : POSITION0;
//	float3 Normal : NORMAL;
//	float2 TextureCoordinate : TEXCOORD0;
//};
//
//// The output from the vertex shader, used for later processing
//struct VertexShaderOutput
//{
//	float4 Position : POSITION0;
//	float3 PosOut : TEXCOORD0;
//	float3 Normal : TEXCOORD1;
//	float2 TextureCoordinate : TEXCOORD2;
//};
//
//// The VertexShader.
//VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
//{
//	VertexShaderOutput output;
//
//	float4 worldPosition = mul(input.Position, World);
//	float4 viewPosition = mul(worldPosition, View);
//	output.Position = mul(viewPosition, Projection);
//
//	float3 normal = normalize(mul(input.Normal, World));
//	output.Normal = normal;
//	output.PosOut = worldPosition;
//
//	output.TextureCoordinate = input.TextureCoordinate;
//
//	return output;
//}
//
//float4 DiffuseFunction(VertexShaderOutput input, float3 LightPosition, float4 DiffuseColor)
//{
//	float diffuseIntensity = (1 - pow(distance(input.PosOut, LightPosition), 2) / 50) * DiffuseIntensity;
//	if (diffuseIntensity < 0) diffuseIntensity = 0;
//
//	float3 light = normalize(LightPosition - input.PosOut);
//	float3 normal = normalize(input.Normal);
//	float diffuse = saturate(dot(light, normal));
//
//	return diffuseIntensity*DiffuseColor*diffuse;
//}
//
//float4 SpecularFunction(VertexShaderOutput input, float3 LightPosition, float4 SpecularColor)
//{
//	float specularIntensity = (1 - pow(distance(input.PosOut, LightPosition), 2) / 50) * SpecularIntensity;
//	if (specularIntensity < 0) specularIntensity = 0;
//
//	float3 normal = normalize(input.Normal);
//	float3 light = normalize(LightPosition - input.PosOut);
//	float3 ref = normalize(reflect(light, normal));
//	float3 v = normalize(input.PosOut - CameraPosition);
//
//	float dotProduct = saturate(dot(ref, v));
//	return specularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0);
//}
//
//// The Pixel Shader
//float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
//{
//	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
//	textureColor.a = 1;
//
//	float4 diffuseEfect = DiffuseFunction(input, LightPosition1, LightColor1);
//	float4 specular = SpecularFunction(input, LightPosition1, LightColor1);
//
//	if (diffuseEfect.x < 0.01 && diffuseEfect.y < 0.01 && diffuseEfect.z < 0.01)
//		return saturate(AmbientColor*AmbientIntensity + textureColor * float4(0.01, 0.01, 0.01, 0.01) + specular);
//	else
//		return saturate(AmbientColor*AmbientIntensity + textureColor * diffuseEfect + specular);
//}
//
//// Our Techinique
//technique Technique1
//{
//	pass Pass1
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction();
//	}
//}



//float4x4 World;
//float4x4 View;
//float4x4 Projection;
//
//// Ambient color
//float4 AmbientColor = float4(1, 1, 1, 1);
//float AmbientIntensity = 0.1;
//
//// Diffuse color
//float4x4 WorldInverseTranspose;
//
//float3 DiffuseLightDirection = float3(1, 0, 0);
//float4 DiffuseColor = float4(1, 1, 1, 1);
//float DiffuseIntensity = 1.0;
//
//// Spectular color
//float Shininess = 200;
//float4 SpecularColor = float4(1, 1, 1, 1); // color light
//float SpecularIntensity = 1;
//float3 ViewVector = float3(1, 0, 0);
//// TODO: add effect parameters here.
//
//struct VertexShaderInput
//{
//	float4 Position : POSITION0;
//	float4 Normal : NORMAL0;
//};
//
//struct VertexShaderOutput
//{
//	float4 Position : POSITION0;
//	float4 Color : COLOR0;
//	float3 Normal : TEXCOORD0;
//};
//
//VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
//{
//	VertexShaderOutput output;
//
//	float4 worldPosition = mul(input.Position, World);
//	float4 viewPosition = mul(worldPosition, View);
//	output.Position = mul(viewPosition, Projection);
//
//	float4 normal = normalize(mul(input.Normal, WorldInverseTranspose));
//	float lightIntensity = dot(normal, DiffuseLightDirection);
//	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
//
//	output.Normal = normal;
//
//	return output;
//}
//
//float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
//{
//	float3 light = normalize(DiffuseLightDirection);
//	float3 normal = normalize(input.Normal);
//	float3 r = normalize(2 * dot(light, normal) * normal - light);
//	float3 v = normalize(mul(normalize(ViewVector), World));
//
//	float dotProduct = dot(r, v);
//	float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0) * length(input.Color);
//
//	return saturate(input.Color + AmbientColor * AmbientIntensity + specular);
//}
//
//technique Spectular
//{
//	pass Pass1
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction();
//	}
//}

//float4x4 World;
//float4x4 View;
//float4x4 Projection;
//
//// Ambient color
//float4 AmbientColor = float4(1, 1, 1, 1);
//float AmbientIntensity = 0.1;
//
//// Diffuse color
//float4x4 WorldInverseTranspose;
//
//float3 LightPosition = float3(1, 0, 0);
//float4 DiffuseColor = float4(1, 1, 1, 1);
//float DiffuseIntensity = 1.0;
//
//// Spectular color
//float Shininess = 20;
//float4 SpecularColor = float4(1, 1, 1, 1); // color light
//float SpecularIntensity = 1;
//float3 ViewVector = float3(1, 0, 0);
//// TODO: add effect parameters here.
//
//struct VertexShaderInput
//{
//	float4 Position : POSITION0;
//	float3 Normal : NORMAL0;
//};
//
//struct VertexShaderOutput
//{
//	float4 Position : POSITION0;
//	float3 Normal : TEXCOORD0;
//};
//
//VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
//{
//	VertexShaderOutput output;
//
//	float4 worldPosition = mul(input.Position, World);
//	float4 viewPosition = mul(worldPosition, View);
//	output.Position = mul(viewPosition, Projection);
//
//	float3 normal = normalize(mul(input.Normal, World));
//	output.Normal = normal;
//
//	return output;
//}
//
//float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
//{
//	float3 light = normalize(LightPosition - input.Position);
//	float3 normal = normalize(input.Normal);
//	float diffuse = dot(normal, light);
//
//	float3 r = normalize(2 * dot(light, normal) * normal - light);
//	float3 v = normalize(mul(normalize(ViewVector), World));
//
//	float dotProduct = dot(r, v);
//	float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0);
//
//	return saturate(DiffuseIntensity*DiffuseColor*diffuse + AmbientColor * AmbientIntensity + specular);
//}
//
//technique Spectular
//{
//	pass Pass1
//	{
//		VertexShader = compile vs_2_0 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction();
//	}
//}



//struct VertexShaderInput
//{
//    float4 Position : POSITION0;
//
//    // TODO: add input channels such as texture
//    // coordinates and vertex colors here.
//};
//
//struct VertexShaderOutput
//{
//    float4 Position : POSITION0;
//
//    // TODO: add vertex shader outputs such as colors and texture
//    // coordinates here. These values will automatically be interpolated
//    // over the triangle, and provided as input to your pixel shader.
//};
//
//VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
//{
//    VertexShaderOutput output;
//
//    float4 worldPosition = mul(input.Position, World);
//    float4 viewPosition = mul(worldPosition, View);
//    output.Position = mul(viewPosition, Projection);
//
//    // TODO: add your vertex shader code here.
//
//    return output;
//}
//
//float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
//{
//    // TODO: add your pixel shader code here.
//
//    return float4(1, 0, 0, 1);
//}
//
//technique Technique1
//{
//    pass Pass1
//    {
//        // TODO: set renderstates here.
//
//        VertexShader = compile vs_2_0 VertexShaderFunction();
//        PixelShader = compile ps_2_0 PixelShaderFunction();
//    }
//}
