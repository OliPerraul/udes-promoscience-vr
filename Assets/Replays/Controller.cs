using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Replays
{
    public class Controller : MonoBehaviour
    {
        //[SerializeField]
        //private ScriptableServerGameInformation serverGameState;

        //[SerializeField]
        //private Camera camera;

        [SerializeField]
        private float scaleSpeed = 0.5f;

        [SerializeField]
        public float dragSpeed = 2;

        private Vector3 dragOrigin;

        public void OnValidate()
        {

        }

        public void Update()
        {
            //// TODO if playback
            //if (Server.Instance.GameState == ServerGameState.SimpleReplay ||
            //    Server.Instance.GameState == ServerGameState.AdvancedReplay)
            //{
            //    if (Promoscience.UI.Utils.IsUIElementActive())
            //        return;

            //    float scroll = Input.GetAxis("Mouse ScrollWheel");
            //    camera.orthographicSize += scroll * scaleSpeed;

            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        dragOrigin = Input.mousePosition;
            //        return;
            //    }

            //    if (Input.GetMouseButton(0))
            //    {
            //        Vector3 pos = camera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            //        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

            //        camera.transform.Translate(move, Space.World);
            //    }

            //}
        }
    }
}