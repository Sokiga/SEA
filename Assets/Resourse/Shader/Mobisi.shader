Shader "Hidden/Mobisi"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutLineCol ("Color",color) = (1.0,1.0,1.0,1.0)
        _OutLineWide("Wide",vector) = (1.0,0.01,0.0,0.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            half4 _OutLineCol;
            half4 _OutLineWide;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 right = i.uv.xy + float2(_MainTex_TexelSize.x*_OutLineWide.x, 0.0);
                float2 left = i.uv.xy + float2(-_MainTex_TexelSize.x*_OutLineWide.x,0.0);
                float2 up = i.uv.xy + float2(0.0,_MainTex_TexelSize.y*_OutLineWide.x) ;
                float2 down = i.uv.xy + float2(0.0, - _MainTex_TexelSize.y*_OutLineWide.x) ;

                float depthL = tex2D(_MainTex,  left);
                float depthR = tex2D(_MainTex,  right);
                float depthUp = tex2D(_MainTex,  up);
                float depthDown = tex2D(_MainTex,  down);

                float depthDiff = sqrt(pow((depthL - depthR)* _OutLineWide.z, 2) + pow((depthUp - depthDown)* _OutLineWide.z, 2))
                                    >_OutLineWide.y?1.0:0.0;
                half3 finalCol = lerp(col.rgb,_OutLineCol,depthDiff);
                return float4(finalCol,1);
            }
            ENDCG
        }
    }
}
