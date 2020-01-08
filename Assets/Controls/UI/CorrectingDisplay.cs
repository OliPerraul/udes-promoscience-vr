using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Algorithms
{
    /// <summary>
    /// Display is shown when the headset user is correcting his own path (As requested by tablet user)
    /// </summary>
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