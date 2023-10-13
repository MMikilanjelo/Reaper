using Godot;
using System.Collections.Generic;
using Generation;
using Enemy.Parts;
using System.Security.Cryptography.X509Certificates;
using Game.Components;
using Generation.Alghoritms;
using System.ComponentModel;
using GameUI;
using GameLogick.Utilities;
public partial class Core : Node2D
{
	UIEvents Uievents;
	[Export] private WorldGeneratorComponent worldGenerator;
	[Export] TileMap tileMap;
	[Export] PackedScene StatUIScene;
	[Export] private AnimationPlayer transitionAnimation;
	[Export] PackedScene DefeatScene;
	HealthComponent playerHealth;
	public override void _Ready()
	{
		
		var player = GameUtilities.GetPlayerNode(this);
		var playerHealth = player.GetNode<HealthComponent>("HealthComponent");
		playerHealth.Connect(HealthComponent.SignalName.Died , Callable.From(()=>ChangeScene()));
	}
	

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("OpenTab"))
		{
			var statMenu = StatUIScene.Instantiate() as StatUI;
			AddChild(statMenu);
		}
		
	}
	public void ChangeScene()
	{
		transitionAnimation.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animationName)=>
		{
			if(animationName == "fade_in")
			{
				GetTree().ChangeSceneToPacked(DefeatScene);
			}

		}));
		transitionAnimation.Play("fade_in");
	}
}
