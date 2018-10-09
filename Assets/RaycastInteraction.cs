using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger fowardDirection;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform raycastStartPoint;

    FloorPainter lastFloorPainter = null;

    bool isActive = false;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    float raycastRange = 5;

	void Start ()
    {
        controls.isControlsEnableValueChangedEvent += OnIsControlsEnableValueChangedEvent;
    }
	
	void Update ()
    {
		if(isActive)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                TriggerDistanceScanner();
            }
        }
	}

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

    void TriggerDistanceScanner()
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;
        int distance = GetStraightLenghtInDirection(posX, posY, fowardDirection.value);
    }

    int GetStraightLenghtInDirection(int posX, int posY, int direction)
    {
        int straightLenght = 0;

        while (labyrinth.GetIsTileWalkable(posX + xByDirection[(direction) % 4], posY + yByDirection[(direction) % 4]))
        {
            straightLenght++;
            posX += xByDirection[direction];
            posY += yByDirection[direction];
        }

        return straightLenght;
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
