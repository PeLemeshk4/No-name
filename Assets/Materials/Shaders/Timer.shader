Shader "Custom/Timer"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 0
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UnityUI"
        }

        Blend SrcAlpha OneMinusSrcAlpha // Убрать, если не нужна прозрачность

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float _FillAmount;
            fixed4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _MainColor; // Применяем цвет
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Рассчитываем угол (0..1) по часовой стрелке
                float2 center = float2(0.5, 0.5);
                float2 dir = i.uv - center;

                float angle = atan2(dir.y, dir.x) - 3.1415926535 * 0.5;
                if (angle < 0) angle += 2 * 3.1415926535;
                angle = angle / (2 * 3.1415926535);

                // Если угол вне заданного диапазона, отбрасываем пиксель
                clip(angle > _FillAmount ? 1 : -1);

                // Возвращаем цвет текстуры + умноженный на _MainColor
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * i.color;
            }
            ENDCG
        }
    }
}
