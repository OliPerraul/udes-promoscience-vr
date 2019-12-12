using UnityEngine;
using System.Collections;
using Cirrus;
using System;

namespace UdeS.Promoscience.Replays
{
    public class AlgorithmSelectionAsset : ScriptableObject
    {
        public ObservableValue<Algorithms.Id> Algorithm = new ObservableValue<Algorithms.Id>();

        // TODO use in both game and level replay
        public ObservableValue<int> MoveIndex = new ObservableValue<int>(0);

        internal void OnAlgorithmMoveIndexChanged(int value)
        {
            throw new NotImplementedException();
        }
    }
}