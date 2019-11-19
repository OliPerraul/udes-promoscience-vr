using UnityEngine;
using System.Collections;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class LabyrinthPanel : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button closeButton;

        [SerializeField]
        private UnityEngine.UI.Button labyrinthButton;

        private Labyrinths.IData labyrinth;

        public void Awake()
        {
            closeButton.onClick.AddListener(OnClosedClicked);
            labyrinthButton.onClick.AddListener(OnLabyrinthClicked);
        }

        public void OnClosedClicked()
        {

        }

        public void OnLabyrinthClicked()
        {

        }


        public LabyrinthPanel Create(
            Transform parent,
            Labyrinths.IData labyrinth,
            bool replayLocked = true)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            return l;
        }
    }


}