Shader "Unlit/Cartoon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmbientCol("暗部颜色",color) = (1.0,1.0,1.0,1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; 
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 nDirWS : TEXCOORD1;
                float4 posWS : TEXCOORD2;
                
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AmbientCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.posWS = mul(unity_ObjectToWorld,v.vertex);
                o.nDirWS = UnityObjectToWorldNormal(v.normal.xyz);
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //向量
                half3 nDirWS = normalize(i.nDirWS);
                half3 lDirWS = normalize(_WorldSpaceLightPos0.xyz);
                half3 vDirWS  =normalize(_WorldSpaceCameraPos.xyz - i.posWS.xyz);
                //贴图数据
                half3 col = tex2D(_MainTex,i.uv).rgb;
                //中间量
                half lambert = max(dot(nDirWS,lDirWS),0);
                lambert = step(0.2,lambert);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                half3 finalRGB = col *lerp(half3(1,1,1),_AmbientCol,1-lambert);
                return half4(finalRGB,1.0);
            }
            ENDCG
        }
    }
}
