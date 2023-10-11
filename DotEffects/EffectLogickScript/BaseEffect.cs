using Godot;
using System;
using Game.Components;

namespace DotEffects
{
	public partial class BaseEffect : Node2D
	{
		[Signal] public delegate void OnRemoveEfectEventHandler();
		protected EffectStats effectStatsData;
		public virtual void ApplyEffect(StatusEfffectData _data)
		{
				
		}
		public virtual int GetMaxStacks()
		{
			return 0;
		}
	}
}

