Shader "Unlit/Tail"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FoamTex ("浮沫贴图", 2D) = "white" {} 
        _Scale("大小",float) = 1
        _Speed("流速",float) = 1
        _Color_Foam("水花颜色",color) = (1.0,1.0,1.0,1.0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent"}
        LOD 100

        Pass
        {
            ZWrite Off ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD3;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _FoamTex;
            float4 _FoamTex_ST;
            float _Scale;
            float _Speed;
            float4 _Color_Foam;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = (v.uv + frac(_Time.x*_Speed))*_Scale;
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed3 col = tex2D(_MainTex, i.uv).rgb;
                half3 foamTex = tex2D(_FoamTex,i.uv1).rgb;

                col *= 3.33;
                col = clamp(col,0,7.49);
                col = sin(col);
                col.rgb *= foamTex;
                col = step(0.5,col)*i.color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return half4(col,i.color.a);
            }
            ENDCG
        }
    }
}
