using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{

    public class TabletToolManagerInterface : MonoBehaviour
    {
        [SerializeField]
        private TabletToolManagerAsset asset;

        [SerializeField]
        private UnityEngine.UI.Button previous;

        [SerializeField]
        private UnityEngine.UI.Button next;

        [SerializeField]
        private GameObject wallDistanceDisplay;

        [SerializeField]
        private UnityEngine.UI.Text wallDistanceText;

        [SerializeField]
        private GameObject flightDistanceDisplay;

        [SerializeField]
        private UnityEngine.UI.Text flightDistanceText;

        [SerializeField]
        private GameObject previewImage;


        public void Awake()
        {
            previous.onClick.AddListener(() => asset.OnLeftPressedHandler?.Invoke());
            next.onClick.AddListener(() => asset.OnRightPressedHandler?.Invoke());

            asset.CurrentTool.OnValueChangedHandler += OnCurrentToolChanged;

            asset.ScannedDistance.OnValueChangedHandler += OnScannedDistanceChanged;

        }

        // TODO 
        /// TODO  2 diffeent Scanned Distance
        public void OnScannedDistanceChanged(float scanned)
        {
            if (scanned < 0)
            {
                flightDistanceText.text = "?";
                wallDistanceText.text = "?";
            }

            flightDistanceText.text = scanned + " m";
            wallDistanceText.text = scanned.ToString() + " m";
        }

        public void OnCurrentToolChanged(ToolId id)
        {
            switch (id)
            {
                case ToolId.FlightDistanceScanner:
                    wallDistanceDisplay.gameObject.SetActive(false);
                    flightDistanceDisplay.gameObject.SetActive(true);
                    previewImage.gameObject.SetActive(false);
                    break;

                case ToolId.WallDistanceScanner:
                    wallDistanceDisplay.gameObject.SetActive(true);
                    flightDistanceDisplay.gameObject.SetActive(false);
                    previewImage.gameObject.SetActive(false);
                    break;

                default:
                    wallDistanceDisplay.gameObject.SetActive(false);
                    flightDistanceDisplay.gameObject.SetActive(false);
                    previewImage.gameObject.SetActive(true);
                    break;

            }
        }

    }
}
