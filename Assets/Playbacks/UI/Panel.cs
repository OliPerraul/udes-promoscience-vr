using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Playbacks.UI
{

    public class Panel : MonoBehaviour
    {
        public delegate void OnButton();

        public OnButton OnExpandHandler;

        public OnButton OnCloseHandler;

        [SerializeField]
        private UnityEngine.UI.Button expandButton;

        [SerializeField]
        private UnityEngine.UI.Button closeButton;


        // Use this for initialization
        private void Awake()
        {
            expandButton.onClick.AddListener(OnExpandClicked);
            closeButton.onClick.AddListener(OnCloseClicked);
        }


        public void OnExpandClicked()
        {
            if (OnExpandHandler != null)
                OnExpandHandler.Invoke();
        }

        public void OnCloseClicked()
        {
            //if(OnCloseHandler != null)
            //    OnCloseHandler.Invoke();

            gameObject.SetActive(false);
        }

    }
}
