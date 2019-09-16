using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{
    public class Compass : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        Transform indicator;

        int direction = 0;

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        void OnEnable()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void Update()
        {
            if (indicator.gameObject.activeSelf)
            {
                indicator.LookAt(indicator.position + new Vector3(100 * Constants.TILE_SIZE * xByDirection[direction], 0, 100 * Constants.TILE_SIZE * -yByDirection[direction]));
            }
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                indicator.gameObject.SetActive(true);
            }
            else
            {
                indicator.gameObject.SetActive(false);
            }
        }
    }
}