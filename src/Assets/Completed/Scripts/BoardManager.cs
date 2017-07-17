using UnityEngine;
using System;
using System.Collections.Generic;		
using Random = UnityEngine.Random;

namespace Completed {
	public class BoardManager : MonoBehaviour
	{
		//"Location" identifiers
		public static int BANK = 0;
		public static int CEMETERY = 1;
		public static int OUTLAW_CAMP = 2;
		public static int SHERIFFS_OFFICE = 3;
 		public static int UNDERTAKERS = 4;
 		public static int SALOON = 5;

 		//"Agent" identifiers
		public static int MINER = 0;
		public static int OUTLAW = 1;
		public static int SHERIFF = 2;
		public static int UNDERTAKER = 3;

		//"Object Cost" identifiers
		public static float FLOOR_COST = 1;
		public static float MOUNTAIN_COST = 3;
		public static float LOCATION_COST = 500; //arbitrarily high cost for a location
		public static float OUTER_WALL_COST = 500; //arbitrarily high cost for an outer wall

		//Grid variables
		public static int dimension = 8;								//Number of dimension/dimension in our game board.
		public static int mountainsCount = 10;									
		public static int locationsCount = 6;
		public static int agentsCount = 4;
		private Transform boardHolder;									
		private List <Vector3> gridPositions = new List <Vector3> ();	

		//Game objects
		public GameObject[] floorTiles;				
		public GameObject[] outerWallTiles;
		public GameObject[] mountainTiles;
		public GameObject[] locationsTiles;
		public GameObject[] agentsTiles;						
		
		//Agent objects
		public GameObject Jesse;
		public GameObject Wyatt;
		public GameObject Grim;

		void InitialiseGridPositions ()
		{
			gridPositions.Clear();
			for(int x = 1; x < dimension-1; x++)
			{
				for(int y = 1; y < dimension-1; y++)
				{
					Vector3 newPosition = new Vector3(x, y, 0f);
					gridPositions.Add (newPosition);
				}
			}
		}
				
		void BoardSetup ()
		{
			boardHolder = new GameObject ("Board").transform;
			bool outerWall = true;
			for(int x = -1; x < dimension + 1; x++)
			{
				for(int y = -1; y < dimension + 1; y++)
				{
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					
					if(x == -1 || x == dimension || y == -1 || y == dimension){ 
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
						outerWall = true;
					}
					
					Vector3 newPosition = new Vector3(x, y, 0f);
					GameObject instance = Instantiate (toInstantiate, newPosition, Quaternion.identity) as GameObject;
					if(!outerWall) Locations.cost[(int)newPosition.x,(int)newPosition.y] = FLOOR_COST;
					instance.transform.SetParent (boardHolder);

				}
			}
		}
				
		Vector3 RandomPosition ()
		{
			int randomIndex = Random.Range (0, gridPositions.Count);
			Vector3 randomPosition = gridPositions[randomIndex];
			gridPositions.RemoveAt (randomIndex);
			return new Vector3(randomPosition.x, randomPosition.y, 0f);
		}
						
		public void SetupScene ()
		{
			BoardSetup();
			
			InitialiseGridPositions();

			for(int i = 0; i < mountainsCount; i++)
			{
				Vector3 randomPosition = RandomPosition();
				GameObject tileChoice = mountainTiles[0];				
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
				Locations.cost[(int)randomPosition.x,(int)randomPosition.y] = MOUNTAIN_COST;
			}

			//lay out each location exactly once
			for(int i=0;i<locationsCount;i++){ 

				Vector3 RandPosition = RandomPosition();
				Locations.LocationPositions[i] = RandPosition;
				Instantiate(locationsTiles[i], RandPosition, Quaternion.identity);
				Locations.cost[(int)RandPosition.x,(int)RandPosition.y] = LOCATION_COST;

			}

			//lay out each agent exactly once
			Jesse = Instantiate(agentsTiles[OUTLAW], Locations.LocationPositions[OUTLAW_CAMP], Quaternion.identity);
			Wyatt = Instantiate(agentsTiles[SHERIFF], Locations.LocationPositions[SHERIFFS_OFFICE], Quaternion.identity);
			Grim = Instantiate(agentsTiles[UNDERTAKER], Locations.LocationPositions[UNDERTAKERS], Quaternion.identity);
		}
	}
}