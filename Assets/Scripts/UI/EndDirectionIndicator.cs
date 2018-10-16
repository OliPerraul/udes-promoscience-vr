using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDirectionIndicator : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform indicator;

    Vector3 endPosition;

    private void Start()
    {
        currentGameState.valueChangedEvent += OnGameStateChanged;
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
        if (currentGameState.value == Constants.PLAYING_TUTORIAL || currentGameState.value == Constants.PLAYING)
        {
            endPosition = labyrinth.GetLabyrithEndPositionInWorldPosition();
        }
    }

    void OnControlsEnableValueChanged()
    {
        if (controls.isControlsEnabled)
        {
            indicator.gameObject.SetActive(true);
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
