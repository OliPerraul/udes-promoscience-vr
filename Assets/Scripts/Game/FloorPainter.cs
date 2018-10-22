using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPainter : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    [SerializeField]
    Material greyMaterial;

    [SerializeField]
    Material yellowMaterial;

    [SerializeField]
    Material redMaterial;

    TileColor floorColor = TileColor.Grey;

    public TileColor GetFloorColorId()
    {
        return floorColor;
    }

    public void PaintFloor()
    {
        if (floorColor == TileColor.Grey)
        {
            floorColor = TileColor.Yellow;
            meshRenderer.material = yellowMaterial;
        }
        else if (floorColor == TileColor.Yellow)
        {
            floorColor = TileColor.Red;
            meshRenderer.material = redMaterial;
        }
        else if (floorColor == TileColor.Red)
        {
            floorColor = TileColor.Grey;
            meshRenderer.material = greyMaterial;
        }
    }

    public void PaintFloorWithColorId(int id)
    {
        if (id == (int) TileColor.Grey)
        {
            floorColor = TileColor.Grey;
            meshRenderer.material = greyMaterial;
        }
        else if (id == (int) TileColor.Yellow)
        {
            floorColor = TileColor.Yellow;
            meshRenderer.material = yellowMaterial;
        }
        else if (id == (int) TileColor.Red)
        {
            floorColor = TileColor.Red;
            meshRenderer.material = redMaterial;
        }
    }
}
