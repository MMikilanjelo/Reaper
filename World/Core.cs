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
	Godot.Collections.Array loadingProgres;
	[Export] private WorldGeneratorComponent worldGenerator;
	[Export] TileMap tileMap;
	[Export] PackedScene StatUIScene;
	[Export] private AnimationPlayer transitionAnimation;
	const string DefeatScenePath = "res://UI/DefeateScreen.tscn";
	HealthComponent playerHealth;
	public override void _Ready()
	{
	
		transitionAnimation.Play("fade_out");
		ResourceLoader.LoadThreadedRequest(DefeatScenePath);
		var player = GameUtilities.GetPlayerNode(this);
		var playerHealth = player.GetNode<HealthComponent>("HealthComponent");
		playerHealth.Connect(HealthComponent.SignalName.Died , Callable.From(()=>ChangeScene()));
	}
	

	public override void _Process(double delta)
	{
		// if(Input.IsActionJustPressed("OpenTab"))
		// {
		// 	var statMenu = StatUIScene.Instantiate() as StatUI;
		// 	AddChild(statMenu);
		// }
		//implement stat menu later;
		
	}
	public void ChangeScene()
	{

			var scene_loading_status =	ResourceLoader.LoadThreadedGetStatus(DefeatScenePath , loadingProgres);
			if(scene_loading_status == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				PackedScene DefeatScreenScene  = ResourceLoader.LoadThreadedGet(DefeatScenePath) as PackedScene;
				transitionAnimation.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animName)=>
				{
					if(animName == "fade_in")
					{
						GetTree().ChangeSceneToPacked(DefeatScreenScene);
					}
						
				}));
				transitionAnimation.Play("fade_in");
				
			}
	}
}
