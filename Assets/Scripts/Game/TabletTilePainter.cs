using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletTilePainter : MonoBehaviour
{ 
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    GameLabyrinth labyrinth;

    void Start ()
    {
        action.valueChangedEvent += OnAction;
	}

    void OnAction()
    {
        if(action.value == Constants.ACTION_PAINT_FLOOR)
        {
            PaintCurrentPositionTile();
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
}