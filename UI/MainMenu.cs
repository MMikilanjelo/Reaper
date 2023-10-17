using Godot;
using System;

namespace GameUI
{
	
	public partial class MainMenu : CanvasLayer
	{
		Godot.Collections.Array loadingProgres;
		const string sceneName = "res://World/Scenes/World.tscn";
		private Button StartButton;
		[Export] private Button QuitBUtton;
		[Export] AnimationPlayer transitionAnimation;

        public override void _Ready()
        {
			
			transitionAnimation.Play("fade_out");
			StartButton = GetNode<Button>("Control/MarginContainer/MarginContainer/HBoxContainer/VBoxContainer/StartButton");
			StartButton.Pressed += () =>  LoadMainScene();
			ResourceLoader.LoadThreadedRequest(sceneName);
		}
		private void LoadMainScene()
		{
			
			var scene_loading_status =	ResourceLoader.LoadThreadedGetStatus(sceneName , loadingProgres);
			if(scene_loading_status == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				PackedScene mainScene  = ResourceLoader.LoadThreadedGet(sceneName) as PackedScene;
				transitionAnimation.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animName)=>
				{
					GetTree().ChangeSceneToPacked(mainScene);
				}));
				transitionAnimation.Play("fade_in");
				
			}
		}
    }
}
