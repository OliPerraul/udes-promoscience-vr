using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    public class GameManagerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<int> Round = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsRoundCompleted = new Cirrus.ObservableValue<bool>();

    }
}
