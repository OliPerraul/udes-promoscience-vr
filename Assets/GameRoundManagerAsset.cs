using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Int", order = 1)]
    public class GameRoundManagerAsset : ScriptableObject
    {
        public Cirrus.MonitoredValue<int> Round = new Cirrus.MonitoredValue<int>();

        public Cirrus.MonitoredValue<bool> IsRoundCompleted = new Cirrus.MonitoredValue<bool>();
    }
}