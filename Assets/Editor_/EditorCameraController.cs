using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    public class EditorCameraController : Cirrus.BaseSingleton<EditorCameraController>
    {
        [SerializeField]
        private UnityEngine.Camera camera;

        public void Awake()
        {
            EditorController.Instance.State.OnValueChangedHandler += OnStateChanged;
        }


        public virtual void OnStateChanged(EditorState state)
        {
            switch (state)
            {
                case EditorState.Select:
                    camera.gameObject.SetActive(true);
                    break;

                case EditorState.Editor:
                    // Use labyrinth camera
                    camera.gameObject.SetActive(false);
                    break;

                //default:
                //    camera.gameObject.SetActive(false);
                //    break;
            }
        }
    }
}