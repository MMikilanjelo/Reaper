using Godot;
using System;
using GameLogick.Utilities;
using Game.Components;

namespace GameUI
{
	public partial class ArrmorContainer : HBoxContainer
	{
		UIEvents Uievents;
		[Export] Label arrmorLabel;

        public override void _Ready()
        {
			Uievents = GetNode<UIEvents>("/root/UIEvents");
			arrmorLabel.Text = (Uievents.playerStats.dmg_reduction * 100).ToString() + "%";

		}
    }

}
