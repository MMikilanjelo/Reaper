using Godot;
using System;

public partial class AbilitieUpgradeCard : PanelContainer
{
	[Export] Label cardNameLabel;
	[Export] Label descriptionCardLabel;
	AnimationPlayer animationPlayer;
	AnimationPlayer hover_animation_player;
	[Signal] public delegate void SelectedEventHandler();
	[Signal] public delegate void TestSignalEventHandler();
	private bool disabled = false;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hover_animation_player = GetNode<AnimationPlayer>("HoverAnimationPlayer");
		Connect(SignalName.GuiInput , new Callable(this , nameof (OnGuiInput)));
		Connect(SignalName.MouseEntered , Callable.From(()=>
		{
			if(disabled)
			{
				return;
			}
			hover_animation_player.Play("hover");
		}));
	} 
	public void SetAbilitieUpgrade(Upgrade upgrade)
	{
		cardNameLabel.Text = upgrade.Name;
		descriptionCardLabel.Text = upgrade.description;
	}
	private void OnGuiInput(InputEvent ev)
	{
		if(disabled)
		{
			return;
		}
		if(Input.IsActionJustPressed("Right_Click"))
		{
			SelectCard();
		}
		
		
	}
	public void PlayIn(double deley = 0)
	{
		Modulate =Colors.Transparent;
		GetTree().CreateTimer(deley).Connect(Timer.SignalName.Timeout , Callable.From(()=>
		{
			animationPlayer.Play("in");
		}));
		
	}
	public void PlayDiscard()
	{
		animationPlayer.Play("discard");
	}
	public void SelectCard()
	{
		disabled = true;
		animationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string animation_name)=>
		{
			if(animation_name == "selected")
			{
				EmitSignal(SignalName.Selected);
			}
		}));
		animationPlayer.Play("selected");
		foreach (AbilitieUpgradeCard other_card in GetTree().GetNodesInGroup("upgrade_card"))
		{
			if(other_card == this)
			{
				continue;
			}
			other_card.PlayDiscard();
		}
	}
}
