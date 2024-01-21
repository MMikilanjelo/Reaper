using Godot;
using System;
using System.Xml.Schema;

namespace Managers
{
	public partial class ArenaTimeManager : Node
	{
		Timer timer;
		[Signal] public delegate void DifficultyIncreasedOverTimeEventHandler(int _arenaDifficulty);
		const int DIFFICULTY_INTERVAL  = 5;
		private int _arenaDifficulty;
		private game_events _gameEvents;
        public override void _Ready()
        {
			_gameEvents = GetNode<game_events>("/root/GameEvents");
           timer = GetNode<Timer>("Timer");
		   timer.Connect(Timer.SignalName.Timeout , new Callable(this , nameof (WaveFinished))); 
        }
        public override void _Process(double delta)
        {
			var _nextTimeTarget = timer.WaitTime - ((_arenaDifficulty + 1 ) * DIFFICULTY_INTERVAL);
			if(timer.TimeLeft <= _nextTimeTarget)
			{
				_arenaDifficulty += 1 ;
				EmitSignal(SignalName.DifficultyIncreasedOverTime , _arenaDifficulty);
			}
        }
        public double GetTImeEepsed()
		{
			return timer.WaitTime - timer.TimeLeft;
		}
		private void WaveFinished()
		{
			_gameEvents.EmitWaveFinishing();
		}
	}
}

