using Godot;
using System;


namespace GameUI
{
	public partial class AchievementContainer : HBoxContainer
	{
		private TextureRect _achievementTextureRect;
		private Label _achievementConditionLable;
		private string _achievementId;
		public  string AchievementId
		{
			get => _achievementId;
		}
        public override void _Ready()
        {
			_achievementTextureRect = GetNode<TextureRect>("TextureRect");	
			_achievementConditionLable = GetNode<Label>("PanelContainer/Conditions");
        }
		public  void SetAchievement(Achievement _achievementData)
		{
			_achievementTextureRect.Texture = _achievementData._achievementTexture;
			_achievementId = _achievementData._achievementId;
			Unlock(_achievementData);
		}
		private void Unlock(Achievement _achievementData)
		{
			if(_achievementData._isUnlocked)
			{
				(_achievementTextureRect.Material as ShaderMaterial).SetShaderParameter("lerp_percent" , 0.0);
				_achievementConditionLable.Text = _achievementData._achievementСonditions;
			}
		}

		
    }
}	

