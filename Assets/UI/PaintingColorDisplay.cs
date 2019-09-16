using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class PaintingColorDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableTileColor paintingColor;

        [SerializeField]
        GameObject colorRings;

        [SerializeField]
        GameObject greyRing;

        [SerializeField]
        GameObject yellowRing;

        [SerializeField]
        GameObject redRing;

        void OnEnable()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            paintingColor.valueChangedEvent += OnPaintingColorValueChanged;
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                colorRings.gameObject.SetActive(true);
            }
            else
            {
                colorRings.gameObject.SetActive(false);
            }
        }

        void OnPaintingColorValueChanged()
        {
            if (paintingColor.Value == TileColor.Grey)
            {
                greyRing.SetActive(true);
                yellowRing.SetActive(false);
                redRing.SetActive(false);
            }
            else if (paintingColor.Value == TileColor.Yellow)
            {
                yellowRing.SetActive(true);
                greyRing.SetActive(false);
                redRing.SetActive(false);
            }
            else if (paintingColor.Value == TileColor.Red)
            {
                redRing.SetActive(true);
                greyRing.SetActive(false);
                yellowRing.SetActive(false);
            }
        }
    }
}
