using Godot;
using System;

public partial class PlayerSpriteImager : Node2D
{
	[Export] Sprite2D PlayerSprite;
	[Export] Marker2D HandRotation;
	[Export] public ShootPositionParticleEmiter shootPositionParticleEmiter;
	[Export] PackedScene bulletParickle;
	public override void _Process(double delta)
	{
		HandRotation.LookAt(GetGlobalMousePosition());
		if(Mathf.RadToDeg(HandRotation.Transform.Rotation) > -90 && Mathf.RadToDeg(HandRotation.Transform.Rotation)< 90 )
		{
			HandRotation.Scale = new Vector2(1 , 1);
			PlayerSprite.Scale = new Vector2(1 , 1);
		}
		else 
		{
			
			HandRotation.Scale = new Vector2(1 ,-1 );
			PlayerSprite.Scale = new Vector2(-1 , 1);
		}
	}

	public void EmitBulletShelsParticle()
	{
		GpuParticles2D 	patickles = bulletParickle.Instantiate() as GpuParticles2D;
		patickles.Position = Position;
		this.AddChild(patickles);
	}
}
