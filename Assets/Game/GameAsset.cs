using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    // TODO remove put in Game

    public class GameAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<int> Level = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsLevelCompleted = new Cirrus.ObservableValue<bool>();

    }
}
