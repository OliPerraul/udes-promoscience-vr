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

    public void PaintFloor()
    {
        if (currentColorId == 0)
        {
            currentColorId = 1;
            meshRenderer.material = redMaterial;
        }
        else if (currentColorId == 1)
        {
            currentColorId = 2;
            meshRenderer.material = greenMaterial;
        }
        else if (currentColorId == 2)
        {
            currentColorId = 0;
            meshRenderer.material = greyMaterial;
        }
    }
}
