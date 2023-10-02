using Godot;
using System.Collections.Generic;


namespace GameLogick.StateMachine
{
	public partial class StateMachine<T> : RefCounted
	{
		public delegate void StateDelegate();

		private  T currentState;

		private readonly Dictionary<T,StateDelegate> states = new();
		private readonly Dictionary<StateDelegate , T> delegates = new();
		private readonly Dictionary<T,StateDelegate> LeavStates = new();
		private readonly Dictionary<T,StateDelegate> Enterstates = new();

		public void AddState(T currentState , StateDelegate stateDelegate)
		{
			states.Add(currentState , stateDelegate);
			delegates.Add(stateDelegate , currentState);
		}
		public void AddLeaveState(T leavstate , StateDelegate leaveStateDelegate)
		{
			LeavStates.Add(leavstate , leaveStateDelegate);
		}
		public void AddEntereState(T enterState , StateDelegate enterStateDelegate)
		{
			Enterstates.Add(enterState , enterStateDelegate);
		}
		public void ChangeState(T state)
		{
			Callable.From(()=>SetState(state)).CallDeferred();
		}
		public void ChangeState(StateDelegate stateDelegate)
		{
			ChangeState(delegates[stateDelegate]);
		}
		public void SetInitiioalState(T state)
		{
			SetState(state);
		}
		public void SetInitiioalState(StateDelegate stateDelegate)
		{
			SetInitiioalState(delegates[stateDelegate]);
		}

		public void Update()
		{
			if(states.ContainsKey(currentState))
			{
				states[currentState]();
			}
		}
		public T GetCurrentState()
		{
			return currentState;
		}
		public void SetState(T state)
		{
			if(LeavStates.ContainsKey(currentState))
			{
				LeavStates[currentState]();
			}
			currentState = state;
			if(Enterstates.ContainsKey(state))
			{
				Enterstates[currentState]();
			}
		}

		


	}
}

