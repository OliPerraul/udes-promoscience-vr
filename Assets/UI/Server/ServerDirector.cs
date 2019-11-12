using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
//using UdeS.Promoscience.Utils;
using System.Linq;

namespace UdeS.Promoscience.UI.Server
{
    public class ServerDirector : MonoBehaviour
    {
        [System.Serializable]
        public class Step
        {
            [SerializeField]
            [HideInInspector]
            private string name;

            [SerializeField]
            public ServerGameState Flag1;
            [SerializeField]
            public ServerGameState Flag2;
            [SerializeField]
            public ServerGameState Flag3;

            public ServerGameState Flags
            {
                get
                {
                    return Flag1 | Flag2 | Flag3;
                }
                    
            }


            [SerializeField]
            public List<GameObject> gameObjectsToActivate;

            [SerializeField]
            public List<GameObject> gameObjectsToDeactivate;

            public void OnValidate()
            {
                name = Flag1.ToString();
            }
        }

        [SerializeField]
        private List<Step> serverGameStateSteps;

        //[SerializeField]
        //private ScriptableServerGameInformation serverGameInformation;

        

        private void OnValidate()
        {
            serverGameStateSteps.ForEach(x => x.OnValidate());
        }

        void Awake()
        {
            Promoscience.Server.Instance.gameStateChangedEvent += OnValueChanged;

            OnValueChanged();
        }

        void OnValueChanged()
        {
            foreach (var stp in serverGameStateSteps)
            {
                if ((stp.Flags & Promoscience.Server.Instance.GameState) != 0)
                {
                    stp.gameObjectsToActivate.ForEach(x => { if (x != null) x.SetActive(true); });
                    stp.gameObjectsToDeactivate.ForEach(x => { if (x != null) x.SetActive(false); });
                }
            }
        }
    }
}
