using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDirectionIndicator : MonoBehaviour
{
    [SerializeField]
    ScriptableClientGameState gameState;

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform indicator;

    Vector3 endPosition;

    private void Start()
    {
        gameState.valueChangedEvent += OnGameStateChanged;
        controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
    }

    void Update ()
    {
	    if(indicator.gameObject.activeSelf)
        {
            indicator.LookAt(endPosition);
        }
	}

    public void OnGameStateChanged()
    {
        if (gameState.Value == ClientGameState.PlayingTutorial || gameState.Value == ClientGameState.Playing)
        {
            endPosition = labyrinth.GetLabyrithEndPositionInWorldPosition();
        }
    }

    void OnControlsEnableValueChanged()
    {
        if (controls.IsControlsEnabled)
        {
            indicator.gameObject.SetActive(true);
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
