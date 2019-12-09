using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replays.UI
{
    public class AlgorithmSelectionInteface : MonoBehaviour
    {
        [SerializeField]
        public AlgorithmSelectionAsset asset;

        [SerializeField]
        private UnityEngine.UI.Text algorithmNameText;

        [SerializeField]
        private UnityEngine.UI.Text algorithmStepsText;

        [SerializeField]
        private UnityEngine.UI.Button algorithmNext;

        [SerializeField]
        private UnityEngine.UI.Button algorithmPrevious;

        public Cirrus.ObservableValue<Algorithms.Id> Algorithm = new Cirrus.ObservableValue<Algorithms.Id>();

        public void Awake()
        {
            Algorithm.OnValueChangedHandler += (x) => algorithmNameText.text = Algorithms.Resources.Instance.GetAlgorithm(x).Name;
            Algorithm.OnValueChangedHandler += (x) => { if (asset != null) asset.Algorithm.Value = x; };

            algorithmPrevious.onClick.AddListener(() => Algorithm.Value = (Algorithms.Id)((int)Algorithm.Value - 1).Mod(Algorithms.Utils.NumAlgorithms));
            algorithmNext.onClick.AddListener(() => Algorithm.Value = (Algorithms.Id)((int)Algorithm.Value + 1).Mod(Algorithms.Utils.NumAlgorithms));            
        }
    }
}