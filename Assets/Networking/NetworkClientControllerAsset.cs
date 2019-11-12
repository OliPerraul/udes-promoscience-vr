using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Network
{
    // TODO: Put scriptable variable in scriptable manager/controller

    // NO MORE SCRIPTABLE GLOBAL VARIABLES!


    public class NetworkClientControllerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<bool> IsConnectedToPair;

        public Cirrus.ObservableValue<bool> IsConnectedToServer;

    }

}