// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CustomSprite/Planet"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_SphereNormal("SphereNormal", 2D) = "white" {}
		_TimeScaleRotation("TimeScaleRotation", Float) = 0
		_BMultiplier("BMultiplier", Float) = 1
		_TilingFix("TilingFix", Vector) = (0,0,0,0)
		_OffsetFix("OffsetFix", Vector) = (0,0,0,0)
		_LightAngle("LightAngle", Float) = 0
		_LightAmount("LightAmount", Float) = 0
		_LightExp("LightExp", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _TextureSample2;
			uniform float2 _TilingFix;
			uniform sampler2D _SphereNormal;
			uniform float4 _SphereNormal_ST;
			uniform float _BMultiplier;
			uniform float _TimeScaleRotation;
			uniform float2 _OffsetFix;
			uniform float4 _MainTex_ST;
			uniform float _LightAngle;
			uniform float _LightAmount;
			uniform float _LightExp;
			uniform float4 _AlphaTex_ST;
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_SphereNormal = IN.texcoord.xy * _SphereNormal_ST.xy + _SphereNormal_ST.zw;
				float4 tex2DNode31 = tex2D( _SphereNormal, uv_SphereNormal );
				float4 break54 = ( tex2DNode31.b * (tex2DNode31*2.0 + -1.0) * _BMultiplier );
				float2 appendResult32 = (float2(break54.r , break54.g));
				float mulTime48 = _Time.y * _TimeScaleRotation;
				float2 appendResult47 = (float2(mulTime48 , 0.0));
				float2 uv029 = IN.texcoord.xy * _TilingFix + ( appendResult32 + appendResult47 + _OffsetFix );
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode13 = tex2D( _MainTex, uv_MainTex );
				float3 rotatedValue67 = RotateAroundAxis( float3(0.5,0.5,0.5), tex2DNode31.rgb, float3(0,0,1), radians( _LightAngle ) );
				float3 break63 = (rotatedValue67*_LightAmount + 0.0);
				float4 temp_output_6_0 = ( tex2D( _TextureSample2, uv029 ) * tex2DNode13 * pow( ( break63.x * break63.y ) , _LightExp ) );
				float2 uv_AlphaTex = IN.texcoord.xy * _AlphaTex_ST.xy + _AlphaTex_ST.zw;
				#ifdef ETC1_EXTERNAL_ALPHA
				float staticSwitch8 = tex2D( _AlphaTex, uv_AlphaTex ).a;
				#else
				float staticSwitch8 = tex2DNode13.a;
				#endif
				float4 appendResult10 = (float4(temp_output_6_0.rgb , staticSwitch8));
				
				fixed4 c = ( IN.color * appendResult10 );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16600
-1769;92;1628;767;2636.698;1092.81;2.474814;True;True
Node;AmplifyShaderEditor.RangedFloatNode;53;-2297.146,-377.468;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-2282.446,-444.9679;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-2447.671,-683.6759;Float;True;Property;_SphereNormal;SphereNormal;1;0;Create;True;0;0;False;0;0ce2fdd14f07a7d4e9a06eae1e2f0762;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;58;-2053.534,-349.8002;Float;False;Property;_BMultiplier;BMultiplier;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;51;-2108.245,-494.368;Float;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1979.622,-854.4045;Float;False;Property;_LightAngle;LightAngle;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1872.783,-546.8301;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1962.361,-243.8733;Float;False;Property;_TimeScaleRotation;TimeScaleRotation;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;72;-1635.56,-980.1192;Float;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;0.5,0.5,0.5;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RadiansOpNode;74;-1683.882,-809.7196;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;71;-1374.734,-1005.491;Float;False;Constant;_Vector1;Vector 1;7;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;54;-1714.745,-555.4539;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleTimeNode;48;-1736.161,-232.1738;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1195.51,-628.9303;Float;False;Property;_LightAmount;LightAmount;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;67;-1287.674,-827.4919;Float;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;62;-1480.114,7.377075;Float;False;Property;_OffsetFix;OffsetFix;5;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;32;-1457.184,-502.057;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;47;-1530.76,-239.9738;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;65;-951.5352,-717.0578;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;4;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;61;-1483.255,-111.9574;Float;False;Property;_TilingFix;TilingFix;4;0;Create;True;0;0;False;0;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1298.303,-381.3791;Float;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;63;-714.0944,-710.3945;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;26;-1040.205,384.9464;Float;False;0;0;_AlphaTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-442.262,-568.205;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;25;-1028.081,161.0601;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1184.75,-224.1866;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;80;-555.7861,-480.7349;Float;False;Property;_LightExp;LightExp;8;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;78;-391.9861,-423.5349;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-844.1292,108.1998;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-882.3936,-329.8923;Float;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-777.9281,357.5999;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;8;-425.4257,172.2479;Float;False;Property;_Keyword0;Keyword 0;1;0;Fetch;True;0;0;False;0;0;0;0;False;ETC1_EXTERNAL_ALPHA;Toggle;2;Key0;Key1;Fetch;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-484.8778,-253.6479;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-180.1312,-61.85463;Float;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;9;-208.7957,-289.4231;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SwizzleNode;7;-399.9024,-38.90933;Float;False;FLOAT3;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;18.59476,-144.8546;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;22;344.9937,-129.137;Float;False;True;2;Float;ASEMaterialInspector;0;6;CustomSprite/Planet;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;51;0;31;0
WireConnection;51;1;52;0
WireConnection;51;2;53;0
WireConnection;60;0;31;3
WireConnection;60;1;51;0
WireConnection;60;2;58;0
WireConnection;74;0;68;0
WireConnection;54;0;60;0
WireConnection;48;0;49;0
WireConnection;67;0;71;0
WireConnection;67;1;74;0
WireConnection;67;2;72;0
WireConnection;67;3;31;0
WireConnection;32;0;54;0
WireConnection;32;1;54;1
WireConnection;47;0;48;0
WireConnection;65;0;67;0
WireConnection;65;1;75;0
WireConnection;46;0;32;0
WireConnection;46;1;47;0
WireConnection;46;2;62;0
WireConnection;63;0;65;0
WireConnection;64;0;63;0
WireConnection;64;1;63;1
WireConnection;29;0;61;0
WireConnection;29;1;46;0
WireConnection;78;0;64;0
WireConnection;78;1;80;0
WireConnection;13;0;25;0
WireConnection;16;1;29;0
WireConnection;12;0;26;0
WireConnection;8;1;13;4
WireConnection;8;0;12;4
WireConnection;6;0;16;0
WireConnection;6;1;13;0
WireConnection;6;2;78;0
WireConnection;10;0;6;0
WireConnection;10;3;8;0
WireConnection;7;0;6;0
WireConnection;11;0;9;0
WireConnection;11;1;10;0
WireConnection;22;0;11;0
ASEEND*/
//CHKSM=74126E4F0DF755C4D6FFDF12A6875A88F79A7E1B