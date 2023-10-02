using Godot;
using System;

public partial class ShootPositionParticleEmiter : Marker2D
{
	
	public void  EmitShootPartickles(Vector2 ParticleEmitDirection , PackedScene ShootPartickle)
	{
		GpuParticles2D 	shootPartickle = ShootPartickle.Instantiate() as GpuParticles2D;
		shootPartickle.GlobalPosition = this.Position;
		shootPartickle.RotationDegrees = this.RotationDegrees;
		this.GetParent().AddChild(shootPartickle);
	}
}
