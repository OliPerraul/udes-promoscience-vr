using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    public enum State
    {
        Select,
        Editor
    }

    public class ControllerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<State> State = new Cirrus.ObservableValue<State>();

        public IData Labyrinth;


    }
}
