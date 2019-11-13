using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Network
{
    // TODO: Put scriptable variable in scriptable manager/controller

    // NO MORE SCRIPTABLE GLOBAL VARIABLES!


    public class NetworkClientControllerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<bool> IsConnectedToPair = new Cirrus.ObservableValue<bool>();

        public Cirrus.ObservableValue<bool> IsConnectedToServer = new Cirrus.ObservableValue<bool>();

    }

}