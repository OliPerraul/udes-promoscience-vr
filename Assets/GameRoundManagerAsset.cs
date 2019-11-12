using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Int", order = 1)]
    public class GameRoundManagerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<int> Round = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsRoundCompleted = new Cirrus.ObservableValue<bool>();
    }
}