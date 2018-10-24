using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DistanceScanner : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    GameObject distanceDisplay;

    [SerializeField]
    GameObject targetDisplay;

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
		if(distanceDisplay.activeSelf)
        {
            ExecuteDistanceScan();
        }
	}

    void OnIsControlsEnableValueChangedEvent()
    {
        if (controls.IsControlsEnabled == true)
        {
            distanceDisplay.SetActive(true);
            targetDisplay.SetActive(true);
        }
        else
        {
            distanceDisplay.SetActive(false);
            targetDisplay.SetActive(false);
        }
    }

    void ExecuteDistanceScan()
    {
        int distance = 0;
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
                distance = (int) raycastHit.distance;
                targetDisplay.transform.position = raycastHit.point;
                targetDisplay.transform.rotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal); ;
                targetDisplay.SetActive(true);
            }
        }
        else
        {
            targetDisplay.SetActive(false);
        }

        text.text = distance.ToString();
    }
}
