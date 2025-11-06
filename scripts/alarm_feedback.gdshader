shader_type canvas_item;

uniform bool enable = false;
uniform float strength = 0.0;
uniform vec3 flash_color = vec3(1.0, 0.0, 0.0);

void fragment() {
	if (enable)
	{
		COLOR = vec4(mix(COLOR.rgb, flash_color, strength), COLOR.a);
	}
	else
	{
		discard;
	}
}