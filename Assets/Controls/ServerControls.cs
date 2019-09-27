using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.ScriptableObjects;
using UnityEngine;

namespace UdeS.Promoscience
{
    public class ServerControls : MonoBehaviour
    {
        [SerializeField]
        private ScriptableServerGameInformation serverGameState;


        [SerializeField]
        private CameraWrapper camera;

        [SerializeField]
        private float scaleSpeed = 0.5f;

        [SerializeField]
        public float dragSpeed = 2;

        private Vector3 dragOrigin;

        public void OnValidate()
        {
            if (camera == null)
            {
                camera = FindObjectOfType<CameraWrapper>();
            }
        }

        void Update()
        {
            // TODO if playback
            if (serverGameState.GameState == Utils.ServerGameState.ViewingPlayback)
            {
                if (Utils.UI.IsUIElementActive())
                    return;

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                camera.Camera.orthographicSize += scroll * scaleSpeed;             

                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    return;
                }

                if (!Input.GetMouseButton(0)) return;

                Vector3 pos = camera.Camera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

                transform.Translate(move, Space.World);

            }
        }
    }
}
