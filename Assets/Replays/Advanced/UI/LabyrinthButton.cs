using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;

namespace UdeS.Promoscience.Replays.Advanced.UI
{
    public class LabyrinthButton : MonoBehaviour
    {
        public Labyrinths.OnLabyrinthEvent OnClickedHandler;

        public Labyrinths.Labyrinth Labyrinth;

        [SerializeField]
        private UnityEngine.UI.Button button;

        private bool init = false;

        public LabyrinthButton Create(
            Transform parent, 
            Labyrinths.Labyrinth labyrinth)
        {
            var l = this.Create(parent);
            l.Labyrinth = labyrinth;
            return l;
        }

        public void Awake()
        {
            button.onClick.AddListener(OnClicked);
        }

        public void OnClicked()
        {            
            if(OnClickedHandler != null) OnClickedHandler.Invoke(Labyrinth);
        }
    }
}
