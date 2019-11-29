using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Cirrus.Editor;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public abstract class BaseSection : MonoBehaviour
    {
        public Cirrus.Event<BaseButton> OnButtonClickHandler;

        [SerializeField]
        [GetComponent(typeof(Canvas), GetComponentAttributeMode.Parent)]
        protected Canvas canvas;

        protected RectTransform canvasTransform;

        [SerializeField]
        [GetComponent(typeof(LayoutElement))]
        protected LayoutElement layoutElement;

        [SerializeField]
        [GetComponent(typeof(RectTransform))]
        protected RectTransform rectTransform;

        public abstract BaseSelectionInterface Select { get; }

        public abstract BaseButton ButtonTemplate { get; }

        public void Fit()
        {
            layoutElement.ignoreLayout = true;
            rectTransform.sizeDelta = new Vector2(
                canvasTransform.rect.width,
                canvasTransform.rect.height);

            rectTransform.localPosition = new Vector2(
                Mathf.Abs(rectTransform.rect.x),
                -canvasTransform.rect.height / 2);
        }

        public void RespectLayout()
        {
            layoutElement.ignoreLayout = false;
            layoutElement.preferredHeight = canvasTransform.rect.height / 2;
        }

        public virtual void Awake()
        {
            canvasTransform = canvas.GetComponent<RectTransform>();

            RespectLayout();
        }

        public virtual void AddButton(Labyrinth labyrinth)
        {
            var button = ButtonTemplate.Create(
                transform,
                labyrinth);

            button.gameObject.SetActive(true);
            button.OnClickedHandler += (x) => OnButtonClickHandler?.Invoke(x);
        }

        
    }
}
