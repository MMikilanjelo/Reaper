using Godot;
using System;
using System.Xml.Schema;

namespace Managers
{
	public partial class ArenaTimeManager : Node
	{
		Timer timer;
		
		const int DIFFICULTY_INTERVAL  = 5;
		private int _waitTime = 30;
		private int _arenaDifficulty;
		private game_events _gameEvents;
        public override void _Ready()
        {
			_gameEvents = GetNode<game_events>("/root/GameEvents");
           timer = GetNode<Timer>("Timer");
		   timer.Connect(Timer.SignalName.Timeout , new Callable(this , nameof (WaveFinished))); 
		   _gameEvents.Connect(game_events.SignalName.NewWaveStarted , Callable.From(()=> OnNewWaveStarted()));
        }
        public override void _Process(double delta)
        {
			var _nextTimeTarget = timer.WaitTime - ((_arenaDifficulty + 1 ) * DIFFICULTY_INTERVAL);
			if(timer.TimeLeft <= _nextTimeTarget)
			{
				_arenaDifficulty += 1 ;
				_gameEvents.EmitDifficultyIncresed(_arenaDifficulty);
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
		private void OnNewWaveStarted()
		{
			timer.Start(_waitTime);
			_waitTime += DIFFICULTY_INTERVAL;
		}
	}
}

