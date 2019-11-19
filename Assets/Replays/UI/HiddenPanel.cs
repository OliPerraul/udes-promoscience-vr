using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Replays.UI
{

    public class HiddenPanel : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField]
        private GameObject Target;

        [SerializeField]
        private GameObject Hidden;

        public void Awake()
        {
            Hidden.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hidden.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hidden.SetActive(false);
        }
    }
}