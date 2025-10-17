// An unlit shader that renders backfaces with an HDR color
// and includes an offset to prevent Z-fighting.
Shader "Unlit/InvertedOutlineShader"
{
    Properties
    {
        [HDR] _Color ("Outline Color", Color) = (1, 1, 0, 1)
        
        // We can control the Z-offset from the material inspector.
        // A value of -1 is a good starting point.
        _Offset ("Z-Offset", Float) = -1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull Front

        Pass
        {
            // This tells the GPU to apply a depth offset to the polygons.
            // The first value is a factor based on polygon slope, the second is a fixed offset.
            // The property name must be in square brackets.
            Offset 1, [_Offset]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}

