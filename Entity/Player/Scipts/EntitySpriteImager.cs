using Godot;
using System;

public partial class EntitySpriteImager : Node2D
{
	[Export] Sprite2D EntitySprite;
	[Export] Marker2D HandRotation;
	[Export] PackedScene bulletParickle;
    public override void _Process(double delta)
	{
		if(Mathf.RadToDeg(HandRotation.Transform.Rotation) > -90 && Mathf.RadToDeg(HandRotation.Transform.Rotation)< 90 )
		{
			HandRotation.Scale = new Vector2(1 , 1);
			EntitySprite.Scale = new Vector2(1 , 1);
		}
		else 
		{
			
			HandRotation.Scale = new Vector2(1 ,-1 );
			EntitySprite.Scale = new Vector2(-1 , 1);
		}
	}

	public void EmitBulletShelsParticle()
	{
		GpuParticles2D 	patickles = bulletParickle.Instantiate() as GpuParticles2D;
		patickles.Position = Position;
		this.AddChild(patickles);
	}
	public  void LookAtTarget(Vector2 target)
	{
		HandRotation.LookAt(target);
	}
}
