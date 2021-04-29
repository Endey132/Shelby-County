Shader "Skins/Pattern" 
{
	Properties 
	{
		_ColorBase("Color Base", Color) = (1,1,1,1)
		_AlbedoBase("Albedo Base", 2D) = "" {}
		_ColorSkin("Color Skin", Color) = (1,1,1,1)
		_AlbedoSkin("Albedo Skin", 2D) = "white" {}
		_MetallicBase("Metallic Base", 2D) = "black" {}
		_MetallicSkin("Metallic Skin", 2D) = "black" {}
		_NormalBase("Normal Base", 2D) = "bump" {}
		_NormalSkin("Normal Skin", 2D) = "bump" {}
		_EmissionBase("Emission Base", 2D) = "black" {}
		_EmissionSkin("Emission Skin", 2D) = "black" {}
	}

	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
		}

		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard
		#pragma target 3.0

		fixed4 _ColorBase;
		sampler2D _AlbedoBase;
		fixed4 _ColorSkin;
		sampler2D _AlbedoSkin;
		sampler2D _MetallicBase;
		sampler2D _MetallicSkin;
		sampler2D _NormalBase;
		sampler2D _NormalSkin;
		sampler2D _EmissionBase;
		sampler2D _EmissionSkin;

		struct Input
		{
			float2 uv_AlbedoBase;
			float2 uv_AlbedoSkin;
		};

		void surf(Input IN, inout SurfaceOutputStandard OUT)
		{
			fixed4 albedoBase = tex2D(_AlbedoBase, IN.uv_AlbedoBase);
			fixed4 albedoSkin = tex2D(_AlbedoSkin, IN.uv_AlbedoSkin);

			fixed4 metallicBase = tex2D(_MetallicBase, IN.uv_AlbedoBase);
			fixed4 metallicSkin = tex2D(_MetallicSkin, IN.uv_AlbedoSkin);

			fixed4 normalBase = tex2D(_NormalBase, IN.uv_AlbedoBase);
			fixed4 normalSkin = tex2D(_NormalSkin, IN.uv_AlbedoSkin);

			fixed4 emissionBase = tex2D(_EmissionBase, IN.uv_AlbedoBase);
			fixed4 emissionSkin = tex2D(_EmissionSkin, IN.uv_AlbedoSkin);

			fixed4 albedo = albedoBase * _ColorBase * albedoBase.a + albedoSkin * _ColorSkin * (1.0 - albedoBase.a);
			fixed4 metallic = metallicBase * albedoBase.a + metallicSkin * (1.0 - albedoBase.a);
			fixed4 normal = normalBase * albedoBase.a + normalSkin * (1.0 - albedoBase.a);
			fixed4 emission = emissionBase * albedoBase.a + emissionSkin * (1.0 - albedoBase.a);

			OUT.Albedo = albedo.rgb;
			OUT.Alpha = albedo.a;

			OUT.Metallic = metallic.r;
			OUT.Normal = UnpackNormal(normal);
			OUT.Smoothness = metallic.a;

			OUT.Emission = emission.rgb;
		}

		ENDCG
	} 

	FallBack "Diffuse"
}

//Shader "Skins/Standard"
//{
//	Properties
//	{
//		_Color("Color", Color) = (1,1,1,1)
//		_AlbedoBase("Albedo Base", 2D) = "white" {}
	//_AlbedoSkin("Albedo Skin", 2D) = "white" {}
//	//_MetallicBase("Metallic Base", 2D) = "black" {}
//	//_MetallicSkin("Metallic Skin", 2D) = "black" {}
//	//_EmissionBase("Emission Base", 2D) = "black" {}
//	//_EmissionSkin("Emission Skin", 2D) = "black" {}
//	}
//
//		SubShader
//	{
//		Tags
//	{
//		"RenderType" = "Opaque"
//	}
//
//		LOD 200
//
//		CGPROGRAM
//
//#pragma surface surf Standard fullforwardshadows
//#pragma target 3.0
//
//		fixed4 _Color;
//	sampler2D _AlbedoBase;
//	//sampler2D _AlbedoSkin;
//	//sampler2D _MetallicBase;
//	//sampler2D _MetallicSkin;
//	//sampler2D _EmissionBase;
//	//sampler2D _EmissionSkin;
//
//	struct Input
//	{
//		float2 uv_AlbedoBase;
//		//float2 uv_AlbedoSkin;
//		//float2 uv_MetallicBase;
//		//float2 uv_MetallicSkin;
//		//float2 uv_EmissionBase;
//		//float2 uv_EmissionSkin;
//	};
//
//	void surf(Input IN, inout SurfaceOutputStandard OUT)
//	{
//		fixed4 albedoBase = tex2D(_AlbedoBase, IN.uv_AlbedoBase);
//		//fixed4 albedoSkin = tex2D(_AlbedoSkin, IN.uv_AlbedoSkin);
//
//		//fixed4 metallicBase = tex2D(_MetallicBase, IN.uv_MetallicBase);
//		//fixed4 metallicSkin = tex2D(_MetallicSkin, IN.uv_MetallicSkin);
//
//		//fixed4 emissionBase = tex2D(_EmissionBase, IN.uv_EmissionBase);
//		//fixed4 emissionSkin = tex2D(_EmissionSkin, IN.uv_EmissionSkin);
//
//		fixed4 albedo = albedoBase;
//		//fixed4 albedo = albedoBase * albedoBase.a + albedoSkin * (1.0 - albedoBase.a);
//		//fixed4 metallic = metallicBase * albedoBase.a + metallicSkin * (1.0 - albedoBase.a);
//		//fixed4 emission = emissionBase * albedoBase.a + emissionSkin * (1.0 - albedoBase.a);
//
//		OUT.Albedo = albedo.rgb * _Color.rgb;
//		OUT.Alpha = albedo.a * _Color.a;
//
//		//OUT.Metallic = metallic.r;
//		//OUT.Smoothness = metallic.a;
//
//		//OUT.Emission = emission.rgb;
//	}
//
//	ENDCG
//	}
//
//		FallBack "Diffuse"
//}