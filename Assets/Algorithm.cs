using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{
    public class Algorithm : MonoBehaviour
    {
        [SerializeField]
        private ScriptableAlgorithm algo;

        [SerializeField]
        RightHandAlgorithm rightHandAlgorithm;

        [SerializeField]
        LongestStraightAlgorithm longestStraightAlgorithm;

        [SerializeField]
        ShortestFlightDistanceAlgorithm shortestFlightDistanceAlgorithm;

        [SerializeField]
        StandardAlgorithm standardAlgorithm;

        public List<Tile> GetAlgorithmSteps()
        {
            List<Tile> algorithmSteps = null;

            if (algo.Value == Utils.Algorithm.RightHand)
            {
                algorithmSteps = rightHandAlgorithm.GetAlgorithmSteps();
            }
            else if (algo.Value == Utils.Algorithm.LongestStraight)
            {
                algorithmSteps = longestStraightAlgorithm.GetAlgorithmSteps();
            }
            else if (algo.Value == Utils.Algorithm.ShortestFlightDistance)
            {
                algorithmSteps = shortestFlightDistanceAlgorithm.GetAlgorithmSteps();
            }
            else if (algo.Value == Utils.Algorithm.Standard)
            {
                algorithmSteps = standardAlgorithm.GetAlgorithmSteps();
            }

            return algorithmSteps;
        }
    }
}