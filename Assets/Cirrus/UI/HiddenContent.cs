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

        [SerializeField]
        private bool isOverriden = false;

        private bool isSupposedToBeHidden = false;

        private bool isHidden = false;

        public void Awake()
        {
            if (!isOverriden)
                Hide();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isSupposedToBeHidden = false;
            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isSupposedToBeHidden = true;

            Hide();
        }

        public void Hide()
        {
            if (isHidden)
                return;

            isHidden = true;

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
            if (!isHidden)
                return;

            isHidden = false;

            Hidden.SetActive(true);

            if (_isHiddenAlpha)
            {
                Hidden.GetComponent<IAlphaContent>()?.Set(1);
            }
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Show();
            }
        }
    }
}