using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectiveControls : MonoBehaviour
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
        if (controls.IsControlsEnabled)
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
        directive.Value = Constants.DIRECTIVE_MOVE_FOWARD;
    }

    public void SetDirectiveTurnLeft()
    {
        directive.Value = Constants.DIRECTIVE_TURN_LEFT;
    }

    public void SetDirectiveTurnRight()
    {
        directive.Value = Constants.DIRECTIVE_TURN_RIGHT;
    }

    public void SetDirectiveUTurn()
    {
        directive.Value = Constants.DIRECTIVE_UTURN;
    }

    public void SetDirectiveStop()
    {
        directive.Value = Constants.DIRECTIVE_STOP;
    }
}
