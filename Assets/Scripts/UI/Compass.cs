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
            indicator.localRotation = Quaternion.Euler(centerEyeTransform.rotation.eulerAngles.y + -centerEyeTransform.rotation.eulerAngles.z + rotationByDirection[direction] - 90, -90, 90);
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
