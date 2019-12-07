using UnityEngine;
using System.Collections;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class Resources : BaseResources<Resources>
    {
        [SerializeField]
        public Segment Segment;

        [SerializeField]
        public TeamReplay PlayerSequence;

        [SerializeField]
        public AlgorithmReplay AlgorithmSequence;
    }
}
