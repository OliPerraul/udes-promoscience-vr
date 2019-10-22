using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Controler", order = 1)]
    public class ScriptableControler : ScriptableObject
    {
        [SerializeField]
        bool isControlsEnable;

        [SerializeField]
        bool isPlayerControlsEnable;

        public Action stopAllMovementEvent;
        public Action resetPositionAndRotation;
        public Action isControlsEnableValueChangedEvent;
        public Action isPlayerControlsEnableValueChangedEvent;

        public void OnEnable()
        {
            IsControlsEnabled = false;
            IsPlayerControlsEnabled = false;
        } 

        public bool IsControlsEnabled
        {
            get
            {
                return isControlsEnable;
            }
            set
            {
                isControlsEnable = value;
                OnControlsEnableValueChanged();
            }
        }

        public bool IsPlayerControlsEnabled
        {
            get
            {
                return isPlayerControlsEnable;
            }
            set
            {
                isPlayerControlsEnable = value;
                OnPlayerControlsEnableValueChanged();
            }
        }

        public void OnControlsEnableValueChanged()
        {
            if (isControlsEnableValueChangedEvent != null)
            {
                isControlsEnableValueChangedEvent();
            }
        }

        public void OnPlayerControlsEnableValueChanged()
        {
            if (isPlayerControlsEnableValueChangedEvent != null)
            {
                isPlayerControlsEnableValueChangedEvent();
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
}