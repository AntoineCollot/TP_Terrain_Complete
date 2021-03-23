Shader "Custom/Glass"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		
		//Frenel
		_FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
		_FresnelBias ("Fresnel Bias", Float) = 0
		_FresnelScale ("Fresnel Scale", Float) = 1
		_FresnelPower ("Fresnel Power", Float) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200
   
        CGPROGRAM
 
        #pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
			float fresnel;
        };
			
		//Frenel
		fixed4 _FresnelColor;
		fixed _FresnelBias;
		fixed _FresnelScale;
		fixed _FresnelPower;
		
		 void vert (inout appdata_full v, out Input o) {
          UNITY_INITIALIZE_OUTPUT(Input,o);
          
		  float3 i = normalize(ObjSpaceViewDir(v.vertex));
			o.fresnel = _FresnelBias + _FresnelScale * pow(1 + dot(i, v.normal), _FresnelPower);
      }

 
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
 
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 fresnelCol = saturate(lerp(fixed4(0,0,0,0), _FresnelColor, 1 - IN.fresnel));
            o.Albedo = _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a+ fresnelCol.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
 