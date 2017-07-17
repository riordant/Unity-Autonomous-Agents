using UnityEngine;
using System.Collections.Generic;

namespace Completed {
	abstract public class State <T> {

		abstract public void Enter (T agent);
		abstract public void Execute (T agent);
		abstract public void Exit (T agent);
	}
	abstract public class Travel<T> : State<T>
	{
	    protected static AStar astar = new AStar();
	    protected Vector3 goalPos;
	    protected State<T> goalState;
	    protected List<Vector3> path;
	}
}