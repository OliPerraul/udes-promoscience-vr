using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetControls : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    ScriptableString straightLength;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    bool isMoving = false;
    bool isTurning = false;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

    float lerpValue = 0;

    Vector3 targetPosition;
    Vector3 fromPosition;

    Quaternion fromRotation;
    Quaternion targetRotation;

    private void Start()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.resetPositionAndRotation += ResetPositionAndRotation;
    }

    void Update ()
    {
       if (controls.IsControlsEnabled)
        {
            if (isMoving)
            {
                lerpValue += Time.deltaTime * Constants.MOVEMENT_SPEED;

                if (lerpValue >= 1)
                {
                    cameraTransform.position = targetPosition;
                    lerpValue = 0;
                    isMoving = false;
                }
                else
                {
                    cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                }
            }
            else if (isTurning)
            {
                lerpValue += Time.deltaTime * Constants.TURNING_SPEED;

                if (lerpValue >= 1)
                {
                    cameraTransform.rotation = targetRotation;
                    lerpValue = 0;
                    isTurning = false;
                }
                else
                {
                    cameraTransform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                }
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                RequestMovementInDirection(forwardDirection.Value);
            }
            else if (OVRInput.GetDown(OVRInput.Button.Left))
            {
                CameraTurnLeft();
            }
            else if (OVRInput.GetDown(OVRInput.Button.Right))
            {
                CameraTurnRight();
            }
            else if (OVRInput.GetDown(OVRInput.Button.Back))
            {
                PaintCurrentPositionTile();
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                TriggerDistanceScanner();
            }
        }
    }

    void RequestMovementInDirection(int direction)
    {
        if (CheckIfMovementIsValid(direction))
        {
            fromPosition = cameraTransform.position;

            if (direction == 0)
            {
                targetPosition = fromPosition + (new Vector3(0, 0, Constants.TILE_SIZE));
                action.Value = Constants.ACTION_MOVE_UP;
            }
            else if (direction == 1)
            {
                targetPosition = fromPosition + (new Vector3(Constants.TILE_SIZE, 0, 0));
                action.Value = Constants.ACTION_MOVE_RIGHT;
            }
            else if (direction == 2)
            {
                targetPosition = fromPosition + (new Vector3(0, 0, -Constants.TILE_SIZE));
                action.Value = Constants.ACTION_MOVE_DOWN;
            }
            else if (direction == 3)
            {
                targetPosition = fromPosition + (new Vector3(-Constants.TILE_SIZE, 0, 0));
                action.Value = Constants.ACTION_MOVE_LEFT;
            }
            
            isMoving = true;
        }
    }

    void CameraTurnLeft()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, -90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurning = true;
        forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
        action.Value = Constants.ACTION_TURN_LEFT;
    }

    void CameraTurnRight()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, 90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurning = true;
        forwardDirection.Value = (forwardDirection.Value + 1) % 4;
        action.Value = Constants.ACTION_TURN_RIGHT;
    }


    bool CheckIfMovementIsValid(int direction)
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;

        posX += xByDirection[direction];
        posY += yByDirection[direction];

        return labyrinth.GetIsTileWalkable(posX,posY);
    }
   
    void PaintCurrentPositionTile()
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;
        GameObject tile = labyrinth.GetTile(posX, posY);
        FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();
        if(floorPainter != null)
        {
            floorPainter.PaintFloor();
            action.Value = Constants.ACTION_PAINT_FLOOR;
        }
    }

    void TriggerDistanceScanner()
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;
        int length = GetStraightLengthInDirection(posX, posY, forwardDirection.Value);
        if(length < 10)
        {
            straightLength.Value = "0" + length;
        }
        else
        {
            straightLength.Value = length.ToString();
        }

        action.Value = Constants.ACTION_DISTANCE_SCANNER;
    }

    int GetStraightLengthInDirection(int posX, int posY, int direction)
    {
        int length = 0;

        while (labyrinth.GetIsTileWalkable(posX + xByDirection[(direction) % 4], posY + yByDirection[(direction) % 4]))
        {
            length++;
            posX += xByDirection[direction];
            posY += yByDirection[direction];
        }

        return length;
    }


    void StopAllMovement()
    {
        isMoving = false;
        isTurning = false;
        lerpValue = 0;
    }

    void ResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        forwardDirection.Value = labyrinth.GetStartDirection();

        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        if (forwardDirection.Value == 1)
        {
            rotation.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (forwardDirection.Value == 2)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (forwardDirection.Value == 3)
        {
            rotation.eulerAngles = new Vector3(0, 270, 0);
        }

        cameraTransform.rotation = rotation;
    }
}
