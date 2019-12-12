using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public abstract class ScannerResource : ScriptableObject
    {
        public abstract ToolId ToolId { get; }

        public abstract DistanceScannerModule CreateModule();
    }

    public static class ScannerUtils
    {
        public const float RaycastRange = 100f;

        public const float MaxTileDistance = 10f;
    }

    public abstract class DistanceScannerModule
    {
        public abstract void DoScan(HeadsetToolManagerAsset tools, RaycastHit[] hits);
    }

    public class ScannerTool : HeadsetTool
    {
        public override ToolId Id => resource.ToolId;

        [SerializeField]
        private ScannerResource resource;

        [SerializeField]
        private ControlsAsset controls;

        [SerializeField]
        private Transform origin;

        [SerializeField]
        private TMPro.TextMeshProUGUI distanceText;

        private DistanceScannerModule module;



        [SerializeField]
        private HeadsetToolManagerAsset tools;



        // Start is called before the first frame update
        public void Awake()
        {
            tools.ScannedDistance.OnValueChangedHandler += OnDistance;
            tools.CurrentTool.OnValueChangedHandler += OnCurrentToolChanged;
            module = resource.CreateModule();
        }

        public void OnCurrentToolChanged(ToolId id)
        {
            Algorithms.FloorPainter.RemoveHighlight();
            Labyrinths.Piece.RemoveHighlight();
        }


        public void FixedUpdate()
        {
            if (
            Client.Instance.Labyrinth.Value == null ||
            module == null)
            {
                tools.ScannedDistance.Value = -1;
                return;
            }

            Ray ray = new Ray(origin.position, origin.forward);

            var res = Physics.RaycastAll(ray, ScannerUtils.RaycastRange);

            module.DoScan(tools, res);
        }

        public override void OnValidate()
        {
            base.OnValidate();
        }

        public void OnDistance(float distance)
        {
            distanceText.text = distance < 0 ? "?" : distance.ToString() + " m";
        }

    }
}