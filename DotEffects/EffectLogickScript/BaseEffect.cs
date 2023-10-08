using Godot;
using System;
using Game.Components;

namespace DotEffects
{
	public partial class BaseEffect : Node2D
	{
		public virtual void ApplyEffect(StatusEfffectData _data)
		{

		}
		public virtual int GetMaxStacks()
		{
			return 0;
		}
	}
}

