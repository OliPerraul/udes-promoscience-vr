using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Avatar Controler", order = 1)]
    public class AvatarControllerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<bool> IsPlayerControlsEnabled = new Cirrus.ObservableValue<bool>();

        public bool IsPlayerControlEnabled
        {
            get {
                return IsPlayerControlsEnabled.Value;
            }

            set {
                IsPlayerControlsEnabled.Value = value;
            }
        }

        public Cirrus.ObservableValue<bool> IsControlsEnabled = new Cirrus.ObservableValue<bool>();

        public Cirrus.ObservableValue<int[]> RecordedSteps = new Cirrus.ObservableValue<int[]>();

        public Action stopAllMovementEvent;

        public Action resetPositionAndRotation;

        public Action isControlsEnableValueChangedEvent;

        public Action isPlayerControlsEnableValueChangedEvent;

        public Cirrus.Event OnPlayerReachedTheEndHandler;

        public Cirrus.Event OnLabyrinthPositionChangedHandler;

        public Cirrus.ObservableValue<Vector3> PlayerPosition = new Cirrus.ObservableValue<Vector3>();

        public Cirrus.ObservableValue<Quaternion> PlayerRotation = new Cirrus.ObservableValue<Quaternion>();

        public Cirrus.ObservableValue<Tile> PlayerPaintTile = new Cirrus.ObservableValue<Tile>();

        public Cirrus.ObservableValue<PositionRotationAndTile> PositionRotationAndTiles = new Cirrus.ObservableValue<PositionRotationAndTile>();

        public Cirrus.ObservableValue<TileColor> PaintingColor = new Cirrus.ObservableValue<TileColor>();

        public Cirrus.ObservableValue<Tile[]> PlayerTilesToPaint = new Cirrus.ObservableValue<Tile[]>();

        public Cirrus.ObservableValue<int> ForwardDirection = new Cirrus.ObservableValue<int>();

        public void OnEnable()
        {
            IsControlsEnabled.Value = false;

            IsPlayerControlsEnabled.Value = false;
        } 

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