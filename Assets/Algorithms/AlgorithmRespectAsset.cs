using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;

namespace UdeS.Promoscience.Algorithms
{
    public class AlgorithmRespectAsset : ScriptableObject
    {
        public FloatEvent OnRespectChangedHandler;

        public MonitoredValue<bool> IsDiverging = new MonitoredValue<bool>();

        //public MonitoredValue<bool> ReturnToDivergencePointAnswer = new MonitoredValue<bool>();

        public Cirrus.Event OnReturnToDivergencePointRequestHandler;

        public void InvokeReturnToDivergent()
        {
            if (OnReturnToDivergencePointRequestHandler != null)
                OnReturnToDivergencePointRequestHandler.Invoke();
        }

        public int ErrorCount = 0;

        private float respect;

        public float Respect
        {
            get
            {
                return respect;
            }

            set
            {
                if (respect.Approximately(value))
                    return;

                respect = value;
                if (OnRespectChangedHandler != null)
                {
                    OnRespectChangedHandler.Invoke(respect);
                }
            }
        }

    }

}
