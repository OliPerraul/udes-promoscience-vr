using UnityEngine;
using System.Collections;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Controls
{
    public enum ToolType : int
    {
        DistanceScanner = 0,
        Compass = 1
    }

    public static class ToolUtils
    {
        public const int ToolCount = 2;
    }

    public class HeadsetToolManager : MonoBehaviour
    {
        [SerializeField]
        private Remote firstPersonRemote;

        [SerializeField]
        private Remote thirdPersonRemote;

        [SerializeField]
        private Promoscience.UI.Compass firstPersonCompass;

        [SerializeField]
        private Promoscience.UI.Compass thirdPersonCompass;

        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private HeadsetInputSchemeAsset inputScheme;

        [SerializeField]
        private HeadsetToolManagerAsset asset;


        public void Awake()
        {
            asset.CurrentEquipment.OnValueChangedHandler += OnToolChanged;
        }

        public void Update()
        {
            if (inputScheme.IsUpPressed)
            {
                asset.CurrentEquipment.Value = (ToolType)((int)asset.CurrentEquipment.Value + 1).Mod(ToolUtils.ToolCount);
            }
            else if(inputScheme.IsDownPressed)
            {
                asset.CurrentEquipment.Value = (ToolType)((int)asset.CurrentEquipment.Value - 1).Mod(ToolUtils.ToolCount);
            }
        }

        public void OnToolChanged(ToolType tool)
        {
            switch (tool)
            {
                case ToolType.DistanceScanner:

                    if (controls.IsThirdPersonEnabled.Value)
                    {
                        thirdPersonRemote.gameObject.SetActive(true);
                        thirdPersonCompass.gameObject.SetActive(false);
                    }
                    else
                    {
                        firstPersonRemote.gameObject.SetActive(true);
                        firstPersonCompass.gameObject.SetActive(false);
                    }
                    break;

                case ToolType.Compass:

                    if (controls.IsThirdPersonEnabled.Value)
                    {
                        thirdPersonRemote.gameObject.SetActive(false);
                        thirdPersonCompass.gameObject.SetActive(true);
                    }
                    else
                    {
                        firstPersonRemote.gameObject.SetActive(false);
                        firstPersonCompass.gameObject.SetActive(true);
                    }

                    break;
            }
        }
    }
}