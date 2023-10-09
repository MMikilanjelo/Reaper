using Game.Components;
using Godot;
using System;

public partial class CombatEvents : Node
{
	[Signal] public delegate void HittedByHitBoxEventHandler();
	[Signal] public delegate void ApplyAffexToHittedEntityEventHandler(PackedScene efectToApply);
	[Signal] public delegate void ApplyAffexToEntityEventHandler(PackedScene efectToApply,StatusRecivierComponent entety);

	public void OnHitByHitBox()
	{
		EmitSignal(SignalName.HittedByHitBox);
	}

	public void Apply_Affex_To_HittedEntity(PackedScene efectToApply )
	{
		EmitSignal(SignalName.ApplyAffexToHittedEntity , efectToApply);
	}
	public void Apply_Affex_To_Entity(PackedScene efectToApply,StatusRecivierComponent entity)
	{
		EmitSignal(SignalName.ApplyAffexToEntity , efectToApply , entity);
	}	

}

