//==============================================================================================
//Author: Uncle Song
//Create Date: 2022-06-21
//Description: 不被遮挡总是显示
//----------------------------------------------------------------------------------------------
//Alter History:
//              2022-06-08  add Codes.
//============================================================================================== 
Shader "SongShaderDemo/AlwaysShow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	  _Clip("剔除：",Range(1,0.01)) = 0.01
    }
    SubShader
    {
        Tags  { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
      	ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			fixed4 _Color= fixed4(0, 0, 0, 0);
	         fixed _Clip;
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
			    clip(i.uv.x);
				clip(i.uv.y);
				if (i.uv.x>1|| i.uv.y>1)
				{
					clip(i.uv.x-1);
					clip(i.uv.y - 1);
				}
				return col;
			
                
            }
            ENDCG
        }
    }
}
