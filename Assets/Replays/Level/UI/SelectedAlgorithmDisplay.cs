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

        public void dosomthing(int x) => steps.text = x.ToString(); 

        public void Awake()
        {
            asset.MoveiNdexHandler += dosomthing;
        }


        public void OnDestroy()
        {
            asset.MoveiNdexHandler -= dosomthing;
        }

    }
}