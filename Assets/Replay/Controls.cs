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
        private Camera camera;

        [SerializeField]
        private float scaleSpeed = 0.5f;

        [SerializeField]
        public float dragSpeed = 2;

        private Vector3 dragOrigin;

        public void OnValidate()
        {

        }

        void Update()
        {
            // TODO if playback
            if (serverGameState.GameState == Promoscience.Utils.ServerGameState.IntermissionReplay)
            {
                if (Promoscience.Utils.UI.IsUIElementActive())
                    return;

                float scroll = Input.GetAxis("Mouse ScrollWheel");
                camera.orthographicSize += scroll * scaleSpeed;

                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    return;
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 pos = camera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                    Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

                    camera.transform.Translate(move, Space.World);
                }

            }
        }
    }
}