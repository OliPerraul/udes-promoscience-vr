using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class ProgessionBarWithTimeLerp : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableClientGameState clientState;

        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        GameObject progress;

        bool isLerping = false;

        float lerpSpeed = 1.0f;
        float currentValue = 1.0f;

        private bool init = false;

        public void Awake()
        {
            if (transform.childCount != 0)
                transform.GetChild(0).gameObject.SetActive(false);
        }

        void OnEnable()
        {
            if (init) return;

            init = true;

            Client.Instance.OnRespectChangedHandler += OnAlgorithmRespectChanged;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;
            //controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        public void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    transform.GetChild(0).gameObject.SetActive(true);
                    break;
                default:
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }


        void Update()
        {
            if (isLerping)
            {
                float lerpSpeedValue = currentValue < Client.Instance.Respect ? lerpSpeed : -lerpSpeed;
                float lerpValue = currentValue + Time.deltaTime * lerpSpeedValue;

                if ((currentValue <= Client.Instance.Respect && lerpValue > Client.Instance.Respect) || 
                    (currentValue >= Client.Instance.Respect && lerpValue < Client.Instance.Respect))
                {
                    isLerping = false;
                    currentValue = Client.Instance.Respect;
                    progress.transform.localScale = new Vector3(currentValue, 1, 1);
                }
                else
                {
                    currentValue = lerpValue;
                    progress.transform.localScale = new Vector3(currentValue, 1, 1);
                }
            }
        }


        void OnAlgorithmRespectChanged(float value)
        {
            isLerping = true;
        }
    }
}
