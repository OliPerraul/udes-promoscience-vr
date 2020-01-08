using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays.UI
{
    /// <summary>
    /// Allows to selected whihc algorithm is selected
    /// </summary>
    public class SelectedAlgorithmDisplay : MonoBehaviour
    {
        [SerializeField]
        private AlgorithmSelectionAsset asset;

        [SerializeField]
        private UnityEngine.UI.Text steps;

        public void SetText(int x) => steps.text = x.ToString(); 

        public void Awake()
        {
            asset.MoveiNdexHandler += SetText;
        }    

        public void OnDestroy()
        {
            asset.MoveiNdexHandler -= SetText;
        }

    }
}