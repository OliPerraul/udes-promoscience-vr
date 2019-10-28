using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
////using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Algorithms
{
    [System.Serializable]
    public enum Id : int
    {
        Tutorial = 0,
        RightHand = 0,
        ShortestFlightDistance = 1,
        LongestStraight = 2,
        Standard = 3
    }

    public abstract class Algorithm
    {
        public const int NumAlgorithms = 4;

        protected Resource resource;

        //protected Labyrinths.IData labyrinth;

        public string Name {
            get
            {
                return resource.Name;
            }
        }

        public string Description
        {
            get
            {
                return resource.Description;
            }
        }

        public abstract List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth);

        public abstract Promoscience.Algorithms.Id Id { get; }

        public Algorithm(Resource resource)//, Labyrinths.IData labyrinth)
        {
            this.resource = resource;
            //this.labyrinth = labyrinth;
        }
    }
}