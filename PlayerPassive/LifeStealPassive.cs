using Game.Components;
using Godot;

namespace PlayerPassive
{
	
	public partial class LifeStealPassive : Node
	{
		public HealthComponent healthComponent;
		private game_events Game_Events;
		private int _lifeStealAmount = 1;
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
			healthComponent.SetCurrentHealth(_lifeStealAmount);
		}
    }
}
