using UnityEngine;

namespace Completed {
	public class Undertaker : Agent {
		
		public StateMachine<Undertaker> stateMachine;
		
		public int deadBodyCount = 0;

		public bool deadBodyNotification = false;

		public delegate void Greeting();
		public static event Greeting onGreeting;

		public void Awake () {
			Sheriff.onGreeting += Greet;
			this.stateMachine = new StateMachine<Undertaker>();
			this.Location = Location.Undertakers;
			this.stateMachine.Init(this, new LurkUndertakers(), new UndertakerGlobal());
		}

		public void ChangeState (State<Undertaker> state) {
			this.stateMachine.ChangeState(state);
		}

		public override void Update () {
			this.stateMachine.Update();
		}

		public void Greet () {
			Debug.Log("<color=blue>Sheriff</color> greets the <color=green>undertaker!</color>");
		}
	}

	public class PickupDeadBody : State<Undertaker>
    {
        public override void Enter(Undertaker undertaker)
        {

        }

        public override void Execute(Undertaker undertaker)
        {
        	Debug.Log("<color=green>picked up dead body..</color>");
            undertaker.deadBodyCount+=1;
            undertaker.ChangeState(new UndertakerTravel(Location.Cemetery, new DropDeadBody()));
        }

        public override void Exit(Undertaker undertaker)
        {}

    }

	public class DropDeadBody : State<Undertaker>
    {
        public override void Enter(Undertaker undertaker)
        {
            undertaker.Location = Location.Cemetery;
        }

        public override void Execute(Undertaker undertaker)
        {
        	Debug.Log("<color=green>Dropped off dead body at cemetery..</color>");
            undertaker.deadBodyCount-=1;
            undertaker.ChangeState(new UndertakerTravel(Location.Undertakers, new LurkUndertakers()));
        }

        public override void Exit(Undertaker undertaker)
        {

        }

    }

	public class LurkUndertakers : State<Undertaker>
    {
        public override void Enter(Undertaker undertaker)
        {
            undertaker.Location = Location.Undertakers;
        }

        public override void Execute(Undertaker undertaker)
        {
        	Debug.Log("<color=green>Lurking in undertakers..</color>");
            if(undertaker.deadBodyNotification){
				Vector3 pos = new Vector3(0,0,0f);
				bool found = false;
				for(int x=0;x<8 && !found;x++)
				{
					for(int y=0;y<8 && !found;y++)
					{
						if(Locations.deadBodies[x,y]>0)
						{
							pos = new Vector3(x,y,0f);
							Locations.deadBodies[x,y]-=1;
							found = true;
						}
					}
				}
				undertaker.deadBodyNotification = false;
				undertaker.ChangeState(new UndertakerTravel(pos, new PickupDeadBody()));
            }
        }

        public override void Exit(Undertaker undertaker)
        {

        }

    }

	public class UndertakerTravel : Travel<Undertaker>
	{
		
	    public UndertakerTravel(Location goal, State<Undertaker> state)
	    {
	    	Debug.Log("<color=green>Travelling to " + Locations.ToString(goal) + "..</color>");
	        goalPos = Locations.LocationPositions[(int)goal];
	        goalState = state;
	    }

	    public UndertakerTravel(Vector3 goalPos, State<Undertaker> state)
	    {
	    	Debug.Log("<color=green>Travelling to collect dead body..</color>");
	    	this.goalPos = goalPos;
	        goalState = state;

	    }
		
	    public override void Enter(Undertaker undertaker)
	    {
	        this.path = astar.GetPath(undertaker.CurrentPosition, goalPos);
	    }

	    public override void Execute(Undertaker undertaker)
	    {

	        if (path.Count > 0)
	        {
	            undertaker.CurrentPosition = path[0];
	            path.RemoveAt(0);
	        }
	        else
	        {
	            State<Undertaker> previousState = undertaker.stateMachine.prevState;
	            undertaker.ChangeState(goalState);
	            undertaker.stateMachine.prevState = previousState;
	        }
	    }

	    public override void Exit(Undertaker undertaker)
	    {
	        path.Clear();
	    }
	}

	public class UndertakerGlobal : State<Undertaker> {

		public override void Enter (Undertaker undertaker) {

		}

		public override void Execute (Undertaker undertaker) {
			
		}

		public override void Exit (Undertaker undertaker) {
		}
	}
}