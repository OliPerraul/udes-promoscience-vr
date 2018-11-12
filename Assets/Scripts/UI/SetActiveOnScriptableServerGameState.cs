using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnScriptableServerGameState : MonoBehaviour
{
    [SerializeField]
    ScriptableServerGameState gameState;

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

    void Start()
    {
        gameState.valueChangedEvent += OnValueChanged;
    }

    void OnValueChanged()
    {
        if(gameState.Value == ServerGameState.Tutorial)
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
        else if (gameState.Value == ServerGameState.GameRound)
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
        else if (gameState.Value == ServerGameState.Intermission)
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
