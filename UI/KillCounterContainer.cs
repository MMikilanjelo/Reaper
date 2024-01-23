using Godot;
using System;


namespace GameUI
{
	public partial class KillCounterContainer : HBoxContainer
	{
		[Export] private Label kill_count_label;
		private UIEvents _uiEvents;
        public override void _Ready()
        {
			kill_count_label.Text = "0";
			_uiEvents = GetNode<UIEvents>("/root/UIEvents");
			_uiEvents.Connect(UIEvents.SignalName.PlayerCurrencyUpdated , Callable.From((int _currentCurrency)=>
			{
				UpdateKillsCount(_currentCurrency);
			}));
        }
		private void UpdateKillsCount(int _currentCurrency)
		{
			kill_count_label.Text = _currentCurrency.ToString();
		}

    }
}

