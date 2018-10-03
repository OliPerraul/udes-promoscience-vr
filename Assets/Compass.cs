using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    Transform indicator;

    Vector3 direction = new Vector3(0, 0, 100 * Constants.TILE_SIZE);

    private void Start()
    {
        controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
    }

    void Update()
    {
        if (indicator.gameObject.activeSelf)
        {
            indicator.LookAt(direction);
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
