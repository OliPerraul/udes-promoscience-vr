using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Algorithms
{
    public class Resource : ScriptableObject
    {
        [SerializeField]

        public Id Id;

        [SerializeField]
        public LocalizeInlineString name;

        public string Name
        {
            get
            {
                return name.Value;                
            }
        }

        [SerializeField]
        public LocalizeString description;

        public string Description
        {
            get
            {
                return description.Value;
            }
        }


        public Algorithm Create()
        {
            switch (Id)
            {
                case Id.LongestStraight:
                    return new LongestStraightAlgorithm(this);

                case Id.Standard:
                    return new StandardAlgorithm(this);

                case Id.RightHand:
                    return new RightHandAlgorithm(this);

                case Id.ShortestFlightDistance:
                    return new ShortestFlightDistanceAlgorithm(this);
                default:
                    return null;
            }
        }
    }
}