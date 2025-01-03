Shader "Custom/BackgroundPulse"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _BassIntensity ("Bass Intensity", Float) = 0.0
        _IsBeat ("Is Beat", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _BaseColor;
        float _BassIntensity;
        float _IsBeat;

        void surf(Input IN, inout SurfaceOutput o)
        {
            float pulse = sin(_BassIntensity * 10.0) * 0.5 + 0.5;
            if (_IsBeat > 0.5)
                o.Albedo = _BaseColor.rgb * pulse * 2.0;
            else
                o.Albedo = _BaseColor.rgb * pulse;
        }
        ENDCG
    }
}
