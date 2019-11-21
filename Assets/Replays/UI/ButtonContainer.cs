using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cirrus.Editor;
using Cirrus;
using UdeS.Promoscience.Labyrinths;

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

        public Event<ButtonContainer> OnContainerRemovedHandler;


        public int Count => buttons.Count;

        private List<ReplayButton> buttons = new List<ReplayButton>();

        public void Awake()
        {
            canvasTransform = canvas.GetComponent<RectTransform>();
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

            //Debug.Log(_rectTransform.rect);
            //Debug.Log(_canvasTransform.rect.height);
            
        }

        public void RespectLayout()
        {
            layoutElement.ignoreLayout = false;
            layoutElement.preferredHeight = canvasTransform.rect.height / 2;
        }


        //public void Add(Labyrinth labyrinth, int index)
        //{
        //    var button = buttonTemplate.Create(
        //        transform,
        //        labyrinth,
        //        index == Labyrinths.Utils.NumLabyrinth - 1 ?
        //            ReplayButtonMode.Both :
        //            ReplayButtonMode.Remove);

        //    buttons.Add(button);

        //    button.name = "btn " + index;

        //    button.gameObject.SetActive(true);

        //    button.OnAddedHandler += OnLabyrinthAdded;

        //    button.OnRemovedHandler += OnLabyrinthRemoved;
        //}

        public void OnLabyrinthRemoved(Transform parent, ReplayButton button)
        {
            //labyrinths.Remove(button.Labyrinth);

            //for (int labyrinthIndex = 0; labyrinthIndex < labyrinths.Count; labyrinthIndex++)
            //{
            //    labyrinths[labyrinthIndex].transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * labyrinthIndex;
            //}

            //buttons.Remove(button);

            //RemoveHorizontal(parent.gameObject);

            //if (buttons.Count == 1)
            //{
            //    buttons[0].Mode = ReplayButtonMode.Add;
            //}
        }


        public void AddLabyrinth(Labyrinth labyrinth)
        {
            var button = buttonTemplate.Create(
                transform,
                labyrinth);

            buttons.Add(button);
            button.gameObject.SetActive(true);
            button.OnAddedHandler += () => AddLabyrinth(select.CreateNextLabyrinth());
            button.OnRemovedHandler += OnLabyrinthRemoved;
           
        }
    }
}