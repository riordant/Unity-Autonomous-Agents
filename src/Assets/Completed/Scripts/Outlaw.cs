using UnityEngine;

namespace Completed {

public class Outlaw : Agent {


	public StateMachine<Outlaw> stateMachine;

	public int lurkTime = 0;

	public bool reset = false;

	public void setLurkTime(){
		lurkTime = Random.Range(1,30); 
	}

	public bool isBored(){
		return (lurkTime <= 0);
	}
	public void Awake () {
		this.stateMachine = new StateMachine<Outlaw>();
		this.stateMachine.Init(this, new OutlawCamp(), new OutlawGlobal());
		this.reset = false;
	}

	public void ChangeState (State<Outlaw> state) {
		this.stateMachine.ChangeState(state);
	}

	public override void Update () {
		this.stateMachine.Update();
		this.lurkTime--;
	}
}

public class OutlawCamp : State<Outlaw> {
	public override void Enter (Outlaw outlaw) {		
		outlaw.reset = false;
		outlaw.Location=Location.OutlawCamp;
		outlaw.setLurkTime();
	}

	public override void Execute (Outlaw outlaw) {
		Debug.Log("<color=red>Lurking in the camp...</color>");
	}

	public override void Exit (Outlaw outlaw) {	

	}
}

public class LurkCemetery : State<Outlaw> {
	public override void Enter (Outlaw outlaw) {
		outlaw.Location=Location.Cemetery;	
		outlaw.setLurkTime();
	}

	public override void Execute (Outlaw outlaw) {
		Debug.Log("<color=red>Lurking in the cemetery...</color>");
	}

	public override void Exit (Outlaw outlaw) {
	}
}

public class LurkBank : State<Outlaw> {
	public override void Enter (Outlaw outlaw) {
		outlaw.Location=Location.Bank;
	}

	public override void Execute (Outlaw outlaw) {
		int newGold = Random.Range(1,11);
		Debug.Log("<color=red>Robbing the Bank! Got " + newGold + " gold coins.</color>");
		outlaw.gold += newGold;
		outlaw.ChangeState(new OutlawTravel(outlaw.stateMachine.prevState.GetType() == typeof(OutlawCamp) ? Location.OutlawCamp : Location.Cemetery, outlaw.stateMachine.prevState));
	}

	public override void Exit (Outlaw outlaw) {
	}
}

public class OutlawTravel : Travel<Outlaw>
{
    public OutlawTravel(Location goal, State<Outlaw> state)
    {
    	Debug.Log("<color=red>Travelling to " + Locations.ToString(goal) + "..</color>");
        goalPos = Locations.LocationPositions[(int)goal];
        goalState = state;
    }

    public override void Enter(Outlaw outlaw)
    {
        this.path = astar.GetPath(outlaw.CurrentPosition, goalPos);
    }

    public override void Execute(Outlaw outlaw)
    {
    	if(!outlaw.reset)
    	{
	        if (path.Count > 1)
	        {
	            outlaw.CurrentPosition = path[0];
	            path.RemoveAt(0);
	        }
	        else
	        {
	            State<Outlaw> previousState = outlaw.stateMachine.prevState;
	            outlaw.ChangeState(goalState);
	            outlaw.stateMachine.prevState = previousState;
	        }
	    }
    }

    public override void Exit(Outlaw outlaw)
    {
        path.Clear();
    }
}

public class OutlawGlobal : State<Outlaw> {

	public override void Enter (Outlaw outlaw) {
		
	}

	public override void Execute (Outlaw outlaw) {
		if(!outlaw.reset)
		{
			string loc = Locations.ToString(outlaw.Location);

			if(outlaw.isBored())
			{
				if(outlaw.Location==Location.OutlawCamp){
				 outlaw.ChangeState(new OutlawTravel(Location.Cemetery, new LurkCemetery()));
				}
				else if(outlaw.Location==Location.Cemetery){
					outlaw.ChangeState(new OutlawTravel(Location.OutlawCamp, new OutlawCamp()));

				}
			}
		
			int randTime = Random.Range(1,11); //once every 10 cycles go to the bank.
			if(outlaw.lurkTime==randTime && !(outlaw.Location==Location.Bank && loc !="Unknown Location"))
			{
				outlaw.ChangeState(new OutlawTravel(Location.Bank, new LurkBank()));
			}
		}
	}

	public override void Exit (Outlaw outlaw) {
		
	}
}
}