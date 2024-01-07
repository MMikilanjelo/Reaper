using Godot;
using System;
using System.Data.Common;

namespace GameUI
{
	public partial class OptionsMenu : CanvasLayer
	{
		[Export] Slider musicSlider;
		[Export] Slider sfxSlider;
		[Export] Button backButton;
		[Signal] public delegate void BackPressedEventHandler();
        public override void _Ready()
        {
            UpdateDisplay();
			sfxSlider.ValueChanged += (double _value) => OnAudioBusChanged( _value, "sfx");
			musicSlider.ValueChanged += (double _value) => OnAudioBusChanged(_value, "music");
			backButton.Pressed += () => OnBackPressed();
			
        }
		private void UpdateDisplay()
		{
			sfxSlider.Value = GetBusVolumePercent("sfx");
			musicSlider.Value = GetBusVolumePercent("music");
		}
        private void SetBusVolumePercent(string _busName , double _percent)
		{
			var _busIndex = AudioServer.GetBusIndex(_busName);
			var _volumeDb = Mathf.DbToLinear(_percent);
			AudioServer.SetBusVolumeDb(_busIndex , (float)_volumeDb);
			GD.Print(_volumeDb);
		}
		private float GetBusVolumePercent(string _busName)
		{
			var _busIndex = AudioServer.GetBusIndex(_busName);
			var _volumeDb = AudioServer.GetBusVolumeDb(_busIndex);
			return Mathf.DbToLinear(_volumeDb);
		}
		private void OnAudioBusChanged(double _value , string _busName)
		{	
			SetBusVolumePercent(_busName , _value);
		}
		private void OnBackPressed()
		{
			EmitSignal(SignalName.BackPressed);
		}

	}
	
}

