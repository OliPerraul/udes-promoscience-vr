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

    bool isActive = true;//temp need to be false
    float raycastRange = 10;

	void Start ()
    {
        controls.isControlsEnableValueChangedEvent += OnIsControlsEnableValueChangedEvent;
    }
	

	void Update ()
    {
		if(isActive)
        {
            Ray ray = new Ray(raycastStartPoint.position, raycastStartPoint.forward);
            RaycastHit[] raycastHit = Physics.RaycastAll(ray, raycastRange);
            if (raycastHit.Length > 0 )
            {
                bool isFloorHit = false;

                for (int i = 0; i < raycastHit.Length; i++)
                {
                    Debug.Log("Hit : " + raycastHit.Length);
                    if (raycastHit[i].transform.tag == "Floor")
                    {
                        if (!isFloorHit)
                        {
                            isFloorHit = true;
                            Debug.Log("Hit a first floor");
                            FloorPainter floorPainter = raycastHit[i].transform.GetComponent<FloorPainter>();
                            if (floorPainter != lastFloorPainter)
                            {
                                if (lastFloorPainter != null)
                                {
                                    lastFloorPainter.PaintGrey();
                                }
                                floorPainter.PaintOrange();
                                lastFloorPainter = floorPainter;
                            }
                        }
                        else
                        {
                            Debug.Log("Hit a second floor");
                        }
                    }
                    else
                    {
                        Debug.Log("Hit the crap");
                    }
                }


                if (!isFloorHit)
                {
                    if (lastFloorPainter != null)
                    {
                        lastFloorPainter.PaintGrey();
                        lastFloorPainter = null;
                    }
                }
            }
            else
            {
                if (lastFloorPainter != null)
                {
                    lastFloorPainter.PaintGrey();
                    lastFloorPainter = null;
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
