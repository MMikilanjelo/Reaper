using Godot;
namespace Game.Components
{
	public partial class HitBoxComponent : Area2D
	{	
		[Export] public float dmg = 1;
		[Export] public CollisionShape2D hitBoxArea;
		[Signal] public delegate void OnImpacktEventHandler();
		[Signal] public delegate void OnWallCollideEventHandler();

		private PackedScene afex = null;
		public PackedScene AtackAfex
		{
			get => afex;
			private	set => afex = value;
		} 
		public override void _Ready()
    	{
			Connect(SignalName.BodyEntered , new Callable(this, nameof(onBodyEntered)));
		}
    	public void OnHit()
		{
		  	EmitSignal(SignalName.OnImpackt);
		}
		private void onBodyEntered(Node2D Body)
		{
			EmitSignal(SignalName.OnWallCollide);
		}
		public void SetDisable(bool diseable)
		{
			hitBoxArea.Disabled = diseable;
		}
		public void EnableHitBox()
		{
			hitBoxArea.Disabled = false;
		}
		public void DisableHitBox()
		{
			hitBoxArea.Disabled = true;
		}
		public void EmitImpackt()
		{
			EmitSignal(SignalName.OnImpackt);
		}
		public void AddEffecToHit(PackedScene afexToAdd)
		{
			if(afexToAdd != null)
			{
				AtackAfex = afexToAdd;
			}
			
		}
	}
}

