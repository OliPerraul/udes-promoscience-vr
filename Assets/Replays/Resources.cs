using UnityEngine;
using System.Collections;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class Resources : BaseResources<Resources>
    {
        [SerializeField]
        public ReplayControllerAsset replayController;

        [SerializeField]
        public Segment Segment;

        [SerializeField]
        public PlayerSequence PlayerSequence;

        [SerializeField]
        public AlgorithmSequence AlgorithmSequence;
    }
}
