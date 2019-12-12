using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Controls
{
    public class FlightDistanceScannerModule : DistanceScannerModule
    {
        private Algorithms.FloorPainter floor;

        public override void DoScan(HeadsetToolManagerAsset tools, RaycastHit[] hits)
        {
            IEnumerable<RaycastHit> floors = hits.Where(x => x.collider.GetComponentInChildren<Algorithms.FloorPainter>() != null);
            if (floors.Count() != 0)
            {
                RaycastHit hit = floors.OrderBy(x => x.distance).First();

                if (hit.distance < ScannerUtils.MaxTileDistance)
                {
                    // TODO: This is a quick solution (looking for floor in asset name)
                    // Bad practice remove
                    // Unity dosent allow for multitag..
                    var piece = hit.collider.GetComponentInParent<Labyrinths.Piece>();

                    if (piece != null &&
                        (piece.gameObject.name.Contains("Floor") ||
                        piece.gameObject.name.Contains("Start") ||
                        piece.gameObject.name.Contains("End")))
                    {
                        Algorithms.FloorPainter newfloor = hit.collider.GetComponentInChildren<Algorithms.FloorPainter>();

                        if (newfloor != null &&
                            newfloor != floor)
                        {
                            floor = newfloor;

                            float dist =
                                    (Client.Instance.Labyrinth.Value.GetLabyrithEndPosition() -
                                    Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(hit.point.x, hit.point.z)).magnitude;

                            floor.Highlight();
                            tools.ScannedDistance.Value = ((int)dist) / Labyrinths.Utils.TileSize;
                            return;

                        }
                    }
                }
            }

            //controller.FlightDistance.Value = -1;
            //Algorithms.FloorPainter.RemoveHighlight();

        }
    }

    public class FlightDistanceScannerResource : ScannerResource
    {
        public override ToolId ToolId => ToolId.FlightDistanceScanner;

        public override DistanceScannerModule CreateModule()
        {
            return new FlightDistanceScannerModule();
        }
    }
}