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

        }
    }
}