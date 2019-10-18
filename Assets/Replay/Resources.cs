using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replay
{
    public class Resources : ScriptableObject
    {
        [SerializeField]
        public Segment Segment;

        [SerializeField]
        public PlayerSequence PlayerSequence;

        [SerializeField]
        public AlgorithmSequence AlgorithmSequence;
    }
}
