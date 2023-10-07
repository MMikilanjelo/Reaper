using Game.Components;
using Godot;
using System;


namespace DotEffects
{
	
	public partial class ToxicDotEffect : Node2D , IEffect
	{
	    public void ApplyEffect()
        {
			GD.Print("Applyed Toxic dmg");
		}
		public void HandleEffect()
        {
			GD.Print("deadling dmg");
        }
		public void RemoveEffect()
        {
           GD.Print("dmg is over"); 
		   QueueFree();
        }
        public override void _Ready()
        {

            ApplyEffect();
			HandleEffect();
			RemoveEffect();
		}
    }
	public interface IEffect
	{
		public void ApplyEffect();
		public void HandleEffect();
		public void RemoveEffect();
	}
	public partial class StatusEffectData : RefCounted
	{
		private float _affect_time_duration;
		private float _affect_tick_time;
		private HealthComponent entityHealthComponent;
	
	}
}
