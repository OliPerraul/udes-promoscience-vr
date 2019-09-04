using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.UI
{
    public class ProgessionBarWithTimeLerp : MonoBehaviour
    {
        [SerializeField]
        ScriptableFloat progressRatio;

        [SerializeField]
        GameObject progress;

        bool isLerping = false;

        float lerpSpeed = 1.0f;
        float currentValue = 1.0f;

        void Start()
        {
            progressRatio.valueChangedEvent += OnValueChanged;
        }

        void Update()
        {
            if (isLerping)
            {
                float lerpSpeedValue = currentValue < progressRatio.Value ? lerpSpeed : -lerpSpeed;
                float lerpValue = currentValue + Time.deltaTime * lerpSpeedValue;
                if ((currentValue <= progressRatio.Value && lerpValue > progressRatio.Value) || (currentValue >= progressRatio.Value && lerpValue < progressRatio.Value))
                {
                    isLerping = false;
                    currentValue = progressRatio.Value;
                    progress.transform.localScale = new Vector3(currentValue, 1, 1);
                }
                else
                {
                    currentValue = lerpValue;
                    progress.transform.localScale = new Vector3(currentValue, 1, 1);
                }
            }
        }


        void OnValueChanged()
        {
            isLerping = true;
        }
    }
}
