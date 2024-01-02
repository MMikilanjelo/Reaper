using Game.Components;
using Godot;

namespace PlayerPassive
{
	
	public partial class LifeStealPassive : Node
	{
		public HealthComponent healthComponent;
		private game_events Game_Events;
        public override void _Ready()
        {
			Game_Events = GetNode<game_events>("/root/GameEvents");
			Game_Events.Connect(game_events.SignalName.OnEnemyDied, 
			Callable.From((Vector2 pos , int enemy_cost_inBullets) => 
			{
				AddHealth();
			}));
        }
		private void AddHealth()
		{
			GD.Print("Add health");
			healthComponent.SetCurrentHealth(1);
		}
    }
}
