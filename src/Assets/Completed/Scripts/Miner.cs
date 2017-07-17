using UnityEngine;

namespace Completed {
	public class Miner : Agent {

		private StateMachine<Miner> stateMachine;

		public static int WAIT_TIME = 5;
		public int waitedTime = 0;
		public int createdTime = 0;

		public void Awake () {
			this.stateMachine = new StateMachine<Miner>();
			this.stateMachine.Init(this, Wait.Instance, new MinerGlobal());
		}

		public void IncreaseWaitedTime (int amount) {
			this.waitedTime += amount;
		}

		public bool WaitedLongEnough () {
			return this.waitedTime >= WAIT_TIME;
		}

		public void CreateTime () {
			this.createdTime++;
			this.waitedTime = 0;
		}

		public void ChangeState (State<Miner> state) {
			this.stateMachine.ChangeState(state);
		}

		public override void Update () {
			this.stateMachine.Update();
		}
	}

	public sealed class Create : State<Miner> {
		
		static readonly Create instance = new Create();
		
		public static Create Instance {
			get {
				return instance;
			}
		}
		
		static Create () {}
		private Create () {}
		
		public override void Enter (Miner agent) {
			Debug.Log("Gathering creative energies...");
		}
		
		public override void Execute (Miner agent) {
			agent.CreateTime();
			Debug.Log("...creating more time, for a total of " + agent.createdTime + " unit" + (agent.createdTime > 1 ? "s" : "") + "...");
			agent.ChangeState(Wait.Instance);
		}
		
		public override void Exit (Miner agent) {
			Debug.Log("...creativity spent!");
		}
	}

	public sealed class Wait : State<Miner> {

		static readonly Wait instance = new Wait();

		public static Wait Instance {
			get {
				return instance;
			}
		}

		static Wait () {}
		private Wait () {}

		public override void Enter (Miner agent) {
			Debug.Log("Starting to wait...");
		}

		public override void Execute (Miner agent) {
			agent.IncreaseWaitedTime(1);
			Debug.Log("...waiting for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");
			if (agent.WaitedLongEnough()) agent.ChangeState(Create.Instance);
		}

		public override void Exit (Miner agent) {
			Debug.Log("...waited long enough!");
		}
	}

	public class MinerGlobal : State<Miner> {

		public override void Enter (Miner miner) {
		}

		public override void Execute (Miner miner) {
		}

		public override void Exit (Miner miner) {
		}
	}
}