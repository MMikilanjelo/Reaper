using Godot;
using System.Collections.Generic;


namespace GameLogick.StateMachine
{
	public partial class ImediateStateMachine<T> 
	{
		public delegate void StateDelegate();
		public T currentState{get;private set;}

		private readonly Dictionary<T  , StateDelegate> _states = new ();
		private readonly Dictionary<StateDelegate , T> _stateDelegates = new ();
		private readonly Dictionary<T  , StateDelegate> _Leavstates = new ();

		public void AddState(T state  , StateDelegate stateDelegate)
		{
			_states.Add(state , stateDelegate);
		}

		public void AddLeaveState(T leavstate , StateDelegate leaveStateDelegate)
		{
			_Leavstates.Add(leavstate, leaveStateDelegate);
		}
		public void ChangeState(T stateToChange)
		{
			if(_Leavstates.ContainsKey(currentState))
			{
				_Leavstates[currentState]();
			}
			currentState = stateToChange;
			if(_states.ContainsKey(stateToChange))
			{
				_states[stateToChange]();
			}
		}
		public void ChangeState(StateDelegate stateDelegate)
		{
			ChangeState(_stateDelegates[stateDelegate]);
		}
	}
}

