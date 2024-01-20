using Enemy.Parts;
using Godot;
using System;
using System.Globalization;

public partial class AchievementEvents : Node
{

	[Signal] public delegate void AchievementUnlockedEventHandler(string _unlockedAchievementId);
	public void EmitAchievementUnlocked(string _unlockedAchievementId)
	{
		EmitSignal(SignalName.AchievementUnlocked ,_unlockedAchievementId);
	}

}
