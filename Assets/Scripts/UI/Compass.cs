using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    Transform centerEyeTransform;

    [SerializeField]
    Transform indicator;

    int direction = 0;

    readonly float[] rotationByDirection = { 0, 90, 180, 270 };

    private void Start()
    {
        controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
    }

    void Update()
    {
        if (indicator.gameObject.activeSelf)
        {
            indicator.rotation = Quaternion.Euler(centerEyeTransform.rotation.eulerAngles.y + rotationByDirection[direction] - 90, centerEyeTransform.rotation.eulerAngles.y - 90 , 90);
        }
    }

    void OnControlsEnableValueChanged()
    {
        if (controls.isControlsEnabled)
        {
            indicator.gameObject.SetActive(true);
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
