using System;
using System.Collections;
using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Algorithm", order = 1)]
    public class ScriptableAlgorithm : ScriptableObject
    {
        [SerializeField]
        Utils.Algorithm value;

        public Action valueChangedEvent;

        [SerializeField]
        private LocalizeString longestStraightString;

        [SerializeField]
        private LocalizeString rightHandString;

        [SerializeField]
        private LocalizeString shortestFlightDistance;

        [SerializeField]
        private LocalizeString standardString;

        [SerializeField]
        private LocalizeString tutorialString;


        public Utils.Algorithm Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }

        public string Name
        {
            get
            {
                switch (value)
                {
                    case Utils.Algorithm.LongestStraight:
                        return tutorialString.Value;

                    case Utils.Algorithm.RightHand:
                        return tutorialString.Value;

                    case Utils.Algorithm.ShortestFlightDistance:
                        return tutorialString.Value;

                    case Utils.Algorithm.Standard:
                        return tutorialString.Value;
                        
                    default:
                        return "[?]";
                }
            }
        }

        public void OnValueChanged()
        {
            if (valueChangedEvent != null)
            {
                valueChangedEvent();
            }
        }
    }
}
