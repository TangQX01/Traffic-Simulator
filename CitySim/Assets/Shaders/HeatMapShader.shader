﻿Shader "UChart/HeatMap/Simple"
{
	Properties
	{
		_HeatMapTex("Texture",2D) = "white"{}
		_Alpha("Alpha",range(0,1)) = 0.8 
	}

	SubShader
	{
		Tags {"RenderType"="Overlay" "Queue"="Transparent" } 
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest [unity_GUIZTestMode]
		ZWrite On
		// Cull off

		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _HeatMapTex;
			half _Alpha;
			uniform int _FactorCount = 0;
			uniform float3 _Factors[1500];
			uniform float3 _FactorsProperties[1500];

			struct a2v
			{
				float4 pos : POSITION;
			};

			struct v2f
			{
				float4 pos : POSITION;
				fixed3 worldPos : TEXCOORD1;
			};

			v2f vert(a2v input)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(input.pos);
				o.worldPos = mul(unity_ObjectToWorld,input.pos).xyz;
				return o;
			}

			// half4 frag(v2f input):COLOR
			// {
			// 	half heat = 0;
			// 	for( int i = 0 ; i < _FactorCount;i++ )
			// 	{
			// 		half dis = distance(input.worldPos,_Factors[i].xyz);
			// 		float radius = _FactorsProperties[i].x;
			// 		float intensity = _FactorsProperties[i].y;
			// 		float temperatureFacator = _FactorsProperties[i].z;
			// 		half ratio = 1 - saturate(dis/radius);
			// 		heat += intensity * ratio * temperatureFacator;
			// 		heat = clamp(heat,0.05,0.95);
			// 	}
			// 	half4 color = tex2D(_HeatMapTex,fixed2(heat,0.5));
			// 	color.a = _Alpha;
			// 	return color;
			// }

			half4 frag(v2f input):COLOR
			{
				half heat = 0;
				for( int i = 0 ; i < _FactorCount; i++ )
				{
					half dis = distance(input.worldPos,_Factors[i].xyz);
					float radius = _FactorsProperties[i].x;
					float intensity = _FactorsProperties[i].y;
					float temperatureFacator = _FactorsProperties[i].z;
					half ratio = 1 - saturate(dis/radius);
					heat += intensity * ratio * temperatureFacator;
					heat = clamp(heat,0.05,0.95);
				}

				// 如果heat值为0，返回完全透明色
				if(heat <= 0.05) // 这里的0.05是因为我们已经对heat进行了clamp操作
				{
					return half4(0,0,0,0);
				}
				
				half4 color = tex2D(_HeatMapTex,fixed2(heat,0.5));
				color.a = _Alpha;
				return color;
			}

			ENDCG
		}
	}

	 FallBack "Diffuse"
}