using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cirrus.Editor;
using Cirrus;
using UdeS.Promoscience.Labyrinths;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{

    public class ButtonContainer : MonoBehaviour
    {
        [SerializeField]
        [GetComponent(typeof(ReplaySelect))]
        private ReplaySelect select;


        [SerializeField]
        [GetComponent(typeof(Canvas), GetComponentAttributeMode.Parent)]
        private Canvas canvas;

        private RectTransform canvasTransform;

        [SerializeField]
        [GetComponent(typeof(LayoutElement))]
        private LayoutElement layoutElement;

        [SerializeField]
        [GetComponent(typeof(RectTransform))]
        private RectTransform rectTransform;

        [SerializeField]
        [GetComponent(typeof(ReplayButton))]
        private ReplayButton buttonTemplate;

        public Event<ReplayButton> OnButtonRemovedHandler;

        public Event<ButtonContainer> OnRemovedHandler;


        public int NumButtons => buttons.Count;

        private List<ReplayButton> buttons = new List<ReplayButton>();

        public void Awake()
        {
            canvasTransform = canvas.GetComponent<RectTransform>();
            select.OnContentChangedHandler += () => AdjustContent();
            RespectLayout();
        }

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

        public void AdjustContent()
        {
            if (buttons.Count == 0)
                return;

            foreach (var btn in buttons)
            {
                btn.Mode = ReplayButtonMode.Remove;
            }

            buttons[buttons.Count - 1].Mode =
                (select.NumContainers == 1 && buttons.Count == 1) ?
                    ReplayButtonMode.Add :
                    ReplayButtonMode.Both;
        }

        public void OnButtonRemoved(Transform parent, ReplayButton button)
        {
            buttons.Remove(button);

            OnButtonRemovedHandler?.Invoke(button);

            if (buttons.Count == 0)
            {
                gameObject?.Destroy();
                OnRemovedHandler?.Invoke(this);
            }
            else AdjustContent();

        }

        public void AddButton(Labyrinth labyrinth)
        {
            var button = buttonTemplate.Create(
                transform,
                labyrinth);

            buttons.Add(button);

            AdjustContent();

            button.gameObject.SetActive(true);

            button.OnAddedHandler += () => AddButton(select.CreateNextLabyrinth());

            button.OnRemovedHandler += OnButtonRemoved;
           
        }
    }
}