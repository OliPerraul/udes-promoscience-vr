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
        private GameObject yellowTrail;

        [SerializeField]
        private GameObject redTrail;

        [SerializeField]
        TileColor floorColor;

        public void SetFloorColor(TileColor value, bool paintFloor = false)
        {
            floorColor = value;
            switch (floorColor)
            {
                case TileColor.Grey:
                case TileColor.NoColor:
                    redTrail.SetActive(false);
                    yellowTrail.SetActive(false);
                    if (paintFloor)
                    {
                        meshRenderer.material = greyMaterial;
                    }
                    break;

                case TileColor.Red:
                    yellowTrail.SetActive(false);
                    if (paintFloor)
                    {
                        meshRenderer.material = redMaterial;
                    }
                    else
                    {
                        redTrail.SetActive(true);
                    }
                    break;

                case TileColor.Yellow:
                    redTrail.SetActive(false);
                    if (paintFloor)
                    {
                        meshRenderer.material = yellowMaterial;
                    }
                    else
                    {
                        yellowTrail.SetActive(true);
                    }

                    break;
            }
        }

        public TileColor GetFloorColor()
        {
            return floorColor;
        }

        public void PaintFloor(bool paintfloor=false)
        {
            if (floorColor == TileColor.Grey)
            {
                SetFloorColor(TileColor.Yellow, paintfloor);
            }
            else if (floorColor == TileColor.Yellow)
            {
                SetFloorColor(TileColor.Red, paintfloor);
            }
            else if (floorColor == TileColor.Red)
            {
                SetFloorColor(TileColor.Grey, paintfloor);
            }
        }

        public void PaintFloorWithColor(TileColor color, bool paintfloor = false)
        {
            if (color == TileColor.Grey)
            {
                SetFloorColor(TileColor.Grey, paintfloor);
            }
            else if (color == TileColor.Yellow)
            {
                SetFloorColor(TileColor.Yellow, paintfloor);
            }
            else if (color == TileColor.Red)
            {
                SetFloorColor(TileColor.Red, paintfloor);
            }
        }
    }
}
