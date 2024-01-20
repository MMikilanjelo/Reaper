using Godot;
using System;
using System.Globalization;

public partial class Achievement : Resource
{

	[Export] public Texture2D _achievementTexture;
	[Export(PropertyHint.MultilineText, "Conditions for take achievement")]
	public string _achievementСonditions;
	[Export] public string _achievementId;
}
