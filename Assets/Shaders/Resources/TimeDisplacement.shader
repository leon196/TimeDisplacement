Shader "Hidden/TimeDisplacement" {
	Properties 	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader 	{
		Cull Off ZWrite Off ZTest Always
		Pass 		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _SlitTexture;
			sampler2D _TimeTexture;
			float _Horizontal;
			float _Vertical;
			float _Mode;

			fixed4 frag (v2f i) : SV_Target {
				float2 uv = i.uv;
				uv.x = lerp(uv.x, 1.0 - uv.x, step(0.5, _Horizontal));
				uv.y = lerp(uv.y, 1.0 - uv.y, step(0.5, _Vertical));
				return lerp(tex2D(_SlitTexture, uv), tex2D(_TimeTexture, uv), step(0.5, _Mode));
			}
			ENDCG
		}
	}
}
