using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using System.Collections.Generic;

namespace UdeS.Promoscience.Algorithms
{
    public class AlgorithmRespectAsset : ScriptableObject
    {
        public FloatEvent OnRespectChangedHandler;

        public ObservableValue<bool> IsCorrectingEnabled = new Cirrus.ObservableValue<bool>();

        public ObservableValue<Tile> WrongTile = new ObservableValue<Tile>();

        public List<Tile> WrongColorTilesWhenDiverging = new List<Tile>();

        public ObservableValue<bool> IsDiverging = new ObservableValue<bool>();

        public Cirrus.Event OnReturnToDivergencePointRequestHandler;

        public void InvokeReturnToDivergent()
        {
            OnReturnToDivergencePointRequestHandler?.Invoke();
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
