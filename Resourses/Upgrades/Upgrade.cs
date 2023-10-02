using Godot;
using System;

public partial class Upgrade : Resource
{
	public string Name;
	 public string description;
	 public string id;
	
	public Upgrade(string Name , string description ,string id)
	{
		this.Name = Name;
		this.description =description;
		this.id = id;
	}
}
