using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Avatar Controler", order = 1)]
    public class AvatarControllerAsset : ScriptableObject
    {
        public Cirrus.NotifyChangeValue<bool> IsPlayerControlsEnabled = new Cirrus.NotifyChangeValue<bool>();

        public bool IsPlayerControlEnabled {
            get { return IsPlayerControlsEnabled.Value; }
            set { IsPlayerControlsEnabled.Value = value; }
        }


        public Cirrus.NotifyChangeValue<bool> IsControlsEnabled = new Cirrus.NotifyChangeValue<bool>();

        public Cirrus.NotifyChangeValue<int[]> RecordedSteps = new Cirrus.NotifyChangeValue<int[]>();

        public Action stopAllMovementEvent;

        public Action resetPositionAndRotation;

        public Action isControlsEnableValueChangedEvent;

        public Action isPlayerControlsEnableValueChangedEvent;

        public Cirrus.Event OnPlayerReachedTheEndHandler;

        public Cirrus.Event OnLabyrinthPositionChangedHandler;

        public Cirrus.Event OnReturnToDivergencePointRequestHandler;

        public Cirrus.NotifyChangeValue<Vector3> PlayerPosition = new Cirrus.NotifyChangeValue<Vector3>();

        public Cirrus.NotifyChangeValue<Quaternion> PlayerRotation = new Cirrus.NotifyChangeValue<Quaternion>();

        public Cirrus.NotifyChangeValue<Tile> PlayerPaintTile = new Cirrus.NotifyChangeValue<Tile>();

        public Cirrus.NotifyChangeValue<PositionRotationAndTile> PositionRotationAndTiles = new Cirrus.NotifyChangeValue<PositionRotationAndTile>();

        public Cirrus.NotifyChangeValue<TileColor> PaintingColor = new Cirrus.NotifyChangeValue<TileColor>();

        public Cirrus.NotifyChangeValue<Tile[]> PlayerTilesToPaint = new Cirrus.NotifyChangeValue<Tile[]>();

        public Cirrus.NotifyChangeValue<bool> ReturnToDivergencePointAnswer = new Cirrus.NotifyChangeValue<bool>();

        public Cirrus.NotifyChangeValue<int> ForwardDirection = new Cirrus.NotifyChangeValue<int>();

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