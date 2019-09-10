using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class ProgessionBar : MonoBehaviour
    {
        [SerializeField]
        ScriptableFloat progressRatio;

        [SerializeField]
        GameObject progress;

        void Start()
        {
            progressRatio.valueChangedEvent += OnValueChanged;
        }


        void OnValueChanged()
        {
            progress.transform.localScale = new Vector3(progressRatio.Value, 1, 1);
        }
    }
}
