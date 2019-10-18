using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replay.UI
{
    public class LabyrinthButton : MonoBehaviour
    {
        public OnIntEvent OnClickedHandler;

        [SerializeField]
        private int id;

        [SerializeField]
        private UnityEngine.UI.Button button;

        private bool init = false;

        public void OnEnable()
        {
            if (init) return;

            init = true;

            button.onClick.AddListener(OnClicked);
        }

        public void OnClicked()
        {            
            if(OnClickedHandler != null) OnClickedHandler.Invoke(id);
        }
    }
}
