Shader "Custom/OutlineGlowShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1) // ���� �ܰ���
        _OutlineWidth ("Outline Width", Float) = 0.1 // �ܰ��� �β�
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Name "OUTLINE"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            // Add outline logic here
        }
    }
}
