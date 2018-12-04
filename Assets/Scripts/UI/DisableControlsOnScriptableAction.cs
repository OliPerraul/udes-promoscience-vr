using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableControlsOnScriptableAction : MonoBehaviour
{
    [SerializeField]
    ScriptableAction scriptableAction;

    [SerializeField]
    ScriptableControler controls;

    void Start()
    {
        scriptableAction.action += OnScriptableAction;
    }

    void OnScriptableAction()
    {
        controls.IsPlayerControlsEnabled = false;
    }
}
