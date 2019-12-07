using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public abstract class BaseButton : MonoBehaviour
    {
        public Event<BaseButton> OnClickedHandler;

        [SerializeField]
        protected LabyrinthObject labyrinth;

        public LabyrinthObject Labyrinth => labyrinth;

        [SerializeField]
        protected UnityEngine.UI.Button button;

        [SerializeField]
        protected UnityEngine.UI.RawImage rawImage;

        public virtual void Awake()
        {
            button.onClick.AddListener(OnClick);
            button.onClick.AddListener(() => OnClickedHandler?.Invoke(this));
        }

        public abstract void OnClick();

        // TODO: this abstraction does not really work, see subclass
        public virtual BaseButton Create(Transform parent, LabyrinthObject labyrinth) { return null; }
    }
}
