Shader "Retro3D/Lit"
{
    Properties
    {
        _MainTex("Base", 2D) = "white" {}
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1)
        _GeoRes("Geometric Resolution", Float) = 40
    }
    SubShader
    {
        //Tags { "RenderType" = "Opaque" }
        //ZWrite On Lighting On Cull Off Fog { Mode Off } Blend One Zero

        //GrabPass { "_GrabTexture" }

        /*CGPROGRAM
		#pragma surface surf Lambert

		struct Input
        {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutput o)
        {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		}

		ENDCG*/

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float4 color : COLOR0;
                float3 texcoord : TEXCOORD;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _GeoRes;

            v2f vert(appdata_base v)
            {
                v2f o;

                float4 wp = mul(UNITY_MATRIX_MV, v.vertex);
                wp.xyz = floor(wp.xyz * _GeoRes) / _GeoRes;

                float4 sp = mul(UNITY_MATRIX_P, wp);
                o.position = sp;

                float2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.texcoord = float3(uv * sp.w, sp.w);

                float4x4 modelMatrixInverse = _World2Object;
                float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
                float3 lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));

                float3 diffuseReflection = float3(_LightColor0.xyz) * max(0.0, dot(normalDirection, lightDirection));

                o.color = float4(diffuseReflection, 1.0) + UNITY_LIGHTMODEL_AMBIENT;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.texcoord.xy / i.texcoord.z;
                return tex2D(_MainTex, uv) * /*_Color*/i.color * 2;
            }

            ENDCG
        }
    }
}
