Shader "Skins/Shine" 
{
	Properties 
	{
		_AlbedoBase("Albedo Base", 2D) = "" {}
		_MetallicBase("Metallic Base", 2D) = "black" {}
		_EmissionBase("Emission Base", 2D) = "black" {}
		_AlbedoSkin("Albedo Skin", 2D) = "black" {}
		_MetallicSkin("Metallic Skin", 2D) = "black" {}
		_Shine("Shine", 2D) = "black" {}
		_ShineScale("Shine Wave Scale", Vector) = (0, 0, 0, 0)
		_ShineOffset("Shine Vertex Offset", Vector) = (0, 0, 0, 0)
	}

	SubShader 
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
		}

		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard vertex:vert
		#pragma target 3.0

		sampler2D _AlbedoBase;
		sampler2D _MetallicBase;
		sampler2D _EmissionBase;
		sampler2D _AlbedoSkin;
		sampler2D _MetallicSkin;
		sampler2D _Shine;
		fixed3 _ShineScale;
		fixed3 _ShineOffset;

		struct Input
		{
			float2 uv_AlbedoBase;
			fixed shine;
		};

		void vert(inout appdata_full v, out Input OUT)
		{
			UNITY_INITIALIZE_OUTPUT(Input, OUT);

			fixed3 vertex = v.vertex * _ShineOffset;
			fixed offset = vertex.x + vertex.y + vertex.z;
			OUT.shine = saturate(sin(offset + _Time.y * _ShineScale.x) * _ShineScale.y - _ShineScale.z);
		}

		void surf(Input IN, inout SurfaceOutputStandard OUT)
		{
			fixed4 albedoBase = tex2D(_AlbedoBase, IN.uv_AlbedoBase);
			fixed4 metallicBase = tex2D(_MetallicBase, IN.uv_AlbedoBase);
			fixed4 emissionBase = tex2D(_EmissionBase, IN.uv_AlbedoBase);

			fixed4 albedoSkin = tex2D(_AlbedoSkin, IN.uv_AlbedoBase);
			fixed4 metallicSkin = tex2D(_MetallicSkin, IN.uv_AlbedoBase);
			fixed4 shine = tex2D(_Shine, IN.uv_AlbedoBase);

			fixed4 albedo = albedoBase * albedoBase.a + lerp(albedoSkin, shine, IN.shine * shine.a) * (1.0 - albedoBase.a);
			fixed4 metallic = metallicBase * albedoBase.a + metallicSkin * (1.0 - albedoBase.a);

			OUT.Albedo = albedo.rgb;
			OUT.Alpha = albedo.a;

			OUT.Metallic = metallic.r;
			OUT.Smoothness = metallic.a;

			OUT.Emission = emissionBase.rgb;
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