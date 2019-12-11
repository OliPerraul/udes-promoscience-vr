using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.Characters;

namespace UdeS.Promoscience.Controls
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Avatar Controler", order = 1)]
    public class ControlsAsset : ScriptableObject
    {


        public Cirrus.ObservableValue<bool> IsPlayerControlsEnabled = new Cirrus.ObservableValue<bool>(true);

        public Cirrus.ObservableValue<bool> IsThirdPersonEnabled = new Cirrus.ObservableValue<bool>(false);

        public Cirrus.ObservableValue<bool> IsTransitionCameraEnabled = new Cirrus.ObservableValue<bool>(false);

        public Cirrus.ObservableValue<bool> IsControlsEnabled = new Cirrus.ObservableValue<bool>(true);

        public Cirrus.ObservableValue<bool> IsCursorLocked = new Cirrus.ObservableValue<bool>(false);

        public Cirrus.ObservableValue<int[]> RecordedSteps = new Cirrus.ObservableValue<int[]>();

        public Action stopAllMovementEvent;

        public Action resetPositionAndRotation;

        public Action isControlsEnableValueChangedEvent;

        public Action isPlayerControlsEnableValueChangedEvent;

        public Cirrus.Event OnPlayerReachedTheEndHandler;

        public Cirrus.Event OnLabyrinthPositionChangedHandler;

        public Cirrus.ObservableValue<Quaternion> CameraRotation = new Cirrus.ObservableValue<Quaternion>();

        public Cirrus.ObservableValue<Vector3> PlayerPosition = new Cirrus.ObservableValue<Vector3>();

        public Cirrus.ObservableValue<Quaternion> PlayerRotation = new Cirrus.ObservableValue<Quaternion>();

        public Cirrus.ObservableValue<Tile> PlayerPaintTile = new Cirrus.ObservableValue<Tile>();

        public Cirrus.ObservableValue<PositionRotationAndTile> PositionRotationAndTiles = new Cirrus.ObservableValue<PositionRotationAndTile>();

        public Cirrus.ObservableValue<TileColor> PaintingColor = new Cirrus.ObservableValue<TileColor>();

        public Cirrus.ObservableValue<Tile[]> PlayerTilesToPaint = new Cirrus.ObservableValue<Tile[]>();

        public Cirrus.ObservableValue<int> ForwardDirection = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<AvatarAnimation> Animation = new Cirrus.ObservableValue<AvatarAnimation>();

        public Cirrus.ObservableValue<float> WallDistance = new Cirrus.ObservableValue<float>();

        public Cirrus.ObservableValue<float> ScannedDistance = new Cirrus.ObservableValue<float>();


        public void StopAllMovement()
        {
            if (stopAllMovementEvent != null)
            {
                stopAllMovementEvent();
            }
        }

        public void ResetPositionAndRotation()
        {
            if (resetPositionAndRotation != null)
            {
                resetPositionAndRotation();
            }
        }
    }
}