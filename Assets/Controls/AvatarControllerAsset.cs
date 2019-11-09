using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Avatar Controler", order = 1)]
    public class AvatarControllerAsset : ScriptableObject
    {
        public Cirrus.MonitoredValue<bool> IsPlayerControlsEnabled = new Cirrus.MonitoredValue<bool>();

        public bool IsPlayerControlEnabled {
            get { return IsPlayerControlsEnabled.Value; }
            set { IsPlayerControlsEnabled.Value = value; }
        }


        public Cirrus.MonitoredValue<bool> IsControlsEnabled = new Cirrus.MonitoredValue<bool>();

        public Cirrus.MonitoredValue<int[]> RecordedSteps = new Cirrus.MonitoredValue<int[]>();

        public Action stopAllMovementEvent;

        public Action resetPositionAndRotation;

        public Action isControlsEnableValueChangedEvent;

        public Action isPlayerControlsEnableValueChangedEvent;

        public Cirrus.Event OnPlayerReachedTheEndHandler;

        public Cirrus.Event OnLabyrinthPositionChangedHandler;

        public Cirrus.MonitoredValue<Vector3> PlayerPosition = new Cirrus.MonitoredValue<Vector3>();

        public Cirrus.MonitoredValue<Quaternion> PlayerRotation = new Cirrus.MonitoredValue<Quaternion>();

        public Cirrus.MonitoredValue<Tile> PlayerPaintTile = new Cirrus.MonitoredValue<Tile>();

        public Cirrus.MonitoredValue<PositionRotationAndTile> PositionRotationAndTiles = new Cirrus.MonitoredValue<PositionRotationAndTile>();

        public Cirrus.MonitoredValue<TileColor> PaintingColor = new Cirrus.MonitoredValue<TileColor>();

        public Cirrus.MonitoredValue<Tile[]> PlayerTilesToPaint = new Cirrus.MonitoredValue<Tile[]>();

        public Cirrus.MonitoredValue<int> ForwardDirection = new Cirrus.MonitoredValue<int>();

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