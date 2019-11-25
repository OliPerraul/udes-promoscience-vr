using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    public class LabyrinthEditorController : MonoBehaviour
    {
        [SerializeField]
        private ControllerAsset controller;

        // Use this for initialization
        public void Start()
        {
            controller.State.Set(State.Select);
        }

    }
}
