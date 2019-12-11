using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public abstract  class ScannerResource : ScriptableObject
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
        public abstract void DoScan(ControlsAsset controller, RaycastHit[] hits);
    }

    //public class HeadsetDistanceScanner : MonoBehaviour
    //{
    //    [SerializeField]
    //    private HeadsetCameraRig cameraRig;

    //    [SerializeField]
    //    private ControlsAsset controller;

    //    [SerializeField]
    //    private HeadsetToolManagerAsset tools;

    //    private DistanceScannerModule module;


    //    public void Awake()
    //    {
    //        Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithmChanged;
    //        tools.CurrentEquipment.OnValueChangedHandler += OnToolChanged;
    //    }

    //    public void OnToolChanged(ToolId tool)
    //    {
    //        if (tool == ToolId.FlightDistanceScanner)
    //        {
    //            OnAlgorithmChanged(Client.Instance.Algorithm.Value);
    //        }
    //        else
    //        {
    //            Disable();
    //        }
    //    }

    //    public void Disable()
    //    {
    //        Algorithms.FloorPainter.RemoveHighlight();
    //        Labyrinths.Piece.RemoveHighlight();
    //        enabled = false;
    //    }

    //    public void OnAlgorithmChanged(Algorithms.Algorithm algorithm)
    //    {
    //        if (algorithm == null)
    //            return;

    //        switch (algorithm.Id)
    //        {
    //            case Algorithms.Id.LongestStraight:
    //                enabled = true;
    //                module = new WallDistanceScannerModule();
    //                break;

    //            case Algorithms.Id.ShortestFlightDistance:
    //                enabled = true;
    //                module = new FlightDistanceScannerModule();
    //                break;

    //            default:
    //                Disable();
    //                module = null;
    //                break;
    //        }
    //    }

    //    public void FixedUpdate()
    //    {
    //        if (cameraRig == null)
    //            return;

    //        if (cameraRig == null ||
    //        Client.Instance.Labyrinth.Value == null ||
    //        module == null)
    //        {
    //            controller.WallDistance.Value = -1;
    //            controller.FlightDistance.Value = -1;
    //            return;
    //        }

    //        Ray ray = new Ray(cameraRig.PointerTransform.position, cameraRig.PointerTransform.forward);

    //        var res = Physics.RaycastAll(ray, ScannerUtils.RaycastRange);

    //        module.DoScan(controller, res);
    //    }
    //}


    public class ScannerTool : HeadsetTool
    {
        public override ToolId Id => resource.ToolId;

        [SerializeField]
        private ScannerResource resource;

        [SerializeField]
        private ControlsAsset controls;

        [SerializeField]
        private TMPro.TextMeshProUGUI distanceText;
        
        private DistanceScannerModule module;

        [SerializeField]
        private HeadsetCameraRig cameraRig;

        public void EnableFlight()
        {
            gameObject.SetActive(true);
        }

        public void EnableWall()
        {
            gameObject.SetActive(true);

        }

        public void Disable()
        {
            Algorithms.FloorPainter.RemoveHighlight();
            Labyrinths.Piece.RemoveHighlight();
            enabled = false;
            gameObject.SetActive(false);
        }


        // Start is called before the first frame update
        public void Awake()
        {
            controls.ScannedDistance.OnValueChangedHandler += OnDistance;
            module = resource.CreateModule();
        }

        public void FixedUpdate()
        {
            if (cameraRig == null)
                return;

            if (cameraRig == null ||
            Client.Instance.Labyrinth.Value == null ||
            module == null)
            {
                controls.ScannedDistance.Value = -1;
                return;
            }

            Ray ray = new Ray(cameraRig.PointerTransform.position, cameraRig.PointerTransform.forward);

            var res = Physics.RaycastAll(ray, ScannerUtils.RaycastRange);

            module.DoScan(controls, res);
        }

        public override void OnValidate()
        {
            base.OnValidate();

            if (cameraRig == null)
                cameraRig = GetComponentInParent<HeadsetCameraRig>();

        }



        public void OnDistance(float distance)
        {
            distanceText.text = distance < 0 ? "?" : distance.ToString() + " m";
        }

    }
}