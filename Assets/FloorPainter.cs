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
    Material orangeMaterial;

    public void PaintGrey()
    {
        meshRenderer.material = greyMaterial;
    }
    public void PaintOrange()
    {
        meshRenderer.material = orangeMaterial;
    }
}
