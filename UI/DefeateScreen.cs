using Godot;
using System;

namespace GameUI
{
	public partial class DefeateScreen : CanvasLayer
	{
		[Export] private ShaderMaterial shaderMaterial;
        [Export] private AnimationPlayer transitionAnimationPlayer;
        private TextureRect texture;
        double angle_redius  = 0;
        public override void _Ready()
        {
            //shaderMaterial = GetNode<TextureRect>("TextureRect").Material as ShaderMaterial;
            transitionAnimationPlayer.Play("Fade_out");
        }
        public override void _Process(double delta)
        {
            // angle_redius += 0.5f * delta;
            // angle_redius = angle_redius % 360f;
            // GD.Print(angle_redius);
		    // shaderMaterial.SetShaderParameter("angle_radians"   , angle_redius);
            
        }
        public void PlayApperence()
        {
            transitionAnimationPlayer.Play("Fade_out");
        }
    }
}

