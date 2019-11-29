using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience
{
    public class ServerCameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
        }

        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.Quickplay:
                case ServerState.Round:
                    camera.gameObject.SetActive(false);
                    break;

                default:
                    camera.gameObject.SetActive(true);
                    break;
            }
        }
    }

}