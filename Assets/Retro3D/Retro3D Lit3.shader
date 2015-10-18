Shader "Retro3D/Lit3"
{
    Properties
    {
        _MainTex("Base", 2D) = "white" {}
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1)
        _GeoRes("Geometric Resolution", Float) = 40
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #pragma multi_compile_fwdbase 
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 texcoord : TEXCOORD0;
                float4 positionWorld : TEXCOORD1;
                float3 normalDirection : TEXCOORD2;
                float3 vertexLighting : TEXCOORD3;
            };

            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
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

                float4x4 modelMatrix = _Object2World;
                float4x4 modelMatrixInverse = _World2Object;

                o.positionWorld = mul(modelMatrix, v.vertex);
                o.normalDirection = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);

                o.vertexLighting = float3(0.0, 0.0, 0.0);
                #ifdef VERTEXLIGHT_ON
                for (int index = 0; index < 4; index++)
                {
                    float4 lightPosition = float4(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index], 1.0);

                    float3 vertexToLightSource = lightPosition.xyz - o.positionWorld.xyz;
                    float3 lightDirection = normalize(vertexToLightSource);
                    float squaredDistance = dot(vertexToLightSource, vertexToLightSource);
                    float attenuation = 1.0 / (1.0 + unity_4LightAtten0[index] * squaredDistance);
                    float3 diffuseReflection = attenuation * unity_LightColor[index].rgb * _Color.rgb * max(0.0, dot(o.normalDirection, lightDirection)); 

                    o.vertexLighting = o.vertexLighting + diffuseReflection;
                }
                #endif

                return o;
            }

            float4 frag(v2f input) : SV_Target
            {
                float3 normalDirection = normalize(input.normalDirection); 
                float3 viewDirection = normalize(_WorldSpaceCameraPos - input.positionWorld.xyz);
                float3 lightDirection;
                float attenuation;

                if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                {
                   attenuation = 1.0; // no attenuation
                   lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                } 
                else // point or spot light
                {
                   float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.positionWorld.xyz;
                   float distance = length(vertexToLightSource);
                   attenuation = 1.0 / distance; // linear attenuation 
                   lightDirection = normalize(vertexToLightSource);
                }
 
                float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
 
                float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

                float2 uv = input.texcoord.xy / input.texcoord.z;
                return tex2D(_MainTex, uv) * float4(input.vertexLighting + ambientLighting + diffuseReflection, 1.0) * 2;
            }

            ENDCG
        }

        Pass
        {
            Tags { "LightMode" = "ForwardAdd" }

            // pass for additional light sources
            Blend One One // additive blending 
 
            CGPROGRAM
 
            #pragma vertex vert  
            #pragma fragment frag 
 
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
 
            // User-specified properties
            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GeoRes;
 
            struct vertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
            };
            struct vertexOutput {
                float4 position : SV_POSITION;
                float3 texcoord : TEXCOORD0;
                float4 positionWorld : TEXCOORD1;
                float3 normalDirection : TEXCOORD2;
            };
 
            vertexOutput vert(vertexInput input) 
            {
                vertexOutput output;
 
                float4 wp = mul(UNITY_MATRIX_MV, input.vertex);
                wp.xyz = floor(wp.xyz * _GeoRes) / _GeoRes;

                float4 sp = mul(UNITY_MATRIX_P, wp);
                output.position = sp;

                float2 uv = TRANSFORM_TEX(input.texcoord, _MainTex);
                output.texcoord = float3(uv * sp.w, sp.w);

                float4x4 modelMatrix = _Object2World;
                float4x4 modelMatrixInverse = _World2Object; 
 
                output.positionWorld = mul(modelMatrix, input.vertex);
                output.normalDirection = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);

                return output;
            }
 
            float4 frag(vertexOutput input) : SV_Target
            {
                float3 normalDirection = normalize(input.normalDirection);
 
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - input.positionWorld.xyz);
                float3 lightDirection;
                float attenuation;
 
                if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                {
                    attenuation = 1.0; // no attenuation
                    lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                } 
                else // point or spot light
                {
                    float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.positionWorld.xyz;
                    float distance = length(vertexToLightSource);
                    attenuation = 1.0 / distance; // linear attenuation 
                    lightDirection = normalize(vertexToLightSource);
                }
 
                float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));
 
                float2 uv = input.texcoord.xy / input.texcoord.z;
                return tex2D(_MainTex, uv) * float4(diffuseReflection, 1.0);
            }
 
            ENDCG
        }
    }
}
