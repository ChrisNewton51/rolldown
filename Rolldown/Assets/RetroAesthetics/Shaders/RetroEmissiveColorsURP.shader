Shader "Universal Render Pipeline/RetroAesthetics/RetroEmissiveColorsURP" {
    Properties {
        _MainColor ("Main color", Color) = (0.1, 0.1, 0.1, 1)
        _LineColor ("Line color", Color) = (0.1, 1, 0.1, 1)
        _EmissionColor ("Emission color", Color) = (1, 1, 1, 1)
        _EmissionGain ("Emission gain", Range(0, 1)) = 0.5
        _GridTex("Grid texture", 2D) = "white" {}
        [Toggle] _UseSpecular ("Use Shininess", float) = 0
        _Specular ("Shininess", Range(1, 1000)) = 100
        _ShadowColor ("Shadow color", Color) = (0, 0, 0, 1)
    }

    SubShader {
        Tags { "RenderPipeline"="UniversalRenderPipeline" }

        // Surface shader function
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM
            
            ENDCG
        }

        Pass {
            Name "SURFACE"
            Tags {"LightMode" = "UniversalForward"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // Structs
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
            };

            // Properties
            float4 _ShadowColor;
            float _UseSpecular;

            // Input
            float4 _LineColor;
            float4 _MainColor;
            sampler2D _GridTex;
            float4 _EmissionColor;
            float _EmissionGain;
            float _Specular;

            // Vertex shader
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // Surface shader
            half4 frag(v2f i) : SV_Target {
                // Fetch texture color
                float3 t = tex2D(_GridTex, i.uv).rgb;
                float val = saturate(1 - (t.r + t.g + t.b));
                
                // Compute albedo color
                float4 albedo = lerp(_LineColor, _MainColor, val);

                // Emission
                float3 emission = t * exp(_EmissionGain * 5.0f) * _EmissionColor.rgb;

                // Specular
                float3 viewDir = normalize(UnityWorldSpaceViewDir(i.vertex));
                float3 normal = normalize(i.normal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.vertex.xyz);
                float3 h = normalize(lightDir + viewDir);
                float diff = max(0, dot(normal, lightDir));
                float nh = max(0, dot(normal, h));
                float spec = pow(nh, _Specular) * _UseSpecular; 
                
                // Final color calculation
                float3 finalColor = (albedo.rgb * _LightColor0.rgb * diff) + (spec * _LightColor0.rgb);
                finalColor += _ShadowColor.rgb * (1.0 - _LightColor0.a);

                // Output
                half4 finalOutput = half4(finalColor + emission, 1.0);
                return finalOutput;
            }
            ENDCG
        }
    }
}
