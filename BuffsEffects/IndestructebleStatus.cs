using Godot;
using System;
using DotEffects;
using Game.Components;
public partial class IndestructebleStatus : BaseEffect 
{
	StatusEfffectData _data;
    public override void ApplyEffect(StatusEfffectData _data)
    {
		this._data = _data;
		_data.healthComponent.canAcceptDamage = false;
		_data.statusRecivierComponent.Connect(StatusRecivierComponent.SignalName.OnForsedRemoveEfect , Callable.From(()=> RemoveEffect()));
    }
	public void RemoveEffect()
	{
		_data.healthComponent.canAcceptDamage = true;
		EmitSignal(SignalName.OnRemoveEfect);
		QueueFree();
	}
}
