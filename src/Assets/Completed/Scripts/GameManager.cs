using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Completed {	
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance = null;
		public BoardManager boardScript;		
		public Outlaw outlaw;
		public Sheriff sheriff;
		public Undertaker undertaker;
		
		void Awake()
		{
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);	
			DontDestroyOnLoad(gameObject);
			
			boardScript = GetComponent<BoardManager>();
			boardScript.SetupScene();

			//add Agent components to GameObjects
			outlaw = boardScript.Jesse.AddComponent<Outlaw>();
			sheriff = boardScript.Wyatt.AddComponent<Sheriff>();
			undertaker = boardScript.Grim.AddComponent<Undertaker>();
		}
			
		void Update()
		{
			//draw Agent at new Location
			boardScript.Jesse.transform.position = outlaw.CurrentPosition;
			boardScript.Wyatt.transform.position = sheriff.CurrentPosition;
			boardScript.Grim.transform.position  = undertaker.CurrentPosition;

			System.Threading.Thread.Sleep(250);

			checkColocation();
		}

		void checkColocation(){
			
			if (sheriff.CurrentPosition==outlaw.CurrentPosition)
            {
            	Debug.Log("Killed the outlaw!");
            	sheriff.gold += outlaw.gold;
            	outlaw.gold = 0;
            	outlaw.reset = true;
            	System.Threading.Thread.Sleep(500); //wait for the end of the cycle
            	outlaw.Awake();
            	if(sheriff.gold>0) {
            		sheriff.ChangeState(new SheriffTravel(Location.Bank, new DropGoldAtBank()));
            	}
            	Vector3 position = sheriff.CurrentPosition;
            	Locations.deadBodies[(int)position.x,(int)position.y]+=1;
            	undertaker.deadBodyNotification = true;
            }

            if (sheriff.CurrentPosition==undertaker.CurrentPosition)
            {
            	sheriff.trigger();
            }
		}
	}
}

