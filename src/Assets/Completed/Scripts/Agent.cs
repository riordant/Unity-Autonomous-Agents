using UnityEngine;
using System.Collections;

namespace Completed {
    abstract public class Agent : MonoBehaviour {

    	abstract public void Update ();

    	public int gold;
    	
        private Vector3 position;
        public Vector3 CurrentPosition
        {
            get { return position; }
            set { position = value; }
        }
    	public Location Location
            {
                get { return Locations.GetLocation(position); }
                set { position = Locations.LocationPositions[(int)value]; }
            }
    }
}