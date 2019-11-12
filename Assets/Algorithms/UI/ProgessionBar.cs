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
        Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        GameObject progress;


        public void Awake()
        {
            if (transform.childCount != 0)
                transform.GetChild(0).gameObject.SetActive(false);

            algorithmRespect.OnRespectChangedHandler += OnValueChanged;
        }


        void OnValueChanged(float value)
        {
            progress.transform.localScale = new Vector3(value, 1, 1);
        }
    }
}
