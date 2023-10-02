using Godot;
using System.Collections.Generic;

namespace GameLogick.StateMachine
{
	public  partial class DelegateStateMachine : RefCounted
	{
		public  delegate void State();

		private State currentState;


		private readonly Dictionary<State , StateFlows> states = new ();

		public void AddState(State normalState , State enterState = null , State exitState = null)
		{
			var StateFlows = new StateFlows(normalState , enterState, exitState);
			states[normalState] = StateFlows;
		}
		public void ChangeState(State toStateDelegate)
		{
			states.TryGetValue(toStateDelegate , out var stateDelegates);
			Callable.From(() => SetState(stateDelegates)).CallDeferred();
		}
		public void Update()
		{
			currentState?.Invoke();
		}
		public  State GetCurrentState()
		{
			return  currentState;
		}
		public void SetInitiioalState(State stateDelegate)
		{
			states.TryGetValue(stateDelegate , out var stateFlows);
			SetState(stateFlows);
		}
		private void SetState(StateFlows stateFlows)
		{
			if(currentState != null)
			{
				states.TryGetValue(currentState , out var currentStateDelegates);
				currentStateDelegates?.ExitState?.Invoke();
			}
			currentState = stateFlows.Normal;
			stateFlows?.EnterState?.Invoke();
		}

		private class StateFlows
		{
			public State Normal{get ; private set;}
			public State EnterState{get ; private set;}

			public State ExitState{get;private set;}

			public StateFlows(State normal , State enterstate = null , State exitstate = null)
			{
				Normal = normal;
				EnterState = enterstate;
				ExitState  = exitstate;
			}
		}
	}
}

