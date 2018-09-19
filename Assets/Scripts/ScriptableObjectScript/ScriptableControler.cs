using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Controler", order = 1)]
public class ScriptableControler : ScriptableObject
{
    [SerializeField]
    bool mIsControlsEnable;

    public Action stopAllMovementEvent;
    public Action resetPositionAndRotation;
    public Action isControlsEnableValueChangedEvent;

    public bool isControlsEnabled
    {
        get
        {
            return mIsControlsEnable;
        }
        set
        {
            mIsControlsEnable = value;
            OnControlsEnableValueChanged();
        }
    }

    public void OnControlsEnableValueChanged()
    {
        if (isControlsEnableValueChangedEvent != null)
        {
            isControlsEnableValueChangedEvent();
        }
    }

    public void StopAllMovement()
    {
        if (stopAllMovementEvent != null)
        {
            stopAllMovementEvent();
        }
    }

    public void ResetPositionAndRotation()
    {
        if (resetPositionAndRotation != null)
        {
            resetPositionAndRotation();
        }
    }
}

