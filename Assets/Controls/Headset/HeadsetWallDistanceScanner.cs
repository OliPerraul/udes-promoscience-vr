//using UnityEngine;
//using System.Collections;
//using System.Linq;
//using System.Collections.Generic;

//namespace UdeS.Promoscience.Controls
//{
//    public class HeadsetWallDistanceScanner : MonoBehaviour
//    {
//        [SerializeField]
//        private HeadsetCameraRig cameraRig;

//        [SerializeField]
//        private AvatarControllerAsset controller;

//        private const float RaycastRange = 100f;

//        private const float MaxTileDistance = 10f;

//        public void FixedUpdate()
//        {
//            if (cameraRig == null)
//                return;

//            if (Client.Instance.Labyrinth.Value == null)
//            {
//                controller.WallDistance.Value = -1;
//                controller.FlightDistance.Value = -1;
//                return;
//            }

//            Ray ray = new Ray(cameraRig.PointerTransform.position, cameraRig.PointerTransform.forward);

//            var res = Physics.RaycastAll(ray, RaycastRange);
//            IEnumerable<RaycastHit> pieces = res.Where(x => x.collider.GetComponentInChildren<Labyrinths.Piece>() != null);

//            if (pieces.Count() != 0)
//            {
//                float dist = pieces.Min(x => x.distance);
//                controller.WallDistance.Value = ((int)dist) / Labyrinths.Utils.TileSize;
//            }
//            else
//            {
//                controller.WallDistance.Value = -1;
//            }


//            IEnumerable<RaycastHit> floors = res.Where(x => x.collider.GetComponentInChildren<Algorithms.FloorPainter>() != null);
//            if (floors.Count() != 0)
//            {
//                RaycastHit hit = floors.OrderBy(x => x.distance).First();

//                if (hit.distance > MaxTileDistance)
//                {
//                    controller.FlightDistance.Value = -1;
//                    Algorithms.FloorPainter.RemoveHighlight();
//                }
//                else
//                {
//                    float dist =
//                        (Client.Instance.Labyrinth.Value.GetLabyrithEndPosition() -
//                        Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(hit.point.x, hit.point.z)).magnitude;

//                    Algorithms.FloorPainter floor = hit.collider.GetComponentInChildren<Algorithms.FloorPainter>();
//                    if (floor != null)
//                    {
//                        floor.Highlight();
//                    }

//                    controller.FlightDistance.Value = ((int)dist) / Labyrinths.Utils.TileSize;
//                }
//            }
//            else
//            {
//                controller.FlightDistance.Value = -1;
//                Algorithms.FloorPainter.RemoveHighlight();
//            }


//        }

//    }
//}