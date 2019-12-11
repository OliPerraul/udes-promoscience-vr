using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Menu
{

    public class ButtonManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button startButton;

        public void Awake()
        {
            startButton.onClick.AddListener(()=> {
                Server.Instance.StartLobby();
                
                
                });
        }
    }
}