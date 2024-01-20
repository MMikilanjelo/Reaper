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
		public  void SetTexture(Texture2D _texture)
		{
			_achievementTextureRect.Texture = _texture;
		}
		public void SetConditionLable(string _conditionText)
		{
			_achievementConditionLable.Text = _conditionText;
		}
		public void SetAchievementId(string _achievementId)
		{
			this._achievementId = _achievementId;
			GD.Print(_achievementId);
		}
		public void OnAchievementUnlocked(string _unlockedAchievementId)
		{
			if(_unlockedAchievementId != _achievementId) return;
		}
		
    }
}	

