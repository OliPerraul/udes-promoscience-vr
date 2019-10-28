using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Algorithms
{
    public class DistanceScanner : MonoBehaviour
    {
        [SerializeField]
        private LocalizeInlineString distanceText;

        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        GameObject distanceDisplay;

        [SerializeField]
        GameObject targetDisplay;

        [SerializeField]
        Text textDisplay;

        [SerializeField]
        Controls.CameraRigWrapper cameraRig;

        [SerializeField]
        Transform raycastStartPoint;

        const string TAG_FLOOR = "Floor";
        const string TAG_WALL = "Wall";

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        float raycastRange = 100 * Promoscience.Utils.TILE_SIZE;

        private bool init = false;

        void OnEnable()
        {
            if (init) return;

            init = true;

            Client.Instance.clientStateChangedEvent += OnClientStateChanged;
            OnClientStateChanged();


            //controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    if (Client.Instance.Algorithm.Id == Promoscience.Algorithms.Id.LongestStraight ||
                        Client.Instance.Algorithm.Id == Promoscience.Algorithms.Id.ShortestFlightDistance)
                    {
                        distanceDisplay.SetActive(true);
                        targetDisplay.SetActive(true);
                    }
                    else
                    {
                        distanceDisplay.SetActive(false);
                        targetDisplay.SetActive(false);
                    }
                    break;

                default:
                    distanceDisplay.SetActive(false);
                    targetDisplay.SetActive(false);
                    break;

            }
        }

        void Update()
        {
            if (distanceDisplay.activeSelf)
            {
                ExecuteDistanceScan();
            }
        }


        void ExecuteDistanceScan()
        {
            float distance = 0;
            string text = "<color=red>" + "?" + "</color>";
            Ray ray = new Ray(raycastStartPoint.position, raycastStartPoint.forward);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, raycastRange))
            {
                if (raycastHit.transform.tag == TAG_WALL)
                {
                    Vector2Int currentPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z);
                    Vector2Int hitWallPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(raycastHit.transform.position.x, raycastHit.transform.position.z);

                    if (hitWallPosition.x == currentPosition.x || hitWallPosition.y == currentPosition.y)
                    {
                        bool isFirstWallInLine = true;

                        if (hitWallPosition.x == currentPosition.x)
                        {
                            int direction = (hitWallPosition.y - currentPosition.y) < 0 ? 0 : 2;
                            int y = currentPosition.y + yByDirection[direction];

                            while (y != hitWallPosition.y)
                            {
                                if (!Client.Instance.Labyrinth.GetIsTileWalkable(currentPosition.x, y))
                                {
                                    isFirstWallInLine = false;
                                    break;
                                }

                                y += yByDirection[direction];
                            }
                        }
                        else if (hitWallPosition.y == currentPosition.y)
                        {
                            int direction = (hitWallPosition.x - currentPosition.x) < 0 ? 3 : 1;
                            int x = currentPosition.x + xByDirection[direction];

                            while (x != hitWallPosition.x)
                            {
                                if (!Client.Instance.Labyrinth.GetIsTileWalkable(x, currentPosition.y))
                                {
                                    isFirstWallInLine = false;
                                    break;
                                }

                                x += xByDirection[direction];
                            }
                        }

                        if (isFirstWallInLine)
                        {
                            distance = (int)(hitWallPosition - currentPosition).magnitude - 1;
                            text = "<color=cyan>" + distance.ToString() + "</color>";
                        }
                    }
                }
                else if (raycastHit.transform.tag == TAG_FLOOR)
                {
                    Vector2Int currentPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z);
                    Vector2Int hitPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(raycastHit.point.x, raycastHit.point.z);

                    if (hitPosition == currentPosition
                            || hitPosition == (currentPosition + new Vector2Int(xByDirection[0], yByDirection[0]))
                            || hitPosition == (currentPosition + new Vector2Int(xByDirection[1], yByDirection[1]))
                            || hitPosition == (currentPosition + new Vector2Int(xByDirection[2], yByDirection[2]))
                            || hitPosition == (currentPosition + new Vector2Int(xByDirection[3], yByDirection[3])))
                    {
                        distance = (Client.Instance.Labyrinth.GetLabyrithEndPosition() - hitPosition).magnitude;
                        distance = Mathf.Round(distance * 10) / 10;
                        text = " <color=lime>" + distance.ToString() + "</color>";
                    }
                }

                targetDisplay.transform.position = raycastHit.point;
                targetDisplay.transform.rotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal); ;
                targetDisplay.SetActive(true);
            }
            else
            {
                targetDisplay.SetActive(false);
            }

            textDisplay.text = distanceText.Value + text;
        }
    }
}
