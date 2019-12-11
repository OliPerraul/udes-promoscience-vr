using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using System.Collections.Generic;

namespace UdeS.Promoscience.Controls
{
    public class TabletToolManager : MonoBehaviour
    {
        [SerializeField]
        private ControlsAsset controls;

        [SerializeField]
        private TabletToolManagerAsset asset;

        private ToolId[] ids = {
                        ToolId.PaintBucket,
                        ToolId.Compass,
                        ToolId.FlightDistanceScanner,
                        ToolId.WallDistanceScanner
            };

        [SerializeField]
        private BaseTool[] tools;

        private Dictionary<ToolId, BaseTool> currentTools = new Dictionary<ToolId, BaseTool>();

        private Dictionary<ToolId, BaseTool> CurrentTools => currentTools;

        private BaseTool currentTool = null;

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

                currentTools.Add(tool.Id, tool);

                tool.gameObject.SetActive(false);
            }

            asset.CurrentTool.OnValueChangedHandler += OnToolChanged;
            asset.OnLeftPressedHandler += OnLeftPressed;
            asset.OnRightPressedHandler += OnRightPressed;

        }

        public void OnValidate()
        {
            if (tools == null || tools.Length == 0)
            {
                tools = GetComponentsInChildren<BaseTool>();
            }
        }

        public void OnLeftPressed()
        {
            currentToolIndex--;
            currentToolIndex = currentToolIndex.Mod(ids.Length);
            asset.CurrentTool.Value = ids[currentToolIndex];
        }

        public void OnRightPressed()
        {
            currentToolIndex++;
            currentToolIndex = currentToolIndex.Mod(ids.Length);
            asset.CurrentTool.Value = ids[currentToolIndex];
        }

        public void OnToolChanged(ToolId id)
        {
            if (CurrentTools.TryGetValue(id, out BaseTool tool))
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
                        ToolId.PaintBucket,
                        ToolId.Compass,
                        ToolId.FlightDistanceScanner };
                    break;

                case Algorithms.Id.LongestStraight:
                    ids = new ToolId[] {
                        ToolId.PaintBucket,
                        ToolId.Compass,
                        ToolId.WallDistanceScanner };
                    break;

                case Algorithms.Id.Standard:
                    ids = new ToolId[] {
                        ToolId.PaintBucket,
                        ToolId.Compass };
                    break;

                case Algorithms.Id.RightHand:
                    ids = new ToolId[] {
                        ToolId.PaintBucket,
                    };
                    break;
            }

            // Put last tool in hand
            currentToolIndex = ids.Length - 1;
            asset.CurrentTool.Value = ids[currentToolIndex];
        }

    }
}