using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;


namespace GameUI
{
	public partial class AchievementController : GridContainer	
	{
		
		[Export] ResourcePreloader _resourcePreloader;
		AchievementEvents _achievementEvents;
		private PackedScene _achievementContainerScene;
    	public override void _Ready()
        {
			_achievementContainerScene = _resourcePreloader.GetResource("AchievementContainer") as PackedScene;
			_achievementEvents = GetNode<AchievementEvents>("/root/AchievementsEvents");
			SetUpAchievements(_achievementEvents.GetAllAchievemnts());
			
        }
		public void SetUpAchievements(Godot.Collections.Array<Achievement> _achievements)
		{
			foreach(var _achievementData in _achievements)
			{
				var _achievementContainerInstance = _achievementContainerScene.Instantiate() as AchievementContainer;
				AddChild(_achievementContainerInstance);
				_achievementContainerInstance.SetAchievement(_achievementData);
			}
		}

	}
}
