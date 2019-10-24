using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
////using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Algorithms
{
    public abstract class Algorithm
    {
        protected Resource resource;

        //protected Labyrinths.IData labyrinth;

        public string Name {
            get
            {
                return resource.Name;
            }
        }

        public abstract List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth);

        public abstract Promoscience.Algorithm Id { get; }

        public Algorithm(Resource resource)//, Labyrinths.IData labyrinth)
        {
            this.resource = resource;
            //this.labyrinth = labyrinth;
        }
    }
}