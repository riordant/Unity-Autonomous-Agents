using UnityEngine;
namespace Completed {
	public class StateMachine <T> {
		
		public T agent;
		public State<T> currState;
		public State<T> prevState;
		public State<T> globalState;

		public void Awake () {
			this.currState=null;
			this.prevState=null;
			this.globalState=null;
		}

		public void Init (T agent, State<T> startState, State<T> globalState) {
			this.agent = agent;
			this.currState = startState;
			this.currState.Enter(this.agent);
			this.prevState = null;
			this.globalState = globalState;

		}

		public void Update () {
			if (this.globalState != null) this.globalState.Execute(agent);
			if (this.currState != null) this.currState.Execute(agent);
		}
		
		public void ChangeState (State<T> nextState) {
			this.prevState = this.currState;

			if(this.currState != null) this.currState.Exit(this.agent);	

			this.currState = nextState;

			if(this.currState != null) this.currState.Enter(this.agent);
		}
	}
}