shader_type canvas_item;
uniform float lerp_percent : hint_range(0.0 ,1.0);
uniform vec4 target_color: source_color = vec4(1.0 , 1.0 , 1.0 , 1.0);
void fragment() {
	vec4 texture_color = texture(TEXTURE, UV);
	vec4 final_color = mix(texture_color , vec4(target_color.rgb, texture_color.a) , lerp_percent);
	COLOR = final_color;
	
}
