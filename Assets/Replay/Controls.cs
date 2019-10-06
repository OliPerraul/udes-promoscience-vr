using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Replay
{
    //public 

    public class Controls : MonoBehaviour
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

                Ray ray = camera.Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Segment sgm = hit.collider.GetComponent<Replay.Segment>();
                    if (sgm != null)
                    {
                        if (sgm.OnMouseEvent != null)
                            sgm.OnMouseEvent.Invoke();
                    }
                }

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                camera.Camera.orthographicSize += scroll * scaleSpeed;

                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    return;
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 pos = camera.Camera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                    Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

                    camera.Camera.transform.Translate(move, Space.World);
                }

            }
        }
    }
}