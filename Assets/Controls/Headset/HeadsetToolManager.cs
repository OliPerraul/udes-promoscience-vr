using UnityEngine;
using System.Collections;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Controls
{
    public enum ToolType : int
    {
        None = 0,
        // TODO: make into 2 tools?
        DistanceScanner = 1,
        Compass = 2
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

        private ToolType[] tools = { ToolType.None, ToolType.Compass, ToolType.DistanceScanner };

        private int activeToolIndex = 0;

        public void Awake()
        {
            Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithm;

            asset.CurrentEquipment.OnValueChangedHandler += OnToolChanged;

            controls.IsThirdPersonEnabled.OnValueChangedHandler += (x) => OnToolChanged(asset.CurrentEquipment.Value);
            
        }

        public void Update()
        {
            if (inputScheme.IsUpPressed)
            {
                asset.CurrentEquipment.Value = tools[(activeToolIndex + 1).Mod(tools.Length)];
            }
            else if(inputScheme.IsDownPressed)
            {
                asset.CurrentEquipment.Value = tools[(activeToolIndex - 1) .Mod(tools.Length)];
            }
        }

        public void OnAlgorithm(Algorithms.Algorithm algorithm)
        {
            switch (algorithm.Id)
            {
                case Algorithms.Id.ShortestFlightDistance:
                case Algorithms.Id.LongestStraight:
                    tools = new ToolType[] { ToolType.None, ToolType.Compass, ToolType.DistanceScanner };
                    break;

                case Algorithms.Id.Standard:
                    tools = new ToolType[] { ToolType.None, ToolType.Compass };
                    break;

                case Algorithms.Id.RightHand:
                    tools = new ToolType[] { ToolType.None };
                    break;
            }

            activeToolIndex = tools.Length - 1;
            asset.CurrentEquipment.Value = tools[activeToolIndex];
        }

        public void OnToolChanged(ToolType tool)
        {
            switch (tool)
            {
                case ToolType.None:
                    if (controls.IsThirdPersonEnabled.Value)
                    {
                        thirdPersonRemote.gameObject.SetActive(false);
                        thirdPersonCompass.gameObject.SetActive(false);
                    }
                    else
                    {
                        firstPersonRemote.gameObject.SetActive(false);
                        firstPersonCompass.gameObject.SetActive(false);
                    }
                    break;


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