using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class ProgessionBarWithTimeLerp : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableClientGameState clientState;

        [SerializeField]
        AvatarControllerAsset controls;

        [SerializeField]
        Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        GameObject progress;

        bool isLerping = false;

        float lerpSpeed = 1.0f;
        float currentValue = 1.0f;


        public void Awake()
        {
            if (transform.childCount != 0)
                transform.GetChild(0).gameObject.SetActive(false);


            algorithmRespect.OnRespectChangedHandler += OnAlgorithmRespectChanged;

            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            OnClientStateChanged(Client.Instance.State.Value);
        }


        public void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
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
                float lerpSpeedValue = currentValue < algorithmRespect.Respect ? lerpSpeed : -lerpSpeed;
                float lerpValue = currentValue + Time.deltaTime * lerpSpeedValue;

                if ((currentValue <= algorithmRespect.Respect && lerpValue > algorithmRespect.Respect) || 
                    (currentValue >= algorithmRespect.Respect && lerpValue < algorithmRespect.Respect))
                {
                    isLerping = false;
                    currentValue = algorithmRespect.Respect;
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
