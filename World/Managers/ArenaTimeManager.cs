using Godot;
using System;

namespace Managers
{
	public partial class ArenaTimeManager : Node
	{
		[Export] Timer timer;

        
		public double GetTImeEepsed()
		{
			return timer.WaitTime - timer.TimeLeft;
		}
	}
}

