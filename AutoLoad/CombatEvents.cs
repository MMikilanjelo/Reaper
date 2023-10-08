using Game.Components;
using Godot;
using System;

public partial class CombatEvents : Node
{
	[Signal] public delegate void HittedByHitBoxEventHandler(HitInfo hitInfo);
	[Signal] public delegate void ApplyAffexToHittedEntityEventHandler(PackedScene efectToApply , HitInfo hitInfo);
	[Signal] public delegate void ApplyAffexToEntityEventHandler(PackedScene efectToApply,StatusRecivierComponent entety);

	public void OnHitByHitBox(HitInfo hitInfo)
	{
		EmitSignal(SignalName.HittedByHitBox , hitInfo);
	}

	public void Apply_Affex_To_HittedEntity(PackedScene efectToApply , HitInfo hitInfo)
	{
		EmitSignal(SignalName.ApplyAffexToHittedEntity , efectToApply , hitInfo);
	}
	public void Apply_Affex_To_Entity(PackedScene efectToApply,StatusRecivierComponent entity)
	{
		EmitSignal(SignalName.ApplyAffexToEntity , efectToApply , entity);
	}	

}
