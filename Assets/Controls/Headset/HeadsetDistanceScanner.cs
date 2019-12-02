using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UdeS.Promoscience.Controls
{
    public static class ScannerUtils
    {
        public const float RaycastRange = 100f;

        public const float MaxTileDistance = 10f;
    }

    public abstract class DistanceScannerModule
    {
        public abstract void DoScan(AvatarControllerAsset controller, RaycastHit[] hits);
    }

    public class FlightDistanceScannerModule : DistanceScannerModule
    {
        public override void DoScan(AvatarControllerAsset controller, RaycastHit[] hits)
        {
            IEnumerable<RaycastHit> floors = hits.Where(x => x.collider.GetComponentInChildren<Algorithms.FloorPainter>() != null);
            if (floors.Count() != 0)
            {
                RaycastHit hit = floors.OrderBy(x => x.distance).First();

                if (hit.distance < ScannerUtils.MaxTileDistance)
                {
                    // TODO: This is a quick solution (looking for floor in asset name lol)
                    // Bad practice remove
                    // Unity dosent allow for multitag..
                    var piece = hit.collider.GetComponentInParent<Labyrinths.Piece>();

                    if (piece != null &&
                        (piece.gameObject.name.Contains("Floor") ||
                        piece.gameObject.name.Contains("Start") ||
                        piece.gameObject.name.Contains("End")))
                    {
                        Algorithms.FloorPainter floor = hit.collider.GetComponentInChildren<Algorithms.FloorPainter>();
                        if (floor != null)
                        {
                            float dist =
                                (Client.Instance.Labyrinth.Value.GetLabyrithEndPosition() -
                                Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(hit.point.x, hit.point.z)).magnitude;

                            floor.Highlight();
                            controller.FlightDistance.Value = ((int)dist) / Labyrinths.Utils.TileSize;
                            return;
                        }                        
                    }
                }
            }

            controller.FlightDistance.Value = -1;
            Algorithms.FloorPainter.RemoveHighlight();
            
        }
    }

    public class WallDistanceScannerModule : DistanceScannerModule
    {
        public override void DoScan(AvatarControllerAsset controller, RaycastHit[] hits)
        {
            IEnumerable<RaycastHit> pieces = hits.Where(x => x.collider.GetComponentInChildren<Labyrinths.Piece>() != null);

            if (pieces.Count() != 0)
            {
                RaycastHit hit = pieces.OrderBy(x => x.distance).First();

                controller.WallDistance.Value = ((int)hit.distance) / Labyrinths.Utils.TileSize;

                Labyrinths.Piece piece = hit.collider.GetComponentInChildren<Labyrinths.Piece>();
                if (piece != null)
                {
                    piece.Highlight();
                }

            }
            else
            {
                controller.WallDistance.Value = -1;
            }
        }
    }



    public class HeadsetDistanceScanner : MonoBehaviour
    {
        [SerializeField]
        private HeadsetCameraRig cameraRig;

        [SerializeField]
        private AvatarControllerAsset controller;

        [SerializeField]
        private HeadsetToolManagerAsset tools;

        private DistanceScannerModule module;


        public void Awake()
        {
            Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithmChanged;
            tools.CurrentEquipment.OnValueChangedHandler += OnToolChanged;
        }

        public void OnToolChanged(ToolType tool)
        {
            if (tool == ToolType.DistanceScanner)
            {
                OnAlgorithmChanged(Client.Instance.Algorithm.Value);
            }
            else
            {
                Disable();
            }
        }

        public void Disable()
        {
            Algorithms.FloorPainter.RemoveHighlight();
            Labyrinths.Piece.RemoveHighlight();
            enabled = false;
        }

        public void OnAlgorithmChanged(Algorithms.Algorithm algorithm)
        {
            if (algorithm == null)
                return;

            switch (algorithm.Id)
            {
                case Algorithms.Id.LongestStraight:
                    enabled = true;
                    module = new WallDistanceScannerModule();
                    break;

                case Algorithms.Id.ShortestFlightDistance:
                    enabled = true;
                    module = new FlightDistanceScannerModule();
                    break;

                default:
                    Disable();
                    module = null;
                    break;
            }
        }

        public void FixedUpdate()
        {
            if (cameraRig == null)
                return;

            if(cameraRig == null ||
            Client.Instance.Labyrinth.Value == null ||
            module == null)
            {
                controller.WallDistance.Value = -1;
                controller.FlightDistance.Value = -1;
                return;
            }

            Ray ray = new Ray(cameraRig.PointerTransform.position, cameraRig.PointerTransform.forward);

            var res = Physics.RaycastAll(ray, ScannerUtils.RaycastRange);

            module.DoScan(controller, res);
        }
    }
}