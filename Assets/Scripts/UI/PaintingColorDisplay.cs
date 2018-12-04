using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingColorDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableTileColor paintingColor;

    [SerializeField]
    GameObject paintingColorImage;

    [SerializeField]
    Image colorImage;

	void Start ()
    {
        controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        paintingColor.valueChangedEvent += OnPaintingColorValueChanged;
    }

    void OnControlsEnableValueChanged()
    {
        if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
        {
            paintingColorImage.gameObject.SetActive(true);
        }
        else
        {
            paintingColorImage.gameObject.SetActive(false);
        }
    }

    void OnPaintingColorValueChanged()
    {
        if (paintingColor.Value == TileColor.Grey)
        {
            colorImage.color = Color.grey;
        }
        else if (paintingColor.Value == TileColor.Yellow)
        {
            colorImage.color = Color.yellow;
        }
        else if (paintingColor.Value == TileColor.Red)
        {
            colorImage.color = Color.red;
        }
    }
}
