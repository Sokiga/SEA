Shader "Unlit/Water"
{
    Properties
    {
        
        _Distance("深度",float) = 3
       
        _Color_01("水面颜色1",color) = (1.0,1.0,1.0,1.0)
        _Color_02("水面颜色2",color) = (1.0,1.0,1.0,1.0)
        _FoamTex ("浮沫贴图", 2D) = "white" {} 
        _Scale("大小",float) = 1
        _Speed("流速",float) = 1
        _Color_Foam("水花颜色",color) = (1.0,1.0,1.0,1.0)
        _SpecInt("高光强度",float) = 5
        _SpecPow("高光次幂",float) = 10
        _Smoothness("光滑度",float) = 10
        _FoamAmount("浮沫数量",float) = 3
        _FoamCutoff("浮沫裁切",float) = 1

    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
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
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 nDirWS : TEXCOORD2;
                float4 posWS : TEXCOORD3;
                float4 screenPos : TEXCOORD4;
            };

            sampler2D _FoamTex;
            float4 _FoamTex_ST;
            float _Scale;
            float _Speed;
            float _Distance;
            float4 _Color_Foam;
            float4 _SpecPow;
            float4 _SpecInt;
            float _Smoothness;
            float4 _Color_01;
            float4 _Color_02;
            sampler2D _CameraDepthTexture;
            float _FoamAmount;
            float _FoamCutoff;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posWS = mul(unity_ObjectToWorld,v.vertex);
                o.nDirWS = UnityObjectToWorldNormal(v.normal.xyz);
                o.screenPos = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.uv = (v.uv + frac(_Time.x*_Speed))*_Scale;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half3 nDirWS = normalize(i.nDirWS);
                half3 lDirWS = normalize(_WorldSpaceLightPos0.xyz);
                half3 vDirWS  =normalize(_WorldSpaceCameraPos.xyz - i.posWS.xyz);
                half3 hDirWS = normalize(lDirWS+vDirWS);
                //贴图数据
                half3 foamTex = tex2D(_FoamTex,i.uv).rgb;
                //中间量
                half lambert = max(dot(nDirWS,lDirWS),0);
                half ndoth = dot(nDirWS,hDirWS);
                half ndotv = dot(nDirWS,vDirWS);

                
                
                float depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);
                float waterDepth = i.screenPos.w;    //水面深度
                float depthDifference = depth - waterDepth;   

                float waterDepthDifference01 = saturate(depthDifference / _Distance);
                float4 col = lerp(_Color_01, _Color_02, waterDepthDifference01);

                float foam = saturate((depth-i.screenPos.w)/_FoamAmount)*_FoamCutoff;
                foam = step(foam,foamTex);
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                half3 spec = pow(ndoth,_SpecPow);
                half3 specCol = lerp(col.rgb,_Color_Foam,_Smoothness);
                half3 finalSpec = specCol *_SpecInt * spec;

                half3 finalCol =lerp(col.rgb,_Color_Foam,foam);
                return half4(finalCol,col.a);
            }
            ENDCG
        }
    }
}
