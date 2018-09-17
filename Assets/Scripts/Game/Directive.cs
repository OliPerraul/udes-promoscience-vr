using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directive : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger directive;

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
