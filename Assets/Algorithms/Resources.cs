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

        //Labyrinths.IData labyrinth)
        public Algorithm CreateAlgorithm(Promoscience.Algorithms.Id id)
        {
            switch (id)
            {
                case Promoscience.Algorithms.Id.LongestStraight:
                    return new LongestStraightAlgorithm(LongestStraightAlgorithm);

                case Promoscience.Algorithms.Id.Standard:
                    return new StandardAlgorithm(StandardAlgorithm);

                case Promoscience.Algorithms.Id.RightHand:
                    return new RightHandAlgorithm(RightHandAlgorithm);

                case Promoscience.Algorithms.Id.ShortestFlightDistance:
                    return new ShortestFlightDistanceAlgorithm(ShortestFlightDistanceAlgorithm);
                default:
                    return null;
            }
        }


    }
}