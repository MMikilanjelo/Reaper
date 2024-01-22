using Godot;
namespace GameUI
{
	public partial class AchievementScreen : CanvasLayer
	{
		private Button backButton;
		[Signal] public delegate void BackPressedEventHandler();
        public override void _Ready()
        {
			SetNodes();
			ConnectToSignals();
		}
		#region Seting Up References  
			private void SetNodes()
			{
				backButton =  GetNode<Button>("Control/Control/BackButton");
			}
		#endregion

		#region Connect to Signals
			private void ConnectToSignals()
			{
				backButton.Pressed += () => OnBackPressed();
			}
		#endregion
		private void OnBackPressed()
		{
			EmitSignal(SignalName.BackPressed);
		}
    }
}

