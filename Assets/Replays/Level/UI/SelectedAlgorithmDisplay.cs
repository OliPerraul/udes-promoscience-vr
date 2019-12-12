using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays.UI
{

    public class SelectedAlgorithmDisplay : MonoBehaviour
    {
        [SerializeField]
        private AlgorithmSelectionAsset asset;

        [SerializeField]
        private UnityEngine.UI.Text steps;

        public void Awake()
        {
            asset.MoveIndex.OnValueChangedHandler += (x) => steps.text = x.ToString(); 
        }


    }
}