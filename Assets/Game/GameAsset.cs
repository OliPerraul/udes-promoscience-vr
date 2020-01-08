using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    // TODO remove put in Game

    /// <summary>
    ///  Shared state of the game
    /// </summary>
    public class GameAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<int> Level = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsLevelCompleted = new Cirrus.ObservableValue<bool>();

    }
}
