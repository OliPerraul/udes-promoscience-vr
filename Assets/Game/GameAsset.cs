using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    // TODO remove put in Game

    public class GameAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<int> Round = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsRoundCompleted = new Cirrus.ObservableValue<bool>();

    }
}
