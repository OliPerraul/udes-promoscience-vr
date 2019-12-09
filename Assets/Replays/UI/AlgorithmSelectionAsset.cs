using UnityEngine;
using System.Collections;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    public class AlgorithmSelectionAsset : ScriptableObject
    {
        public ObservableValue<Algorithms.Id> Algorithm = new ObservableValue<Algorithms.Id>();
    }
}