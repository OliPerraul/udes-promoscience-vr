﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Controls
{

    public class WallDistanceScannerModule : DistanceScannerModule
    {
        private Labyrinths.Piece wall;


        public override void DoScan(ControlsAsset controller, RaycastHit[] hits)
        {
            IEnumerable<RaycastHit> pieces = hits.Where(x => x.collider.GetComponent<Labyrinths.Piece>() != null);

            if (pieces.Count() != 0)
            {
                RaycastHit hit = pieces.OrderBy(x => x.distance).First();

                Labyrinths.Piece newWall = hit.collider.GetComponent<Labyrinths.Piece>();
                if (newWall != null &&
                    newWall != wall)
                {
                    wall = newWall;

                    newWall.Highlight();
                    controller.WallDistance.Value = ((int)hit.distance) / Labyrinths.Utils.TileSize;
                    return;
                }

            }

            //Labyrinths.Piece.RemoveHighlight();
            //controller.WallDistance.Value = -1;

        }
    }

    public class WallDistanceScannerResource : ScannerResource
    {
        public override ToolId ToolId => ToolId.WallDistanceScanner;

        public override DistanceScannerModule CreateModule()
        {
            return new WallDistanceScannerModule();
        }
    }
}