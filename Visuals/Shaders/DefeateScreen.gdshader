shader_type canvas_item;

uniform vec4 first_color : source_color;
uniform vec4 second_color : source_color;

uniform float angle_radians = 0;


vec2 calculateVectorFromAngle(float targetAngle) {
	return vec2(cos(targetAngle), sin(targetAngle));
}

bool getEqualOrLessThanBoundary(float x, float y, vec2 boundary) {
	return (x - 0.5) * boundary.x <= (y - 0.5) * boundary.y;
}

bool getGreaterThanBoundary(float x, float y, vec2 boundary) {
	return (x - 0.5) * boundary.x > (y - 0.5) * boundary.y;
}

void fragment() {
	vec2 line_1 = calculateVectorFromAngle(angle_radians);
	vec2 line_2 = calculateVectorFromAngle(angle_radians + PI/2.0);
	
	if (getEqualOrLessThanBoundary(UV.x, UV.y, line_1) && 
		getEqualOrLessThanBoundary(UV.x, UV.y, line_2)) {
		COLOR = first_color;
	} else if (getGreaterThanBoundary(UV.x, UV.y, line_1) &&
		getGreaterThanBoundary(UV.x, UV.y, line_2)) {
		COLOR = first_color;
	} else if (getEqualOrLessThanBoundary(UV.x, UV.y, line_1) &&
		getGreaterThanBoundary(UV.x, UV.y, line_2)) {
		COLOR = second_color;
	} else if (getGreaterThanBoundary(UV.x, UV.y, line_1) &&
		getEqualOrLessThanBoundary(UV.x, UV.y, line_2)) {
		COLOR = second_color;
	}
}
