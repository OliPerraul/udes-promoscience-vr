using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    public class Resources : ScriptableObject
    {
        [SerializeField]
        public Algorithms.Resources Algorithms;

        [SerializeField]
        public Labyrinths.ScriptableResources Labyrinths;

        [SerializeField]
        public Replays.Resources Replays;

    }
}
