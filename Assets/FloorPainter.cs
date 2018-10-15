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
    Material redMaterial;

    [SerializeField]
    Material greenMaterial;

    int currentColorId = 0;

    public int GetFloorColorId()
    {
        return currentColorId;
    }

    public void PaintFloor()
    {
        if (currentColorId == Constants.GREY_COLOR_ID)
        {
            currentColorId = Constants.RED_COLOR_ID;
            meshRenderer.material = redMaterial;
        }
        else if (currentColorId == Constants.RED_COLOR_ID)
        {
            currentColorId = Constants.GREEN_COLOR_ID;
            meshRenderer.material = greenMaterial;
        }
        else if (currentColorId == Constants.GREEN_COLOR_ID)
        {
            currentColorId = Constants.GREY_COLOR_ID;
            meshRenderer.material = greyMaterial;
        }
    }

    public void PaintFloorWithColorId(int id)
    {
        if (id == Constants.GREY_COLOR_ID)
        {
            currentColorId = id;
            meshRenderer.material = greyMaterial;
        }
        else if (id == Constants.RED_COLOR_ID)
        {
            currentColorId = id;
            meshRenderer.material = redMaterial;
        }
        else if (id == Constants.GREEN_COLOR_ID)
        {
            currentColorId = id;
            meshRenderer.material = greenMaterial;
        }
    }
}
