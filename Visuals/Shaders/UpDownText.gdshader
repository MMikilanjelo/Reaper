shader_type canvas_item;

uniform float height = 3;
uniform float speed = 3;

void vertex() {
 VERTEX.y += height * sin(( UV.x) + (speed * TIME));
}
