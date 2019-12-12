using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{

    public class TabletToolManagerInterface : MonoBehaviour
    {
        [SerializeField]
        private ControlsAsset controls;

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
        private GameObject compassDisplay;

        [SerializeField]
        private UnityEngine.UI.Text compassText;


        [SerializeField]
        private UnityEngine.UI.Image bucketImage;

        [SerializeField]
        private UnityEngine.UI.Text bucketText;


        // TODO remove
        [SerializeField]
        private GameObject previewImage;


        public void Awake()
        {
            previous.onClick.AddListener(() => asset.OnLeftPressedHandler?.Invoke());
            next.onClick.AddListener(() => asset.OnRightPressedHandler?.Invoke());

            controls.ForwardDirection.OnValueChangedHandler += OnForwardValueCHanged;

            controls.PaintingColor.OnValueChangedHandler += OnColorValueCHanged;


            asset.CurrentTool.OnValueChangedHandler += OnCurrentToolChanged;

            asset.ScannedDistance.OnValueChangedHandler += OnScannedDistanceChanged;

        }

        public void Start()
        {
            OnScannedDistanceChanged(-1);

            // TODO remove real world tool
            previewImage.gameObject.SetActive(false);
        }

        // TODO 
        /// TODO  2 diffeent Scanned Distance
        public void OnScannedDistanceChanged(float scanned)
        {
            if (scanned < 0)
            {
                flightDistanceText.text = "?";
                wallDistanceText.text = "?";
                return;
            }

            flightDistanceText.text = scanned.ToString() + " m";
            wallDistanceText.text = scanned.ToString() + " m";
        }



        public void OnForwardValueCHanged(int dir)
        {
            switch ((Direction)dir)
            {
                case Direction.Up:// North
                    compassText.text = "Nord";
                    break;

                case Direction.Down:// North
                    compassText.text = "Sud";
                    break;

                case Direction.Left:// North
                    compassText.text = "Est";
                    break;

                case Direction.Right:// North
                    compassText.text = "Ouest";
                    break;
            }
        }


        public void OnColorValueCHanged(TileColor dir)
        {
            switch (dir)
            {
                case TileColor.Red:// North
                    bucketText.text = "Rouge";
                    bucketImage.color = UnityEngine.Color.red;
                    break;

                case TileColor.Yellow:// North
                    bucketText.text = "Jaune";
                    bucketImage.color = UnityEngine.Color.yellow;
                    break;

                case TileColor.NoColor:// North
                case TileColor.Grey:// North
                    compassText.text = "Gris";
                    bucketImage.color = UnityEngine.Color.gray;
                    break;

                //case Direction.Right:// North
                //    compassText.text = "Ouest";
                //    break;
            }
        }


        public void OnCurrentToolChanged(ToolId id)
        {
            switch (id)
            {
                case ToolId.FlightDistanceScanner:
                    wallDistanceDisplay.gameObject.SetActive(false);
                    flightDistanceDisplay.gameObject.SetActive(true);
                    compassDisplay.gameObject.SetActive(false);
                    bucketImage.gameObject.SetActive(false);
                    previewImage.gameObject.SetActive(false);
                    break;

                case ToolId.WallDistanceScanner:
                    wallDistanceDisplay.gameObject.SetActive(true);
                    flightDistanceDisplay.gameObject.SetActive(false);
                    compassDisplay.gameObject.SetActive(false);
                    bucketImage.gameObject.SetActive(false);
                    previewImage.gameObject.SetActive(false);
                    break;

                // TODO use compass object
                case ToolId.Compass:
                    wallDistanceDisplay.gameObject.SetActive(false);
                    flightDistanceDisplay.gameObject.SetActive(false);
                    compassDisplay.gameObject.SetActive(true);
                    bucketImage.gameObject.SetActive(false);
                    previewImage.gameObject.SetActive(false);
                    break;

                case ToolId.PaintBucket:
                    wallDistanceDisplay.gameObject.SetActive(false);
                    flightDistanceDisplay.gameObject.SetActive(false);
                    compassDisplay.gameObject.SetActive(false);
                    bucketImage.gameObject.SetActive(true);
                    previewImage.gameObject.SetActive(false);
                    break;

                //default:
                //    wallDistanceDisplay.gameObject.SetActive(false);
                //    flightDistanceDisplay.gameObject.SetActive(false);
                //    compassDisplay.gameObject.SetActive(false);
                //    previewImage.gameObject.SetActive(true);
                //    break;

            }
        }

    }
}
