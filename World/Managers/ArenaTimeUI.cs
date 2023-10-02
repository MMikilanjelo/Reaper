using Godot;
using System;


namespace Managers
{
	public partial class ArenaTimeUI : CanvasLayer
	{
	 	[Export]ArenaTimeManager arenaTimeManager;	
		[Export] Label label;
       
        public override void _Process(double delta)
        {
            if(arenaTimeManager == null)
			{
				return;
			}
			float timeElapsed = (float)arenaTimeManager.GetTImeEepsed();
			label.Text = Format_Secontds_ToString(timeElapsed);
        }
		private string Format_Secontds_ToString(float seconds)
		{
			var minutes = MathF.Floor(seconds/ 60);
			var remaining_seconds = seconds - (minutes * 60);
			int displaySecenods = (int)Math.Floor(remaining_seconds);
			return minutes.ToString() + ":" + displaySecenods.ToString("D2");
		}
    
		
	}
}

