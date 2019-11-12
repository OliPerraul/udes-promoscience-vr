using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

namespace UdeS.Promoscience.Algorithms
{
    [System.Serializable]
    public enum Id : int
    {
        Randomized = -1,
        GameRound = -2,

        Tutorial = 0,
        RightHand = 0,
        ShortestFlightDistance = 1,
        LongestStraight = 2,
        Standard = 3
    }

    public class Utils
    {
        public static Id Random
        {
            get
            {
                return (Id)UnityEngine.Random.Range((int)Id.Tutorial, (int)Id.Standard);
            }
        }
    }

    public class Algorithm : ScriptableObject
    {

        [SerializeField]
        public LocalizeInlineString name;

        public string Name
        {
            get
            {
                return name.Value;                
            }
        }

        [SerializeField]
        public LocalizeString description;

        public string Description
        {
            get
            {
                return description.Value;
            }
        }

        public const int NumAlgorithms = 4;

        protected Algorithm resource;

        //protected Labyrinths.IData labyrinth;


        public virtual List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth) { return null; }

        public virtual Id Id { get { return 0; } }



    }
}