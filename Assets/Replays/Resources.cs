using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays
{
    public class Resources : ScriptableObject
    {
        [SerializeField]
        public Segment Segment;

        [SerializeField]
        public PlayerSequence PlayerSequence;

        [SerializeField]
        public AlgorithmSequence AlgorithmSequence;

        public static Resources Instance;

        public void OnEnable()
        {
            Instance = this;
        }
    }
}
