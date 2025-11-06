shader_type canvas_item;
uniform float time_passed;
uniform float wave_intensity = 0.1;
uniform vec4 color_start : source_color = vec4(0.0, 0.0, 1.0, 1.0);
uniform vec4 color_end : source_color = vec4(1.0, 0.0, 0.0, 1.0);

void fragment() {
	vec2 uv = UV;
	uv.x += sin(uv.y * 10.0 + time_passed * 5.0) * wave_intensity;
	vec4 texture_color = texture(TEXTURE, uv);
	vec3 top_color = mix(color_start.rgb, color_end.rgb, (sin(time_passed * 6.0) + 1.0) / 2.0);
	vec3 bottom_color = mix(color_start.rgb, color_end.rgb, (sin(time_passed * 6.0 + 1.0) + 1.0) / 2.0);
    vec3 gradient_color = mix(top_color, bottom_color, uv.y);
	if (uv.x < 0.1 || uv.x > 0.9) {
		discard;
	}
	COLOR = vec4(gradient_color, texture_color.a);
}