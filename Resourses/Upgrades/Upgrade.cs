using Godot;
using System;

public partial class Upgrade : Resource
{
	[Export] public  string Name;
	[Export(PropertyHint.MultilineText)] public string description;
	[Export] public string id;
	[Export] public bool isUnique;
}
