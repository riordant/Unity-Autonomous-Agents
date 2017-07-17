using UnityEngine;
using System.Collections;
using System;
namespace Completed {
    public enum Location
    {
        Bank = 0,
        Cemetery = 1,
        OutlawCamp = 2, 
        SheriffsOffice = 3, 
        Undertakers = 4,
        Saloon = 5
    }

    public class Locations
    {
        public static float[,] cost = new float[BoardManager.dimension,BoardManager.dimension]; //the cost of the tile at that location
        public static int[,] deadBodies = new int[BoardManager.dimension,BoardManager.dimension]; //the amount of dead bodies at that location
        public static Vector3[] LocationPositions = new Vector3[BoardManager.locationsCount];

        //returns the position that the agent is at.
        public static Location GetLocation(Vector3 position)
        {
            for(int i=0; i<LocationPositions.Length; ++i)
            {
                if(Vector3.Distance(position, LocationPositions[i]) < 1.0f)
                {
                    return (Location)i;
                }
            }

            return (Location)(-1);
        }

        

        public static String ToString(Location location)
        {
            switch (location)
            {
                case Location.Bank:
                    return "Bank";
                case Location.Cemetery:
                    return "Cemetery";
                case Location.OutlawCamp:
                    return "Outlaw Camp";
                case Location.SheriffsOffice:
                    return "Sheriff's Office";
                case Location.Undertakers:
                    return "Undertakers";
                case Location.Saloon:
                    return "Saloon";
                default:
                    return "Unknown Location";
            }
        }
    }
}