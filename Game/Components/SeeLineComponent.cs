using Godot;
using System;
using Game.Components;
public partial class SeeLineComponent : RayCast2D
{
	private float viewDistance = 100f;
	public void SetTargetPosition(Vector2 newTargetPos)
	{
		TargetPosition = newTargetPos.Normalized() * viewDistance;
	}
	public void SetCollisionMask(uint mask)
	{
		CollisionMask = mask;
	}
	public void SetViewDistance(float value)
	{
		viewDistance = value;
	}
}
