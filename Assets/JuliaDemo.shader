Shader "JuliaDemo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _C("", Vector)=(0, 0, 0, 0)
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float2 _C;
            float2 _P;
            float _S;
            float2 _uvScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float Julia (float2 pos)
            {
                int count = 200;
                pos = pos;
                
                float2 ret;
                for(int i = 0; i < count; i++)
                {
                    ret.x =  pos.x * pos.x - pos.y * pos.y + _C.x;
                    ret.y = pos.x * pos.y * 2 + _C.x;
                    
                    if (length(ret) > 2)
                    {
                        return ((i+1) / (float)count) / 2 + 0.5; 
                    }
                    
                    pos = ret;
                }
                return 0;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 pos = i.uv;
                pos = ((pos * 2 - float2(1, 1)) * _S - _P) * _uvScale;;
                
                float c = Julia(pos);
                fixed4 col;
                
                col = fixed4(1, 1, 1, 1) * c;
                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}
