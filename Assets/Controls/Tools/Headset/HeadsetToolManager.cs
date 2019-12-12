using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using System.Collections.Generic;

namespace UdeS.Promoscience.Controls
{
    //public static class ToolUtils
    //{
    //    public const int ToolCount = 4;
    //}

    public class HeadsetToolManager : MonoBehaviour
    {
        [SerializeField]
        private ControlsAsset controls;

        [SerializeField]
        private HeadsetInputSchemeAsset inputScheme;

        [SerializeField]
        private HeadsetToolManagerAsset asset;

        private ToolId[] ids = {
                        ToolId.None,
                        ToolId.PaintBucket, // TODO should handle painting color not just visual
                        ToolId.AlgorithmClipboard,
                        ToolId.Compass,
                        ToolId.FlightDistanceScanner,
                        ToolId.WallDistanceScanner
            };

        [SerializeField]
        private HeadsetTool[] tools;

        private Dictionary<ToolId, HeadsetTool> leftHandedThirdPerson = new Dictionary<ToolId, HeadsetTool>();

        private Dictionary<ToolId, HeadsetTool> rightHandedThirdPerson = new Dictionary<ToolId, HeadsetTool>();

        private Dictionary<ToolId, HeadsetTool> leftHandedFirstPerson = new Dictionary<ToolId, HeadsetTool>();

        private Dictionary<ToolId, HeadsetTool> rightHandedFirstPerson = new Dictionary<ToolId, HeadsetTool>();

        private Dictionary<ToolId, HeadsetTool> currentTools;

        private Dictionary<ToolId, HeadsetTool> CurrentTools => currentTools;

        private HeadsetTool currentTool = null;

        private int currentToolIndex = 0;


        public void Awake()
        {
            Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithm;

            // Sort tools
            // TODO do this in Edit Time OnValidate
            foreach (var tool in tools)
            {
                if (tool == null)
                    continue;

                if (tool.IsLeftHanded && tool.IsThirdPerson) leftHandedThirdPerson.Add(tool.Id, tool);
                else if (tool.IsLeftHanded && !tool.IsThirdPerson) leftHandedFirstPerson.Add(tool.Id, tool);
                else if (!tool.IsLeftHanded && tool.IsThirdPerson) rightHandedThirdPerson.Add(tool.Id, tool);
                else if (!tool.IsLeftHanded && !tool.IsThirdPerson) rightHandedFirstPerson.Add(tool.Id, tool);

                tool.gameObject.SetActive(false);
            }

            asset.CurrentTool.OnValueChangedHandler += OnToolChanged;

            Client.Instance.Settings.IsLeftHanded.OnValueChangedHandler += OnPreferenceChanged;
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnPreferenceChanged;
            OnPreferenceChanged(false);
        }

        public void OnValidate()
        {
            if (tools == null || tools.Length == 0)
            {
                tools = GetComponentsInChildren<HeadsetTool>();
            }
        }

        public void Update()
        {
            if (inputScheme.IsUpPressed)
            {
                currentToolIndex++;
                currentToolIndex = currentToolIndex.Mod(ids.Length);
            }
            else if (inputScheme.IsDownPressed)
            {
                currentToolIndex--;
                currentToolIndex = currentToolIndex.Mod(ids.Length);
            }

            asset.CurrentTool.Value = ids[currentToolIndex];
        }

        public void OnPreferenceChanged(bool preference)
        {
            currentTools = controls.IsThirdPersonEnabled.Value ?
                Client.Instance.Settings.IsLeftHanded.Value ?
                    leftHandedThirdPerson :
                    rightHandedThirdPerson
                    :
                Client.Instance.Settings.IsLeftHanded.Value ?
                        leftHandedFirstPerson :
                        rightHandedFirstPerson;

            OnToolChanged(asset.CurrentTool.Value);
        }


        public void OnToolChanged(ToolId id)
        {
            //switch (Client.Instance.State.Value)
            //{
            //    case ClientGameState.Playing:
            //    case ClientGameState.PlayingTutorial:
            //        break;

            //    default:
            //        if (currentTool != null) currentTool.gameObject.SetActive(false);
            //        currentTool = null;
            //        return;
            //}

            if (CurrentTools.TryGetValue(id, out HeadsetTool tool))
            {
                if (currentTool != null) currentTool.gameObject.SetActive(false);
                currentTool = tool;
                currentTool.gameObject.SetActive(true);
            }
        }

        public void OnAlgorithm(Algorithms.Algorithm algorithm)
        {
            switch (algorithm.Id)
            {
                case Algorithms.Id.ShortestFlightDistance:
                    ids = new ToolId[] {
                        ToolId.None,
                        ToolId.PaintBucket,
                        ToolId.AlgorithmClipboard,
                        ToolId.Compass,
                        ToolId.FlightDistanceScanner };
                    break;

                case Algorithms.Id.LongestStraight:
                    ids = new ToolId[] {
                        ToolId.None,
                        ToolId.PaintBucket,
                        ToolId.AlgorithmClipboard,
                        ToolId.Compass,
                        ToolId.WallDistanceScanner };
                    break;

                case Algorithms.Id.Standard:
                    ids = new ToolId[] {
                        ToolId.None,
                        ToolId.PaintBucket,
                        ToolId.AlgorithmClipboard,
                        ToolId.Compass };
                    break;

                case Algorithms.Id.RightHand:
                    ids = new ToolId[] {
                        ToolId.None,
                        ToolId.PaintBucket,
                        ToolId.AlgorithmClipboard
                    };
                    break;
            }

            // Put last tool in hand
            currentToolIndex = ids.Length - 1;
            asset.CurrentTool.Value = ids[currentToolIndex];
        }

    }
}