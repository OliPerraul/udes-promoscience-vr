﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class DirectionDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        GameObject directionDisplayer;

        void OnEnable()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                directionDisplayer.SetActive(true);
            }
            else
            {
                directionDisplayer.SetActive(false);
            }
        }
    }
}
