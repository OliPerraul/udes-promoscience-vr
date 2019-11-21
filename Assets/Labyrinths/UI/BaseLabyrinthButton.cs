using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public abstract class BaseLabyrinthButton : MonoBehaviour
    {
        public Event<BaseLabyrinthButton> OnClickedHandler;

        [SerializeField]
        protected Labyrinth labyrinth;

        public Labyrinth Labyrinth => labyrinth;

        [SerializeField]
        protected UnityEngine.UI.Button button;

        [SerializeField]
        protected UnityEngine.UI.RawImage rawImage;

        public virtual void Awake()
        {
            button.onClick.AddListener(() => OnClickedHandler?.Invoke(this));
        }
    }
}
