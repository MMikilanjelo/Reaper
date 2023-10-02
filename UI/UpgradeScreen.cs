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
	List<HBoxContainer> CardContainers = new List<HBoxContainer>();

	[Signal] public delegate void UpgradeSelectedEventHandler(Upgrade playerUpgrade);

	public override void _Ready()
	{
		CardContainers.Add(CardContainer);
		CardContainers.Add(CardConteinerRight);
		CardContainers.Add(CardConteinerLeft);
		GetTree().Paused = true;

	}
	public void SetAbilitiesUpgrades(Godot.Collections.Array<Upgrade> upgrades)
	{
		
		foreach(var upgrade in upgrades)
		{
		
			var cardInstance = UpgradeCardScene.Instantiate() as AbilitieUpgradeCard;
			CardContainer.AddChild(cardInstance);
			cardInstance.SetAbilitieUpgrade(upgrade);
			cardInstance.Selected += () => OnUpgradeSelected(upgrade);
			
			
		}
			
	}
	private void OnUpgradeSelected(Upgrade upgrade)
	{
		EmitSignal(SignalName.UpgradeSelected , upgrade);
		GetTree().Paused = false;
		QueueFree();
	}
	
}
