using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Algorithms
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
        Labyrinths.Skin skin;

        public Material RedMaterial
        {
            get {
                return skin == null ? skin.RedFloorMaterial : redMaterial; 
            }
        }

        public Material YellowMaterial
        {
            get
            {
                return skin == null ? skin.YellowFloorMaterial : yellowMaterial;
            }
        }

        public Material GreyMaterial
        {
            get
            {
                return skin == null ? skin.GreyFloorMaterial : greyMaterial;
            }
        }


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
                meshRenderer.material = YellowMaterial;
            }
            else if (floorColor == TileColor.Yellow)
            {
                floorColor = TileColor.Red;
                meshRenderer.material = RedMaterial;
            }
            else if (floorColor == TileColor.Red)
            {
                floorColor = TileColor.Grey;
                meshRenderer.material = GreyMaterial;
            }
        }

        public void PaintFloorWithColor(TileColor color)
        {
            if (color == TileColor.Grey)
            {
                floorColor = TileColor.Grey;
                meshRenderer.material = GreyMaterial;
            }
            else if (color == TileColor.Yellow)
            {
                floorColor = TileColor.Yellow;
                meshRenderer.material = YellowMaterial;
            }
            else if (color == TileColor.Red)
            {
                floorColor = TileColor.Red;
                meshRenderer.material = RedMaterial;
            }
        }
    }
}
