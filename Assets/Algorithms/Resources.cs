using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Algorithms
{
    public class Resources : ScriptableObject
    {
        public static Resources Instance;

        public void OnEnable()
        {
            Instance = this;
        }

        [SerializeField]
        public Resource StandardAlgorithm;

        [SerializeField]
        public Resource ShortestFlightDistanceAlgorithm;

        [SerializeField]
        public Resource RightHandAlgorithm;

        [SerializeField]
        public Resource LongestStraightAlgorithm;

        public Algorithm CreateAlgorithm(Promoscience.Algorithm id)
            //Labyrinths.IData labyrinth)
        {
            switch (id)
            {
                case Promoscience.Algorithm.LongestStraight:
                    return new LongestStraightAlgorithm(RightHandAlgorithm);

                case Promoscience.Algorithm.Standard:
                    return new StandardAlgorithm(RightHandAlgorithm);

                case Promoscience.Algorithm.RightHand:
                    return new RightHandAlgorithm(RightHandAlgorithm);

                case Promoscience.Algorithm.ShortestFlightDistance:
                    return new ShortestFlightDistanceAlgorithm(RightHandAlgorithm);
                default:
                    return null;
            }
        }


    }
}