Shader "Custom/ShaderHolePunch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HoleCenter ("Hole Center", Vector) = (0, 0, 0, 0)
        _HoleRadius ("Hole Radius", Float) = 0.3
        _HoleFeather ("Hole Feather", Float) = 0.2
        _OverlayColor ("Overlay Color", Color) = (0, 0, 0, 1)
        _OverlayAlpha ("Overlay Alpha", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
                float4 worldPos : WORLD_POS;
            };

            sampler2D _MainTex;
            float4 _HoleCenter;
            float _HoleRadius;
            float _HoleFeather;
            float4 _OverlayColor;
            float _OverlayAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 holeCenterWorld = _HoleCenter.xy;
                float2 currentWorldPos = i.worldPos.xy;
                float distanceFromCenter = distance(currentWorldPos, holeCenterWorld);

                float alpha = _OverlayAlpha;
                if (distanceFromCenter < _HoleRadius)
                {
                    alpha = 0.0;
                }
                else if (distanceFromCenter < _HoleRadius + _HoleFeather)
                {
                    alpha *= (distanceFromCenter - _HoleRadius) / _HoleFeather;
                }

                float4 finalColor = _OverlayColor;
                finalColor.a *= alpha;

                return finalColor;
            }
            ENDCG
        }
    }
}