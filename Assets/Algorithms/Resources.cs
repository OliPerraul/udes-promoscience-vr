using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Algorithms
{
    public class Resources : ScriptableObject
    {
        [SerializeField]
        public Resource StandardAlgorithm;

        [SerializeField]
        public Resource ShortestFlightDistanceAlgorithm;

        [SerializeField]
        public Resource RightHandAlgorithm;

        [SerializeField]
        public Resource LongestStraightAlgorithm;

        public Algorithm CreateAlgorithm(
            Utils.Algorithm id,
            Labyrinths.IData labyrinth)
        {
            switch (id)
            {
                case Utils.Algorithm.LongestStraight:
                    return new LongestStraightAlgorithm(RightHandAlgorithm, labyrinth);

                case Utils.Algorithm.Standard:
                    return new StandardAlgorithm(RightHandAlgorithm, labyrinth);

                case Utils.Algorithm.RightHand:
                    return new RightHandAlgorithm(RightHandAlgorithm, labyrinth);

                case Utils.Algorithm.ShortestFlightDistance:
                    return new ShortestFlightDistanceAlgorithm(RightHandAlgorithm, labyrinth);
                default:
                    return null;
            }
        }


    }
}