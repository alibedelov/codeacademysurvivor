Shader "Custom/ColorTransitionWithTexture"
{
    Properties
    {
        _ColorCount ("Color Count", Range(2, 10)) = 2
        _Colors ("Colors", Color) = (1,1,1,1)
        _Speed ("Speed", Range(0.1, 10.0)) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
        _TextureScale ("Texture Scale", Range(0.1, 10.0)) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float2 uv2_TexCoords;
        };

        sampler2D _MainTex;
        float _ColorCount;
        float4 _Colors[10];
        float _Speed;
        float _TextureScale;

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 texColor = tex2D(_MainTex, IN.uv2_TexCoords * _TextureScale);
            fixed4 color = fixed4(0, 0, 0, 1);
            float t = _Time.y * _Speed;

            // Calculate color interpolation
            float index = floor(t);
            float blend = t - index;
            int startIndex = int(index) % int(_ColorCount);
            int endIndex = (startIndex + 1) % int(_ColorCount);

            fixed4 startColor = _Colors[startIndex];
            fixed4 endColor = _Colors[endIndex];

            color.rgb = lerp(startColor.rgb, endColor.rgb, blend);
            color.rgb *= texColor.rgb;

            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}