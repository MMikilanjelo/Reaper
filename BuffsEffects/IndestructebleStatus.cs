using Godot;
using System;
using DotEffects;
using Game.Components;
public partial class IndestructebleStatus : BaseEffect 
{
	StatusEfffectData _data;
	[Export] ShaderMaterial shaderMaterial;
	[Export] ResourcePreloader resourcePreloader;
    public override void _Ready()
    {
		effectStatsData = resourcePreloader.GetResource("IndestructableEffect") as EffectStats;
    }
    public override void ApplyEffect(StatusEfffectData _data)
    {
		this._data = _data;
		_data.visuals.Material = shaderMaterial;
		shaderMaterial.SetShaderParameter("color" , new Color(0 , 255 ,255 ,0.5f));
		shaderMaterial.SetShaderParameter("width" , .7);
		shaderMaterial.SetShaderParameter("pattern" , 2);
		_data.healthComponent.canAcceptDamage = false;
		_data.statusRecivierComponent.SetReciveEffect(false);
		_data.statusRecivierComponent.Connect(StatusRecivierComponent.SignalName.OnForsedRemoveEfect , Callable.From(()=> RemoveEffect()));
    }
	public void RemoveEffect()
	{
		_data.healthComponent.canAcceptDamage = true;
		_data.visuals.Material = null;
		_data.statusRecivierComponent.SetReciveEffect(true);
		EmitSignal(SignalName.OnRemoveEfect , this);
		QueueFree();
	}
}
