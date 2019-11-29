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

    public class ReplaySection : Labyrinths.UI.BaseSection
    {
        [SerializeField]
        [GetComponent(typeof(ReplaySelectionInterface))]
        private ReplaySelectionInterface select;

        [SerializeField]
        [GetComponent(typeof(ReplayButton))]
        private ReplayButton buttonTemplate;

        public override Labyrinths.UI.BaseButton ButtonTemplate => buttonTemplate;

        public Event<ReplayButton> OnButtonRemovedHandler;

        public Event<ReplaySection> OnRemovedHandler;

        public override Labyrinths.UI.BaseSelectionInterface Select => select;

        public int NumButtons => buttons.Count;

        private List<ReplayButton> buttons = new List<ReplayButton>();

        public override void Awake()
        {
            base.Awake();
            select.OnContentChangedHandler += () => AdjustContent();
        }

        public void AdjustContent()
        {
            if (buttons.Count == 0)
                return;

            foreach (var btn in buttons)
            {
                btn.Mode = ButtonMode.Remove;
            }

            buttons[buttons.Count - 1].Mode =
                (select.NumSections == 1 && buttons.Count == 1) ?
                    ButtonMode.Add :
                    ButtonMode.Both;
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

        public override void AddButton(Labyrinth labyrinth)
        {
            var button = buttonTemplate.CreateReplayButton(
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