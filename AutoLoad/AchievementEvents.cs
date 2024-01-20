using Enemy.Parts;
using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public partial class AchievementEvents : Node
{
	[Signal] public delegate void AchievementUnlockedEventHandler(string _unlockedAchievementId);
	[Export] Godot.Collections.Array<Achievement> _achievements = new Godot.Collections.Array<Achievement>();

    public override void _Ready()
    {
		AchievementUnlocked += (string _unlockedAchievementId) => UnlockAchievent(_unlockedAchievementId);
		//GetTree().CreateTimer(6f).Connect(Timer.SignalName.Timeout , Callable.From(()=> UnlockAchievent("crab")));
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
		ResourceSaver.Save( _updatedAchievement,_updatedAchievement.ResourcePath);
	}
	public Godot.Collections.Array<Achievement> GetAllAchievemnts()
	{
		return _achievements;
	}
	private Achievement GetAchievementById(string _unlockedAchievementId)
	{
		return _achievements.FirstOrDefault(_achievement => _achievement?._achievementId == _unlockedAchievementId);
	}
}
