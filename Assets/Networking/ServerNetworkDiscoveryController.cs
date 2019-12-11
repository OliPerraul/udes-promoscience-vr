using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UdeS.Promoscience.Network
{
    // TODO remove
    public class ServerNetworkDiscoveryController : MonoBehaviour
    {
        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scn, LoadSceneMode md)
        {
            // TODO fix
            // We cannot have multiple network discovery and for some reason ours DontDestroyOnLoad
            if(!scn.path.Contains("Lobby")) DoStop();
        }

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<ServerNetworkDiscovery>()?.DoStart();
        }

        void DoStop()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            GetComponent<ServerNetworkDiscovery>()?.DoStop();
            gameObject.Destroy();
        }
    }
}