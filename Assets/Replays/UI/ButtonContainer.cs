using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cirrus.Editor;


namespace UdeS.Promoscience.Replays.UI
{

    public class ButtonContainer : MonoBehaviour
    {
        [SerializeField]
        [GetComponent(typeof(Canvas), GetComponentAttributeMode.Parent)]
        private Canvas _canvas;

        private RectTransform _canvasTransform;

        [SerializeField]
        [GetComponent(typeof(LayoutElement))]
        private LayoutElement _layoutElement;

        [SerializeField]
        [GetComponent(typeof(RectTransform))]
        private RectTransform _rectTransform;


        public void Awake()
        {
            _canvasTransform = _canvas.GetComponent<RectTransform>();
            RespectLayout();
        }

        public void Fit()
        {
            _layoutElement.ignoreLayout = true;
            _rectTransform.sizeDelta = new Vector2(
                _canvasTransform.rect.width,
                _canvasTransform.rect.height);

            _rectTransform.localPosition = new Vector2(
                Mathf.Abs(_rectTransform.rect.x),
                -_canvasTransform.rect.height / 2);

            //Debug.Log(_rectTransform.rect);
            //Debug.Log(_canvasTransform.rect.height);
            
        }

        public void RespectLayout()
        {
            _layoutElement.ignoreLayout = false;
            _layoutElement.preferredHeight = _canvasTransform.rect.height / 2;
        }




    }
}