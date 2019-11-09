using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Int", order = 1)]
    public class GameRoundManagerAsset : ScriptableObject
    {
        public Cirrus.NotifyChangeValue<int> Round = new Cirrus.NotifyChangeValue<int>();

        public Cirrus.NotifyChangeValue<bool> IsRoundCompleted = new Cirrus.NotifyChangeValue<bool>();
    }
}