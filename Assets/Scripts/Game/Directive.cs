﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directive : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger directive;

    private void Start()
    {
        controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
    }

    void OnControlsEnableValueChanged()
    {
        if (controls.isControlsEnabled)
        {
            foreach (Transform child in transform)
            {
                if (child != this.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child != this.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetDirectiveMoveFoward()
    {
        directive.value = Constants.DIRECTIVE_MOVE_FOWARD;
    }

    public void SetDirectiveTurnLeft()
    {
        directive.value = Constants.DIRECTIVE_TURN_LEFT;
    }

    public void SetDirectiveTurnRight()
    {
        directive.value = Constants.DIRECTIVE_TURN_RIGHT;
    }

    public void SetDirectiveUTurn()
    {
        directive.value = Constants.DIRECTIVE_UTURN;
    }

    public void SetDirectiveStop()
    {
        directive.value = Constants.DIRECTIVE_STOP;
    }
}
