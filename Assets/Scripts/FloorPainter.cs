using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{
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

        [SerializeField]
        TileColor floorColor;

        public TileColor GetFloorColor()
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

        public void PaintFloorWithColor(TileColor color)
        {
            if (color == TileColor.Grey)
            {
                floorColor = TileColor.Grey;
                meshRenderer.material = greyMaterial;
            }
            else if (color == TileColor.Yellow)
            {
                floorColor = TileColor.Yellow;
                meshRenderer.material = yellowMaterial;
            }
            else if (color == TileColor.Red)
            {
                floorColor = TileColor.Red;
                meshRenderer.material = redMaterial;
            }
        }
    }
}
