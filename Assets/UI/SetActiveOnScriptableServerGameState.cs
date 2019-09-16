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
        List<GameObject> gameObjectsToActivateOnTutorial = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnTutorial = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToActivateOnGameRound = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnGameRound = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToActivateOnIntermission = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnIntermission = new List<GameObject>();

        void OnEnable()
        {
            serverGameInformation.gameStateChangedEvent += OnValueChanged;
        }

        void OnValueChanged()
        {
            if (serverGameInformation.GameState == ServerGameState.Tutorial)
            {
                foreach (GameObject gObject in gameObjectsToActivateOnTutorial)
                {
                    gObject.SetActive(true);
                }

                foreach (GameObject gObject in gameObjectsToHideOnTutorial)
                {
                    gObject.SetActive(false);
                }
            }
            else if (serverGameInformation.GameState == ServerGameState.GameRound)
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
            else if (serverGameInformation.GameState == ServerGameState.Intermission)
            {
                foreach (GameObject gObject in gameObjectsToActivateOnIntermission)
                {
                    gObject.SetActive(true);
                }

                foreach (GameObject gObject in gameObjectsToHideOnIntermission)
                {
                    gObject.SetActive(false);
                }
            }
        }
    }
}
