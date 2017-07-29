// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skybox/Space"
{
    Properties
    {
        _Noise1 ("Noise 1", Color) = (0.1, 0.1, 0.1, 1)
        _Noise2 ("Noise 2", Color) = (0, 0, 0, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    float4 _Noise1;
    float4 _Noise2;
    
    v2f vert (appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos (v.position);
        o.texcoord = v.texcoord;
        return o;
    }

	float triz(float x) {
		return 1 - abs(x % 2 - 1);
	}

	float r(float3 seed)
	{
		return frac(sin(dot(seed.xyz, float3(12.9898, 178.233, 45.5432))) * cos(dot(seed.xyz, float3(122.9898, 8.233, 45.5432))) * 0.5 * ((triz(_Time.y*1)*1) + 1));
	}
    
    float4 frag (v2f i) : COLOR
    {
		float angleYX = atan2(i.texcoord.y, i.texcoord.x);
		float angleXZ = atan2(i.texcoord.x, i.texcoord.z);
		float angleZY = atan2(i.texcoord.z, i.texcoord.y);
		float v = (sin(_Time.x*20) + 1) / 2;
		float4 finalColor = float4(v, 0, 0, 1);
		finalColor = lerp(_Noise1, _Noise2, r(i.texcoord));
        return finalColor;//float4(angleYX, angleXZ, angleZY, 1);
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    } 
}