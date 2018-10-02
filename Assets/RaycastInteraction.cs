using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    Transform raycastStartPoint;

    FloorPainter lastFloorPainter = null;

    bool isActive = false;
    float raycastRange = 5;// was 10

	void Start ()
    {
        controls.isControlsEnableValueChangedEvent += OnIsControlsEnableValueChangedEvent;
    }
	/*
	void Update ()
    {
		if(isActive)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                TriggerRaycast();
            }
        }
	}*/

    void TriggerRaycast()
    {
        Ray ray = new Ray(raycastStartPoint.position, -raycastStartPoint.forward);
        RaycastHit[] raycastHit = Physics.RaycastAll(ray, raycastRange);
        if (raycastHit.Length > 0)
        {
            bool isFirstFloorHit = false;

            for (int i = 0; i < raycastHit.Length; i++)
            {
                if (raycastHit[i].transform.tag == "Floor")
                {
                    if (!isFirstFloorHit)
                    {
                        isFirstFloorHit = true;
                        FloorPainter floorPainter = raycastHit[i].transform.GetComponent<FloorPainter>();
                        floorPainter.PaintFloor();
                    }
                }
            }
        }
    }

    void OnIsControlsEnableValueChangedEvent()
    {
        if(controls.isControlsEnabled == true)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
}
