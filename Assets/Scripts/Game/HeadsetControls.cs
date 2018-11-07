using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetControls : MonoBehaviour
{
    [SerializeField]
    ScriptableGameAction action;

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    ScriptableVector3 playerPosition;

    [SerializeField]
    ScriptableQuaternion playerRotation;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptablePositionRotationAndTile playerPositionRotationAndTiles;

    [SerializeField]
    ScriptableTileColor paintingColor;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    bool isChainingMovement = false;
    bool isMoving = false;
    bool isPrimaryTouchpadHold = false;
    bool isPrimaryIndexTriggerHold = false;
    bool isTurningLeft = false;
    bool isTurningRight = false;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

    readonly float[] rotationByDirection = { 0, 90, 180, 270 };

    float primaryIndexTriggerHoldTime = 0;
    float lerpValue = 0;
    float moveSpeed = 0;
    float turnSpeed = 0;

    Vector2Int lastLabyrinthPosition;

    Vector3 fromPosition;
    Vector3 targetPosition;
    Vector3 lastPosition;

    Quaternion fromRotation;
    Quaternion targetRotation;
    Quaternion lastRotation;


    private void Start()
    {
        controls.stopAllMovementEvent += OnStopAllMovement;
        controls.resetPositionAndRotation += OnResetPositionAndRotation;
        playerPositionRotationAndTiles.valueChangedEvent += OnPlayerPositionRotationAndTiles;
    }

    void Update ()
    {

        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            isPrimaryTouchpadHold = true;
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            isPrimaryTouchpadHold = false;
        }

        if (controls.IsControlsEnabled)
        {
            if (isPrimaryTouchpadHold)
            {
                isPrimaryTouchpadHold = true;

                if (!isTurningLeft && !isTurningRight)
                {
                    if (isMoving)
                    {
                        if( lerpValue >= 0.5f && CheckIfMovementIsValidInDirectionFromPosition(forwardDirection.Value, targetPosition))
                        {
                            isChainingMovement = true;
                        }
                    }
                    else
                    {
                        RequestMovementInDirection(forwardDirection.Value);
                    }
                }
            }
            else if (OVRInput.GetDown(OVRInput.Button.Left))
            {
                if (!isMoving)
                {
                    if (isTurningLeft)
                    {
                        isChainingMovement = true;
                    }
                    else if (isTurningRight)
                    {
                        if(isChainingMovement)
                        {
                            isChainingMovement = false;
                        }
                        else
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, -90, 0);
                            fromRotation = targetRotation;
                            targetRotation = fromRotation * trajectory;
                            forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
                            action.Value = GameAction.TurnLeft;
                            lerpValue = 1 - lerpValue;
                            isTurningLeft = true;
                            isTurningRight = false;
                        }
                    }
                    else
                    {
                        CameraTurnLeft();
                    }
                }
            }
            else if (OVRInput.GetDown(OVRInput.Button.Right))
            {
                if (!isMoving)
                {
                    if (isTurningRight)
                    {
                        isChainingMovement = true;
                    }
                    else if (isTurningLeft)
                    {
                        if (isChainingMovement)
                        {
                            isChainingMovement = false;
                        }
                        else
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, 90, 0);
                            fromRotation = targetRotation;
                            targetRotation = fromRotation * trajectory;
                            forwardDirection.Value = (forwardDirection.Value + 1) % 4;
                            action.Value = GameAction.TurnRight;
                            lerpValue = 1 - lerpValue;
                            isTurningLeft = false;
                            isTurningRight = true;
                        }
                    }
                    else
                    {
                        CameraTurnRight();
                    }
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                isPrimaryIndexTriggerHold = true;
                primaryIndexTriggerHoldTime = 0;
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (paintingColor.Value == TileColor.Yellow)
                {
                    paintingColor.Value = TileColor.Red;
                }
                else
                {
                    paintingColor.Value = TileColor.Yellow;
                    PaintCurrentPositionTile();
                }

                isPrimaryIndexTriggerHold = false;
            }

            if(isPrimaryIndexTriggerHold)
            {
                primaryIndexTriggerHoldTime += Time.deltaTime;

                if (primaryIndexTriggerHoldTime >= 1)
                {
                    paintingColor.Value = TileColor.Grey;
                    PaintCurrentPositionTile();
                }
            }

            if (isMoving)
            {
                float xi = ((moveSpeed * moveSpeed)/(-2 * Constants.MOVEMENT_ACCELERATION)) + 1;

                if (isChainingMovement)
                {
                    xi += 1;
                }

                moveSpeed = xi < lerpValue ? moveSpeed - (Time.deltaTime * Constants.MOVEMENT_ACCELERATION) : moveSpeed + (Time.deltaTime * Constants.MOVEMENT_ACCELERATION);
                lerpValue += Time.deltaTime * moveSpeed;

                if (lerpValue >= 1)
                {
                    if(isChainingMovement)
                    {
                        isChainingMovement = false;
                        lerpValue = lerpValue - 1;

                        RequestMovementInDirection(forwardDirection.Value);

                        if (paintingColor.Value != TileColor.Red)
                        {
                            PaintCurrentPositionTile();
                        }

                        cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                    }
                    else
                    {
                        cameraTransform.position = targetPosition;
                        moveSpeed = 0;
                        lerpValue = 0;
                        isMoving = false;

                        if (paintingColor.Value != TileColor.Red)
                        {
                            PaintCurrentPositionTile();
                        }
                    }
                }
                else
                {
                    cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                }
            }
            else if (isTurningLeft || isTurningRight)
            {
                float xi = ((turnSpeed * turnSpeed) / ( -2 * Constants.TURNING_ACCELERATION)) + 1;

                if(isChainingMovement)
                {
                    xi++;
                }

                turnSpeed = xi < lerpValue ? turnSpeed - (Time.deltaTime * Constants.TURNING_ACCELERATION) : turnSpeed + (Time.deltaTime * Constants.TURNING_ACCELERATION);
                lerpValue += Time.deltaTime * turnSpeed;

                if (lerpValue >= 1)
                {
                    if (isChainingMovement)
                    {
                        isChainingMovement = false;
                        lerpValue = lerpValue - 1;

                        if(isTurningLeft)
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, -90, 0);
                            fromRotation = targetRotation;
                            targetRotation = targetRotation * trajectory;

                            forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
                            action.Value = GameAction.TurnLeft;
                        }
                        else if (isTurningRight)
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, 90, 0);
                            fromRotation = targetRotation;
                            targetRotation = targetRotation * trajectory;

                            forwardDirection.Value = (forwardDirection.Value + 1) % 4;
                            action.Value = GameAction.TurnRight;
                        }

                        cameraTransform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                    }
                    else
                    {
                        cameraTransform.rotation = targetRotation;
                        turnSpeed = 0;
                        lerpValue = 0;
                        isTurningLeft = false;
                        isTurningRight = false;
                    }
                }
                else
                {
                    cameraTransform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                }
            }

            Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

            if (labyrinthPosition != lastLabyrinthPosition)
            {
                if (paintingColor.Value == TileColor.Red)
                {
                    PaintTile(lastLabyrinthPosition, TileColor.Red);
                }
                
                lastLabyrinthPosition = labyrinthPosition;
            }
           
        }
    }

    void RequestMovementInDirection(int direction)
    {
        if (CheckIfMovementIsValidInDirectionFromPosition(direction, cameraTransform.position))
        {
            fromPosition = cameraTransform.position;
            targetPosition = fromPosition + (new Vector3(xByDirection[direction] * Constants.TILE_SIZE, 0, -yByDirection[direction] * Constants.TILE_SIZE));

            if (direction == 0)
            {
                action.Value = GameAction.MoveUp;
            }
            else if (direction == 1)
            {
                action.Value = GameAction.MoveRight;
            }
            else if (direction == 2)
            {
                action.Value = GameAction.MoveDown;
            }
            else if (direction == 3)
            {
                action.Value = GameAction.MoveLeft;
            }
            
            isMoving = true;
        }
    }

    private void FixedUpdate()
    {
        if(cameraTransform.position != lastPosition)
        {
            playerPosition.Value = cameraTransform.position;
            lastPosition = cameraTransform.position;
        }

        if (cameraTransform.rotation != lastRotation)
        {
            playerRotation.Value = cameraTransform.rotation;
            lastRotation = cameraTransform.rotation;
        }
    }

    void CameraTurnLeft()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, -90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurningLeft = true;
        forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
        action.Value = GameAction.TurnLeft;
    }

    void CameraTurnRight()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, 90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurningRight = true;
        forwardDirection.Value = (forwardDirection.Value + 1) % 4;
        action.Value = GameAction.TurnRight;
    }

    bool CheckIfMovementIsValidInDirectionFromPosition(int direction, Vector3 position)
    {
        Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(position.x, position.z);

        labyrinthPosition.x += xByDirection[direction];
        labyrinthPosition.y += yByDirection[direction];

        return labyrinth.GetIsTileWalkable(labyrinthPosition);
    }

    void PaintTile(Vector2Int position, TileColor color)
    {
        TileColor tileColor = labyrinth.GetTileColor(position);

        if (color != tileColor)
        {
            GameObject tile = labyrinth.GetTile(position);
            FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();

            if (floorPainter != null)
            {
                floorPainter.PaintFloorWithColor(color);
                action.Value = GameAction.PaintFloor;
                playerPaintTile.SetTile(position, color);
            }
        }
    }

    void PaintTile(int positionX, int positionY, TileColor color)
    {
        Vector2Int position = new Vector2Int(positionX, positionY);

        PaintTile(position, color);
    }

    void PaintCurrentPositionTile()
    {
        Vector2Int position = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

        PaintTile(position, paintingColor.Value);
    }

    void OnStopAllMovement()
    {
        isMoving = false;
        isTurningLeft = false;
        isTurningRight = false;
        isChainingMovement = false;
        isPrimaryTouchpadHold = false;
        lerpValue = 0;
    }

    void OnResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        forwardDirection.Value = labyrinth.GetStartDirection();
        lastLabyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(0, 0);

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

        paintingColor.Value = TileColor.Yellow;
    }

    void OnPlayerPositionRotationAndTiles()
    {
        OnStopAllMovement();

        cameraTransform.position = playerPositionRotationAndTiles.GetPosition();
        lastLabyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(playerPositionRotationAndTiles.GetPosition().x, playerPositionRotationAndTiles.GetPosition().z);

        cameraTransform.rotation = playerPositionRotationAndTiles.GetRotation();

        SetForwardDirectionWithRotation(playerPositionRotationAndTiles.GetRotation());

        Tile[] tiles = playerPositionRotationAndTiles.GetTiles();

        for (int i = 0; i < tiles.Length; i++)
        {
            PaintTile(tiles[i].x, tiles[i].y, tiles[i].color);
        }

        paintingColor.Value = TileColor.Yellow;

        PaintCurrentPositionTile();
    }

    void SetForwardDirectionWithRotation(Quaternion rotation)
    {
        float y = rotation.eulerAngles.y;
        float epsilon = 1;

        if(y < rotationByDirection[0] + epsilon && y > rotationByDirection[0] - epsilon)
        {
            forwardDirection.Value = 0;
        }
        else if (y < rotationByDirection[1] + epsilon && y > rotationByDirection[1] - epsilon)
        {
            forwardDirection.Value = 1;
        }
        else if (y < rotationByDirection[2] + epsilon && y > rotationByDirection[2] - epsilon)
        {
            forwardDirection.Value = 2;
        }
        else if (y < rotationByDirection[3] + epsilon && y > rotationByDirection[3] - epsilon)
        {
            forwardDirection.Value = 3;
        }
    }
}
