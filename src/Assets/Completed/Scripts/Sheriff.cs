using UnityEngine;

namespace Completed {
	public class Sheriff : Agent {

		public StateMachine<Sheriff> stateMachine;
		
		public int patrolTime = 0;

		public delegate void Greeting();
		public static event Greeting onGreeting;

		public void setPatrolTime(){
			patrolTime = Random.Range(1,11); 
		}

		public void Awake () {
			this.stateMachine = new StateMachine<Sheriff>();
			this.Location = Location.SheriffsOffice;
			this.stateMachine.Init(this, new Patrol(), new SheriffGlobal());
		}

		public void ChangeState (State<Sheriff> state) {
			this.stateMachine.ChangeState(state);
		}

		public override void Update () {
			this.stateMachine.Update();
		}

		public void trigger(){
			onGreeting();
		}

	}

	public class Patrol : State<Sheriff>
    {
        public override void Enter(Sheriff sheriff)
        {

        }

        public override void Execute(Sheriff sheriff)
        {
        	Debug.Log("<color=blue>Patrolling..</color>");
            Location nextLocation = (Location)Random.Range(0,6);
            while(nextLocation==Location.OutlawCamp) nextLocation = (Location)Random.Range(0,6);
            sheriff.ChangeState(new SheriffTravel(nextLocation, new Patrol()));
        }

        public override void Exit(Sheriff sheriff)
        {

        }

    }

    public class DropGoldAtBank : State<Sheriff>
    {
        public override void Enter(Sheriff sheriff){}

        public override void Execute(Sheriff sheriff)
        {
            sheriff.gold = 0;
            Debug.Log("<color=blue>Dropped Gold at bank. Go have a pint at the saloon!</color>");
            sheriff.ChangeState(new SheriffTravel(Location.Saloon, new Patrol()));
        }

        public override void Exit(Sheriff sheriff){}

    }

	public class SheriffTravel : Travel<Sheriff>
	{
		
	    public SheriffTravel(Location goal, State<Sheriff> state)
	    {
	    	Debug.Log("<color=blue>Travelling to " + Locations.ToString(goal) + "..</color>");
	        goalPos = Locations.LocationPositions[(int)goal];
	        goalState = state;
	    }
		
	    public override void Enter(Sheriff sheriff)
	    {
	        this.path = astar.GetPath(sheriff.CurrentPosition, goalPos);
	    }

	    public override void Execute(Sheriff sheriff)
	    {
	        if (path.Count > 0)
	        {
	            sheriff.CurrentPosition = path[0];
	            path.RemoveAt(0);
	        }
	        else
	        {
	            State<Sheriff> previousState = sheriff.stateMachine.prevState;
	            sheriff.ChangeState(goalState);
	            sheriff.stateMachine.prevState = previousState;
	        }
	    }

	    public override void Exit(Sheriff sheriff)
	    {
	        path.Clear();
	    }
	}

	public class SheriffGlobal : State<Sheriff> {

		public override void Enter (Sheriff sheriff) {}

		public override void Execute (Sheriff sheriff) {}

		public override void Exit (Sheriff sheriff) {}
	}
}