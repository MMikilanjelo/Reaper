using Godot;
using System;
using System.Collections.Generic;
using Generation.Alghoritms;

public partial class UpgradeScreen : CanvasLayer
{
	[Export] PackedScene UpgradeCardScene;
	[Export] HBoxContainer CardContainer;
	[Export] HBoxContainer CardConteinerLeft;
	[Export] HBoxContainer CardConteinerRight;
	AnimationPlayer animationPlayer;
	List<HBoxContainer> CardContainers = new List<HBoxContainer>();
	[Signal] public delegate void UpgradeSelectedEventHandler(Upgrade playerUpgrade);

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animation_name)=>
		{
			if(animation_name == "out")
			{
				GetTree().Paused = false;
				QueueFree();
			}	
		}));
		CardContainers.Add(CardContainer);
		CardContainers.Add(CardConteinerRight);
		CardContainers.Add(CardConteinerLeft);
		GetTree().Paused = true;

	}
	public void SetAbilitiesUpgrades(Godot.Collections.Array<Upgrade> upgrades)
	{
		double delay = 0f;
		foreach(var upgrade in upgrades)
		{
		
			var cardInstance = UpgradeCardScene.Instantiate() as AbilitieUpgradeCard;
			CardContainer.AddChild(cardInstance);
			cardInstance.SetAbilitieUpgrade(upgrade);
			cardInstance.PlayIn(delay);
			cardInstance.Selected += () => OnUpgradeSelected(upgrade);
			delay += .2;
			
		}
			
	}
	private void OnUpgradeSelected(Upgrade upgrade)
	{
		EmitSignal(SignalName.UpgradeSelected , upgrade);
		animationPlayer.Play("out");
		
	}
	
}
