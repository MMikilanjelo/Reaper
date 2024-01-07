using Godot;
using System;
using System.Xml.Resolvers;

namespace GameUI
{
	
	public partial class MainMenu : CanvasLayer
	{
		Godot.Collections.Array loadingProgres;
		const string sceneName = "res://World/Scenes/World.tscn";
		private Button StartButton;
		PackedScene optionScene;
		[Export] Button exitButton;
		[Export] AnimationPlayer transitionAnimation;
		[Export] ResourcePreloader resourcePreloader;
		[Export] Button optionsButton;

        public override void _Ready()
        {
			optionScene = resourcePreloader.GetResource("OptionsMenu") as  PackedScene;
			transitionAnimation.Play("fade_out");
			StartButton = GetNode<Button>("Control/MarginContainer/MarginContainer/HBoxContainer/VBoxContainer/StartButton");
			StartButton.Pressed += () =>  LoadMainScene();
			optionsButton.Pressed += () => LoadOptionsMenu();
			exitButton.Pressed += () => OnExitPressed();
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
		private void LoadOptionsMenu()
		{
			var optionInstance = optionScene.Instantiate() as OptionsMenu;
			AddChild(optionInstance);
			optionInstance.BackPressed += () => OnOptionClosed(optionInstance);
		}
		private void OnOptionClosed(Node optionsInstance)
		{
			optionsInstance.QueueFree();
		}
		private void OnExitPressed()
		{
			GetTree().Quit();
		}
    }
}
