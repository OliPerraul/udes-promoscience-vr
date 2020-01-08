using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Shows progress and algorithm name
    /// </summary>
    public class ClipboardTool : HeadsetTool
    {
        public override ToolId Id => ToolId.AlgorithmClipboard;

        // TODO description        
        [SerializeField]
        private TMPro.TextMeshProUGUI algorithmName;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        public void Awake()
        {
            Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithmChanged;

            algorithmRespect.OnRespectChangedHandler += OnRespectChanged;
        }

        public void OnAlgorithmChanged(Algorithms.Algorithm algorithm)
        {
            algorithmName.text = algorithm.Name;
        }


        public void OnRespectChanged(float respect)
        {

        }



    }
}
