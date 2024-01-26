
using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public partial class AchievementEvents : Node
{
	private ResourcePreloader _achievementResourcePreloader;
	[Signal] public delegate void AchievementUnlockedEventHandler(string _unlockedAchievementId);
	Godot.Collections.Array<Achievement> _achievements = new Godot.Collections.Array<Achievement>();

    public override void _Ready()
    {
		_achievementResourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
		LoadAchievemnts();
		Connect(SignalName.AchievementUnlocked , Callable.From((string _unlockedAchievementId)=> UnlockAchievent(_unlockedAchievementId)));
	}
    public void EmitAchievementUnlocked(string _unlockedAchievementId)
	{
		EmitSignal(SignalName.AchievementUnlocked ,_unlockedAchievementId);
	}
	private void UnlockAchievent(string _unlockedAchievementId)
	{
		var _updatedAchievement = GetAchievementById(_unlockedAchievementId);
		if(_updatedAchievement == null && _updatedAchievement._isUnlocked) return;
		_updatedAchievement._isUnlocked = true;
		ResourceSaver.Save( _updatedAchievement , _updatedAchievement.ResourcePath);
	}
	public Godot.Collections.Array<Achievement> GetAllAchievemnts()
	{
		return _achievements;
	}
	private Achievement GetAchievementById(string _unlockedAchievementId)
	{
		return _achievements.FirstOrDefault(_achievement => _achievement?._achievementId == _unlockedAchievementId);
	}
	private void LoadAchievemnts()
	{
		foreach(var _achieventResourseName in _achievementResourcePreloader.GetResourceList())
		{
			_achievements.Add(_achievementResourcePreloader.GetResource(_achieventResourseName) as Achievement);
		}
	}
}
