using Godot;
using System;
using System.Globalization;

public partial class Achievement : Resource
{

	[Export] public Texture2D _achievementTexture;
	[Export(PropertyHint.MultilineText, "Conditions for take achievement")]
	public string _achievementСonditions {get ; set;}
	[Export] public string _achievementId {get ; set;}
	[Export] public bool _isUnlocked {get;set;}
}
