using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReturnToDivergencePointRequest : MonoBehaviour
{

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableBoolean isDiverging;

    [SerializeField]
    ScriptableBoolean returnToDivergencePointAnswer;

    [SerializeField]
    ScriptableAction returnToDivergencePointRequest;

    [SerializeField]
    GameObject gameObjectToActivate;


    void Start()
    {
        returnToDivergencePointRequest.action += OnScriptableAction;
        isDiverging.valueChangedEvent += OnIsDivergingValueChanged;
    }

    void OnScriptableAction()
    {
        if (isDiverging.Value)
        {
            controls.IsControlsEnabled = false;
            gameObjectToActivate.SetActive(true);
        }
        else
        {
            returnToDivergencePointAnswer.Value = false;
        }
    }

    void OnIsDivergingValueChanged()
    {
        if(gameObjectToActivate.activeSelf)
        {
            returnToDivergencePointAnswer.Value = false;
            gameObjectToActivate.SetActive(false);
            controls.IsControlsEnabled = true;
        }
    }
}
