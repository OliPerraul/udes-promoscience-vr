using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{

    public class SetActiveOnScriptableServerGameState : MonoBehaviour
    {
        [SerializeField]
        ScriptableServerGameInformation serverGameInformation;

        [SerializeField]
        List<GameObject> gameObjectsToActivateOnLobby = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnLobby = new List<GameObject>();

        //[SerializeField]
        //List<GameObject> gameObjectsToActivateOnTutorial = new List<GameObject>();

        //[SerializeField]
        //List<GameObject> gameObjectsToHideOnTutorial = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToActivateOnGameRound = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnGameRound = new List<GameObject>();

        //[SerializeField]
        //List<GameObject> gameObjectsToActivateOnIntermission = new List<GameObject>();

        //[SerializeField]
        //List<GameObject> gameObjectsToHideOnIntermission = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToActivateViewingPlayback = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideViewingPlayback = new List<GameObject>();


        void OnEnable()
        {
            serverGameInformation.gameStateChangedEvent += OnValueChanged;

            OnValueChanged();

        }

        void OnValueChanged()
        {
            if (serverGameInformation.GameState == ServerGameState.Lobby ||
                serverGameInformation.GameState == ServerGameState.Intermission)
            {
                foreach (GameObject gObject in gameObjectsToActivateOnLobby)
                {
                    gObject.SetActive(true);
                }

                foreach (GameObject gObject in gameObjectsToHideOnLobby)
                {
                    gObject.SetActive(false);
                }
            }            
            else if (serverGameInformation.GameState == ServerGameState.GameRound ||
                    serverGameInformation.GameState == ServerGameState.Tutorial)
            {
                foreach (GameObject gObject in gameObjectsToActivateOnGameRound)
                {
                    gObject.SetActive(true);
                }

                foreach (GameObject gObject in gameObjectsToHideOnGameRound)
                {
                    gObject.SetActive(false);
                }
            }
            else if (serverGameInformation.GameState == ServerGameState.ViewingPlayback)
            {
                foreach (GameObject gObject in gameObjectsToActivateViewingPlayback)
                {
                    gObject.SetActive(true);
                }

                foreach (GameObject gObject in gameObjectsToHideViewingPlayback)
                {
                    gObject.SetActive(false);
                }
            }
        }
    }
}
