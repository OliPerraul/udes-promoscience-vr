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

    FloorColor floorColor = FloorColor.Grey;

    public FloorColor GetFloorColorId()
    {
        return floorColor;
    }

    public void PaintFloor()
    {
        if (floorColor == FloorColor.Grey)
        {
            floorColor = FloorColor.Yellow;
            meshRenderer.material = yellowMaterial;
        }
        else if (floorColor == FloorColor.Yellow)
        {
            floorColor = FloorColor.Red;
            meshRenderer.material = redMaterial;
        }
        else if (floorColor == FloorColor.Red)
        {
            floorColor = FloorColor.Grey;
            meshRenderer.material = greyMaterial;
        }
    }

    public void PaintFloorWithColorId(int id)
    {
        if (id == (int) FloorColor.Grey)
        {
            floorColor = FloorColor.Grey;
            meshRenderer.material = greyMaterial;
        }
        else if (id == (int) FloorColor.Yellow)
        {
            floorColor = FloorColor.Yellow;
            meshRenderer.material = yellowMaterial;
        }
        else if (id == (int) FloorColor.Red)
        {
            floorColor = FloorColor.Red;
            meshRenderer.material = redMaterial;
        }
    }
}
