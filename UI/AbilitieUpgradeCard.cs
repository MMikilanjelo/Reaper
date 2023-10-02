using Godot;
using System;

public partial class AbilitieUpgradeCard : PanelContainer
{
	[Export] Label cardNameLabel;
	[Export] Label descriptionCardLabel;
	[Signal] public delegate void SelectedEventHandler();
	[Signal] public delegate void TestSignalEventHandler();

	public override void _Ready()
	{
		Connect(SignalName.GuiInput , new Callable(this , nameof (OnGuiInput)));
	}
	public void SetAbilitieUpgrade(Upgrade upgrade)
	{
		cardNameLabel.Text = upgrade.Name;
		descriptionCardLabel.Text = upgrade.description;
	}
	private void OnGuiInput(InputEvent ev)
	{
		
		if(Input.IsActionJustPressed("Right_Click"))
		{
		 	EmitSignal(SignalName.Selected);
		}
		
		
	}
}
