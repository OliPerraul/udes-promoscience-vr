using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class ProgessionBarWithTimeLerp : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState clientState;

        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        GameObject progress;

        bool isLerping = false;

        float lerpSpeed = 1.0f;
        float currentValue = 1.0f;

        void OnEnable()
        {
            clientState.OnRespectChangedHandler += OnAlgorithmRespectChanged;
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                foreach (Transform child in transform)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }



        void Update()
        {
            if (isLerping)
            {
                float lerpSpeedValue = currentValue < clientState.Respect ? lerpSpeed : -lerpSpeed;
                float lerpValue = currentValue + Time.deltaTime * lerpSpeedValue;
                if ((currentValue <= clientState.Respect && lerpValue > clientState.Respect) || (currentValue >= clientState.Respect && lerpValue < clientState.Respect))
                {
                    isLerping = false;
                    currentValue = clientState.Respect;
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
