using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Network
{
    // TODO: Put scriptable variable in scriptable manager/controller

    // NO MORE SCRIPTABLE GLOBAL VARIABLES!


    public class NetworkClientControllerAsset : ScriptableObject
    {
        public Cirrus.NotifyChangeValue<bool> IsConnectedToPair;

        public Cirrus.NotifyChangeValue<bool> IsConnectedToServer;

    }

}