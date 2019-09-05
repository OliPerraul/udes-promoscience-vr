using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.UI
{
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
}
