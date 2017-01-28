// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:221,x:33112,y:32579,varname:node_221,prsc:2|emission-9167-OUT,voffset-642-OUT;n:type:ShaderForge.SFN_TexCoord,id:8195,x:31928,y:32610,varname:node_8195,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:3264,x:32460,y:32616,varname:node_3264,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-2569-XYZ;n:type:ShaderForge.SFN_Color,id:2244,x:32524,y:32194,ptovrint:False,ptlb:node_2244,ptin:_node_2244,varname:node_2244,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.08088237,c2:0.1252534,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:4003,x:32524,y:32375,ptovrint:False,ptlb:node_2244_copy,ptin:_node_2244_copy,varname:_node_2244_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2573529,c2:0.7234281,c3:1,c4:1;n:type:ShaderForge.SFN_Lerp,id:9167,x:32678,y:32421,varname:node_9167,prsc:2|A-2244-RGB,B-4003-RGB,T-2147-OUT;n:type:ShaderForge.SFN_Multiply,id:9757,x:32688,y:32665,varname:node_9757,prsc:2|A-3264-OUT,B-2136-OUT,C-2857-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2136,x:32366,y:32803,ptovrint:False,ptlb:node_2136,ptin:_node_2136,varname:node_2136,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Sin,id:2147,x:32877,y:32665,varname:node_2147,prsc:2|IN-9757-OUT;n:type:ShaderForge.SFN_Tau,id:2857,x:32382,y:32883,varname:node_2857,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:8739,x:32752,y:33126,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:642,x:33050,y:33293,varname:node_642,prsc:2|A-8739-OUT,B-9433-OUT,C-2147-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9433,x:32490,y:33368,ptovrint:False,ptlb:node_9433,ptin:_node_9433,varname:node_9433,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_FragmentPosition,id:2569,x:32155,y:32596,varname:node_2569,prsc:2;proporder:2244-4003-2136-9433;pass:END;sub:END;*/

Shader "Custom/NewSurfaceShader" {
    Properties {
        _node_2244 ("node_2244", Color) = (0.08088237,0.1252534,1,1)
        _node_2244_copy ("node_2244_copy", Color) = (0.2573529,0.7234281,1,1)
        _node_2136 ("node_2136", Float ) = 2
        _node_9433 ("node_9433", Float ) = 0.1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d9 opengl gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _node_2244;
            uniform float4 _node_2244_copy;
            uniform float _node_2136;
            uniform float _node_9433;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float node_2147 = sin((mul(unity_ObjectToWorld, v.vertex).rgb.r*_node_2136*6.28318530718));
                v.vertex.xyz += (v.normal*_node_9433*node_2147);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float node_2147 = sin((i.posWorld.rgb.r*_node_2136*6.28318530718));
                float3 emissive = lerp(_node_2244.rgb,_node_2244_copy.rgb,node_2147);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d9 opengl gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _node_2136;
            uniform float _node_9433;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float node_2147 = sin((mul(unity_ObjectToWorld, v.vertex).rgb.r*_node_2136*6.28318530718));
                v.vertex.xyz += (v.normal*_node_9433*node_2147);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
