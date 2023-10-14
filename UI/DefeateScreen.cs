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
            transitionAnimationPlayer.Play("Fade_out");
            transitionAnimationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string name)=>
            {
                if(name == "Fade_in")
                {
                     GetTree().ChangeSceneToFile("res://UI/MainMenu.tscn");
                }
                   
            }));
            //shaderMaterial = GetNode<TextureRect>("TextureRect").Material as ShaderMaterial;
            
        }
        public override void _Process(double delta)
        {
            if(Input.IsActionJustPressed("CloseTab"))
            {
                transitionAnimationPlayer.Play("Fade_in");
            }
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

