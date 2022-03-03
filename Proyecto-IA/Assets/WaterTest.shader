Shader "Custom/WaterTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_WaterTex ("Water Texture", 2D) = "cyan" {}
		_WaterMap("Water Map (B&W)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _WaterTex;
        sampler2D _WaterMap;
		int4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            
			float2 uv_WaterTex;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed4 map = tex2D(_WaterMap, IN.uv_MainTex);
			fixed4 land = tex2D(_MainTex, IN.uv_MainTex); //Land texture
			land.a = map.a;

			fixed4 water = map * tex2D(_WaterTex, IN.uv_WaterTex)*_Color;
			water.a = map;

			fixed4 col = land * water;

            o.Albedo = land;
			o.Emission = water;

            o.Alpha = map.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
