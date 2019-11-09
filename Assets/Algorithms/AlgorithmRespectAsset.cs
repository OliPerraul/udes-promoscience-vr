using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;

namespace UdeS.Promoscience.Algorithms
{
    public class AlgorithmRespectAsset : ScriptableObject
    {
        public FloatEvent OnRespectChangedHandler;

        public NotifyChangeValue<bool> IsDiverging = new Cirrus.NotifyChangeValue<bool>();

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
