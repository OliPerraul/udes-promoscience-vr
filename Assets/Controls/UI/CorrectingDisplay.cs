using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Algorithms
{
    public class CorrectingDisplay : MonoBehaviour
    {
        [SerializeField]
        private AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private GameObject correctingIndication;

        public void Awake()
        {
            algorithmRespect.IsCorrectingEnabled.OnValueChangedHandler += OnCorrectingEnabled;
            OnCorrectingEnabled(false);
        }

        public void OnCorrectingEnabled(bool enabled)
        {
            correctingIndication.SetActive(enabled);
        }        
    }
}