Shader "BiohazardSDK/Public/Star System"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
			Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+12500" "IsEmissive" = "true"  }
			Cull Off
			ZTest Always
			LOD 200
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
#define time _Time.y


			float random(float x) {
				return frac(sin(x) * 1e4);
			}

			float noise2(float3 p) {
				float3 ip = floor(p);
				p = frac(p);
				p = smoothstep(0.0, 1.0, p);
				float3 st = float3(7, 37, 289);
				float4 pos = dot(ip, st) + float4(0, st.y, st.z, st.y + st.z);
				float4 val = lerp(frac(sin(pos) * 7894.552), frac(sin(pos + st.x) * 7894.552), p.x);
				float2 val2 = lerp(val.xz, val.yw, p.y);
				return lerp(val2.x, val2.y, p.z);
			}

			
			float noise(float3 p) {
				const float3 step = float3(110.0, 241.0, 171.0);

				float3 i = floor(p);
				float3 f = frac(p);

			
				float n = dot(i, step);

				float3 u = f * f * (3.0 - 2.0 * f);
				return lerp(lerp(lerp(random(n + dot(step, float3(0.0, 0.0, 0.0))),
					random(n + dot(step, float3(1.0, 0.0, 0.0))),
					u.x),
					lerp(random(n + dot(step, float3(0.0, 1.0, 0.0))),
						random(n + dot(step, float3(1.0, 1.0, 0.0))),
						u.x),
					u.y),
					lerp(lerp(random(n + dot(step, float3(0.0, 0.0, 1.0))),
						random(n + dot(step, float3(1.0, 0.0, 1.0))),
						u.x),
						lerp(random(n + dot(step, float3(0.0, 1.0, 1.0))),
							random(n + dot(step, float3(1.0, 1.0, 1.0))),
							u.x),
						u.y),
					u.z);
			}

			float2x2 rot(float a) {
				float ca = cos(a);
				float sa = sin(a);
				return float2x2(ca, sa, -sa, ca);
			}

			float sphere(float3 p, in float3 centerPos, float radius) {
				return length(p - centerPos) - radius;
			}

			float fire(float3 p, float3 centerPos, float scale, float radius) {
				float l = min(length(p - centerPos) / radius, 1.0) * 0.7 + 0.3;
				float nl = (1.0 - l);
				float x = (noise((p + sin(time) * 2.0 * l) * 0.2) * 2.0 - 1.0) * 15.0 * nl;
				float y = (noise((p + sin(time + 5.0) * 2.0 * l) * 0.2) * 2.0 - 1.0) * 15.0 * nl;
				float z = (noise((p + sin(time + 3.5) * 2.0 * l) * 0.2) * 2.0 - 1.0) * 15.0 * nl;
				p += float3(x, y, z);
				return max((noise(p * scale) + noise(p * 2.0 * scale) * 0.5 +
					noise(p * 3.0 * scale) * 0.33 +
					noise(p * 4.0 * scale) * 0.25) * 0.4807 - (l * l * l * l), 0.0);
			}

			float flow(float3 p, float scale) {
				float l = length(p - float3(0, -20, 0));
				return lerp(sin(l - time * 10.0) + cos(l - time * 10.0),
					0.0,
					min(1.0 - (100.0 - l) * 0.01, 1.0));
			}
			float map(float3 p) {
				return sphere(p, float3(0.0, 0.0, 0.0), 20.0);
			}

			float map2(float3 p) {
				return sphere(p, float3(0.0, 0.0, 0.0), 10.0);
			}

			float mapHyper(float3 p) {
				return fire(p, float3(0.0, 0.0, 0.0), 0.3, 20.0);
			}


			float dot2(float3 v) { return dot(v, v); }

			float udQuad(float3 p, float3 a, float3 b, float3 c, float3 d)
			{

				float3 ba = b - a; float3 pa = p - a;
				float3 cb = c - b; float3 pb = p - b;
				float3 dc = d - c; float3 pc = p - c;
				float3 ad = a - d; float3 pd = p - d;
				float3 nor = cross(ba, ad);

				return sqrt(
					(sign(dot(cross(ba, nor), pa)) +
						sign(dot(cross(cb, nor), pb)) +
						sign(dot(cross(dc, nor), pc)) +
						sign(dot(cross(ad, nor), pd)) < 3.0)
					?
					min(min(min(
						dot2(ba * clamp(dot(ba, pa) / dot2(ba), 0.0, 1.0) - pa),
						dot2(cb * clamp(dot(cb, pb) / dot2(cb), 0.0, 1.0) - pb)),
						dot2(dc * clamp(dot(dc, pc) / dot2(dc), 0.0, 1.0) - pc)),
						dot2(ad * clamp(dot(ad, pd) / dot2(ad), 0.0, 1.0) - pd))
					:
					dot(nor, pa) * dot(nor, pa) / dot2(nor));
			}

			float map3(float3 p) {
				return udQuad(p,
					float3(10000, 20.0, 10000.0),
					float3(-10000.0, 20.1, 10000.0),
					float3(-10000.0, 20.0, -10000.0),
					float3(10000.0, 20.0, -10000.0));
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = float2(i.uv);
				uv -= 0.5;
				uv /= (i.uv, 1);
				float3 s = float3(0, 0, -100);
				float t2 = (time * 0.5 + 10.0);
				s.y = -abs(cos(t2 * 0.1) * 100.0);
				s.xz = (t2 * 0.1);

				float3 t = float3(0, 0, 0);
				float3 cz = normalize(t - s);
				float3 cx = normalize(cross(cz, float3(0, 1, 0)));
				float3 cy = normalize(cross(cz, cx));
				float3 r = normalize(uv.x * cx + uv.y * cy + cz * 0.7);

				
				float3 p = s;
				float dd = 0.0;
				for (int i = 0; i < 60; ++i) {
					float d = map(p);
					if (d < 0.0001) break;
					if (dd > 500.0) { dd = 500.0; break; }
					p += d * r * 0.8;
					dd += d;
				}

				float2 c = float2(0, 0);
				for (int i = 0; i < 400; ++i) {
					float d = map(p);
					float d2 = map2(p);
					c += float2(mapHyper(p) * 0.018, 0);
					c.y = 1.0;
					if (dd >= 500.0) { dd = 500.0; c.y = 0.0; break; }
					if (d2 < 0.001) { c.x += 0.5; c.y = 2.0; break; }
					if (d > 0.001) { break; }
					p += r * (0.1);
					dd += 0.1;
				}

				
				p = s;
				dd = 0.0;
				int hit = 1;
				for (int i = 0; i < 60; ++i) {
					float d = map3(p);
					if (d < 0.0001) break;
					if (dd > 500.0) { dd = 500.0; hit = 0; break; }
					p += d * r * 0.8;
					dd += d;
				}
				
				float2 off = float2(0.01, 0);
				p.y = p.y + flow(p, 0.1) * 2.0;
				float3 n = normalize(map3(p) - float3(map3(p - off.xyy), map3(p - off.yxy), map3(p - off.yyx)));
				if (hit == 1) { r = reflect(p - s, n); s = p - r; r = normalize(r); }

				p = s;
				dd = 0.0;
				for (int i = 0; i < 60; ++i) {
					float d = map(p);
					if (d < 0.0001) break;
					if (dd > 500.0) { dd = 500.0; break; }
					p += d * r * 0.8;
					dd += d;
				}

				
				float dotr = (dot(r, normalize(float3(0, -1, 1))));

				float3 sky = lerp(float3(0.2, 0.1, 0.1),
					float3(0.1, 0.1, 0.2),
					min(dotr + 0.2, 1.0));

				float3 star = float3(dot(r, normalize(float3(1.0, 0.0, 0.0))),
					dot(r, normalize(float3(0.0, 1.0, 0.0))),
					dot(r, normalize(float3(0.0, 0.0, 1.0))));

				float starPoint = noise(star * 200.0) * 30.0;

				float3 b = lerp(float3(1.0, 1.0, 0.5),
					sky,
					min(starPoint + 0.2, 1.0));

				
				float2 c2 = float2(0, 0);
				if (hit == 1) {
					for (int i = 0; i < 400; ++i) {
						float d2 = map2(p);
						c2 += float2(mapHyper(p) * 0.018, 0);
						c2.y = 1.0;
						if (dd >= 500.0) { dd = 500.0; c2.y = 0.0; break; }
						if (d2 < 0.001) { c2.x += 0.5; c2.y = 2.0; break; }
						p += r * (0.1);
						dd += 0.1;
					}
				}

				
				float3 col = (0.0);
				if (c.y == 1.0)col = lerp(float3(1.0, 0.0, 0.0), float3(1.0, 1.0, 0.0), c.x);
				if (c.y == 2.0)col = lerp(float3(1.0, 0.0, 0.0), float3(1.0, 1.0, 0.0), c.x);

				float3 col2 = (0.0);
				if (c2.y == 1.0) col2 = lerp(float3(1.0, 0.0, 0.0), float3(1.0, 1.0, 0.0), c2.x);
				if (c2.y == 2.0) col2 = lerp(float3(1.0, 0.0, 0.0), float3(1.0, 1.0, 0.0), c2.x);
				if (c2.y == 0.0) col2 = b;
				else {
					if (c2.y != 2.0)col2 = lerp(b, col2, c2.x);
				}
				if (c.y == 0.0)col = col2;
				else {
					if (c.y != 2.0)col = lerp(col2, col, c.x);
				}

				return float4(col, 1.0);
			}

			
			
			ENDCG
		}
	}
}
