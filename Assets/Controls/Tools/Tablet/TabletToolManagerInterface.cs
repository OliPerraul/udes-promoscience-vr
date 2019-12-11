using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{

    public class TabletToolManagerInterface : MonoBehaviour
    {
        [SerializeField]
        private TabletToolManagerAsset asset;

        [SerializeField]
        private UnityEngine.UI.RawImage previewImage;

        [SerializeField]
        private UnityEngine.UI.Button previous;

        [SerializeField]
        private UnityEngine.UI.Button next;


        public void Awake()
        {
            previous.onClick.AddListener(() => asset.OnLeftPressedHandler?.Invoke());
            next.onClick.AddListener(() => asset.OnRightPressedHandler?.Invoke());

            //asset.CurrentTool.OnValueChangedHandler +=

        }
    }
}
