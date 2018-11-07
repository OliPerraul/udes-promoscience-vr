using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DistanceScanner : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    GameObject distanceDisplay;

    [SerializeField]
    GameObject targetDisplay;

    [SerializeField]
    Text textDisplay;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform raycastStartPoint;

    const string TAG_FLOOR = "Floor";
    const string TAG_WALL = "Wall";

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

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
        float distance = 0;
        string text = "";
        Ray ray = new Ray(raycastStartPoint.position, raycastStartPoint.forward);
        RaycastHit raycastHit;
        
        if (Physics.Raycast(ray, out raycastHit, raycastRange))
        {
            if (raycastHit.transform.tag == TAG_WALL)
            {
                Vector2Int currentPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);
                Vector2Int hitWallPosition = labyrinth.GetWorldPositionInLabyrinthPosition(raycastHit.transform.position.x, raycastHit.transform.position.z);

                if (hitWallPosition.x == currentPosition.x || hitWallPosition.y == currentPosition.y)
                {
                    //Might need a check to see if it's the first wall in line for later when obstacles wont be big block without holes
                    distance = (int)(hitWallPosition - currentPosition).magnitude - 1;
                    text = distance.ToString();
                }
            }
            else if (raycastHit.transform.tag == TAG_FLOOR)
            {
                Vector2Int currentPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);
                Vector2Int hitPosition = labyrinth.GetWorldPositionInLabyrinthPosition(raycastHit.point.x, raycastHit.point.z);

                //Could also check if they are walkable
                if (hitPosition == currentPosition
                        || hitPosition == (currentPosition + new Vector2Int(xByDirection[0], yByDirection[0]))
                        || hitPosition == (currentPosition + new Vector2Int(xByDirection[1], yByDirection[1]))
                        || hitPosition == (currentPosition + new Vector2Int(xByDirection[2], yByDirection[2]))
                        || hitPosition == (currentPosition + new Vector2Int(xByDirection[3], yByDirection[3])))
                {
                    distance = (labyrinth.GetLabyrithEndPosition() - hitPosition).magnitude;
                    distance = Mathf.Round(distance * 10) / 10;
                    text = distance.ToString();
                }
            }

            targetDisplay.transform.position = raycastHit.point;
            targetDisplay.transform.rotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal); ;
            targetDisplay.SetActive(true);
        }
        else
        {
            targetDisplay.SetActive(false);
        }

        textDisplay.text = text;
    }
}
