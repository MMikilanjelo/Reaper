using Game.Components;
using Godot;

public partial class experience : Node2D
{
	[Export] Area2D PickableArea;
  	[Export]private AudioStreamPlayer2D soundPlayer;
	private game_events game_Events;
  	private bool _isCollected = false;
	private int _expDrop = 1;
	public override void _Ready()
	{
		
		game_Events = GetNode<game_events>("/root/GameEvents");
		PickableArea.Connect(Area2D.SignalName.AreaEntered , new Callable(this , nameof(OnAreaEntered)));
		game_Events.OnAbilityUpgradeAded += 
		(Upgrade addedUpgrade , Godot.Collections.Dictionary<Upgrade , int> playerUpgrades) => 
		IncreaseExpDrop(addedUpgrade);
		
	}

	private void  OnAreaEntered(Area2D oterArea)
	{
		if(oterArea is HitBoxComponent || _isCollected)
		{
			return;
		}
		_isCollected = true;
		soundPlayer.Play();
		this.Visible = false;
		soundPlayer.Connect(AudioStreamPlayer2D.SignalName.Finished , Callable.From(()=>
		{
			QueueFree();
		}));
		game_Events.On_ExperienceVialCollected(_expDrop);
	}
	private void IncreaseExpDrop(Upgrade addedUpgrade)
	{
		if(addedUpgrade.id == "exp")
		{
			_expDrop += (int)addedUpgrade.value;
		}
	}
}
	