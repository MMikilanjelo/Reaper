using Godot;
using System;
using System.Runtime.CompilerServices;

namespace GameUI
{
	public partial class DefeateScreen : CanvasLayer
	{
		[Export] private ShaderMaterial shaderMaterial;
        [Export] private AnimationPlayer transitionAnimationPlayer;
        [Export] private Label TotalKills;
        [Export] private Label TotalUpgrades;
        [Export] private Label TotalDmg;
        [Export] private Label TotalSouls;
        UIEvents uiEvents;
        public override void _Ready()
        {
            uiEvents = GetNode<UIEvents>("/root/UIEvents");
            TotalKills.Text = uiEvents.playerStatistic._totalKills.ToString();
            TotalDmg.Text = uiEvents.playerStatistic._totalDealtDmg.ToString();
            TotalSouls.Text = uiEvents.playerStatistic._totalSouls.ToString();
            TotalUpgrades.Text = uiEvents.playerStatistic._totalUpgrades.ToString();
            
            transitionAnimationPlayer.Play("Fade_out");
            transitionAnimationPlayer.Connect(AnimationPlayer.SignalName.AnimationFinished , Callable.From((string name)=>
            {
                if(name == "Fade_in")
                {
                    GetTree().ChangeSceneToFile("res://UI/MainMenu.tscn");
                }
            }));
        }
        public override void _Process(double delta)
        {
            if(Input.IsActionJustPressed("CloseTab"))
            {
                transitionAnimationPlayer.Play("Fade_in");
                uiEvents.ResetStatistik();
            }
        }
        public void PlayApperence()
        {
            transitionAnimationPlayer.Play("Fade_out");
            
        }
    }
}

