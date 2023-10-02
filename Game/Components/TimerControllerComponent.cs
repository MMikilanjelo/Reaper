using Godot;
using System;

namespace Game.Components
{
	public partial class TimerControllerComponent : Node
	{
		public Timer CreateTimer(float seconds , bool AutoStart = false , bool OneShoot = false)
		{
			Timer timer = new Timer();
			timer.Autostart = AutoStart;
			timer.OneShot = OneShoot;
			AddChild(timer);
			return timer;
		}
	}
}

