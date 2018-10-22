using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DistanceScanner : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    GameObject DistanceDisplay;

    [SerializeField]
    Text text;

    [SerializeField]
    Transform raycastStartPoint;

    float raycastRange = 100 * Constants.TILE_SIZE;

	void Start ()
    {
        controls.isControlsEnableValueChangedEvent += OnIsControlsEnableValueChangedEvent;
    }
	
	void Update ()
    {
		if(DistanceDisplay.activeSelf)
        {
            int distance = (int) GetDistanceFromScan();

            text.text = distance.ToString();
        }
	}

    void OnIsControlsEnableValueChangedEvent()
    {
        if (controls.isControlsEnabled == true)
        {
            DistanceDisplay.SetActive(true);
        }
        else
        {
            DistanceDisplay.SetActive(false);
        }
    }

    float GetDistanceFromScan()
    {
        Ray ray = new Ray(raycastStartPoint.position, raycastStartPoint.forward);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, raycastRange))
        {

            if (raycastHit.transform.tag == "Overlay")//Temp
            {
                Debug.Log("Hit Overlay with raycast");
            }

            if (raycastHit.transform.tag != "Overlay")
            {
                return raycastHit.distance;
            }
        }

        return 0;
    }

}
