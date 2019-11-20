using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Cirrus.UI
{

    public class HiddenContent : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField]
        private GameObject Target;

        [SerializeField]
        private GameObject Hidden;

        [SerializeField]
        private bool _isHiddenAlpha;

        [SerializeField]
        private float _hiddenAlpha = 0.5f;

        public void Awake()
        {
            Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Show();
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        public void Hide()
        {
            if (_isHiddenAlpha)
            {
                Hidden.GetComponent<IAlphaContent>()?.Set(_hiddenAlpha);
            }
            else
            {
                Hidden.SetActive(false);
            }
        }

        public void Show()
        {
            Hidden.SetActive(true);

            if (_isHiddenAlpha)
            {
                Hidden.GetComponent<IAlphaContent>()?.Set(1);
            }
        }

    }
}