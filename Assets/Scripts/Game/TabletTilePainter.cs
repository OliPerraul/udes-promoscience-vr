using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//To be remove
public class TabletTilePainter : MonoBehaviour
{ 
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    ScriptableString straightLength;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

    void Start ()
    {
        action.valueChangedEvent += OnAction;
	}

    void OnAction()
    {
        if (action.value == Constants.ACTION_PAINT_FLOOR)
        {
            PaintCurrentPositionTile();
        }
        else if (action.value == Constants.ACTION_DISTANCE_SCANNER)
        {
            TriggerDistanceScanner();
        }
    }

    void PaintCurrentPositionTile()
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;
        GameObject tile = labyrinth.GetTile(posX, posY);
        FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();
        if (floorPainter != null)
        {
            floorPainter.PaintFloor();
        }
    }

    void TriggerDistanceScanner()
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;
        int length = GetStraightLengthInDirection(posX, posY, forwardDirection.value);
        if (length < 10)
        {
            straightLength.value = "0" + length;
        }
        else
        {
            straightLength.value = length.ToString();
        }
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
}