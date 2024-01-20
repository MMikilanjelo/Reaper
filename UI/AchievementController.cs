using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;


namespace GameUI
{
	public partial class AchievementController : GridContainer	
	{
		[Export] Godot.Collections.Array<Achievement> _achievements = new Godot.Collections.Array<Achievement>();
		[Export] ResourcePreloader _resourcePreloader;
		AchievementEvents _achievementEvents;
		private PackedScene _achievementContainerScene;
    	public override void _Ready()
        {
			_achievementContainerScene = _resourcePreloader.GetResource("AchievementContainer") as PackedScene;
			_achievementEvents = GetNode<AchievementEvents>("/root/AchievementsEvents");
			SetUpAchievements();
        }
		public void SetUpAchievements()
		{
			foreach(var _achievementData in _achievements)
			{
				
				var _achievementContainerInstance = _achievementContainerScene.Instantiate() as AchievementContainer;
				AddChild(_achievementContainerInstance);
				InitializeAchievementContainer(_achievementContainerInstance , _achievementData);
			}
		}
		private void InitializeAchievementContainer(AchievementContainer _achievementContainer , Achievement _achievementData)
		{
			_achievementEvents.AchievementUnlocked += (string _unlockedAchievementId) => 
			{
				_achievementContainer.OnAchievementUnlocked(_unlockedAchievementId);
			};
			_achievementContainer.SetAchievementId(_achievementData._achievementId);
			_achievementContainer.SetTexture(_achievementData._achievementTexture);
			_achievementContainer.SetConditionLable(_achievementData._achievementСonditions);
		}
	}
}
