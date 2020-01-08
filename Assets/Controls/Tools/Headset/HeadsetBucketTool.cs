using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Shows currently selected color
    /// </summary>
    public class HeadsetBucketTool : HeadsetTool
    {
        public override ToolId Id => ToolId.PaintBucket;

        [SerializeField]
        private HeadsetControlsAsset controls;

        [SerializeField]
        private HeadsetToolManagerAsset headsetTools;

        [SerializeField]
        private GameObject yellowColor;

        [SerializeField]
        private GameObject redColor;

        [SerializeField]
        private GameObject whiteColor;

        public void Awake()
        {
            controls.PaintingColor.OnValueChangedHandler += OnPaintingColorChanged;
            headsetTools.CurrentTool.OnValueChangedHandler += OnToolchanged;
        }

        public void OnToolchanged(ToolId id)
        {
            if (id == ToolId.PaintBucket)
            {
                OnPaintingColorChanged(controls.PaintingColor.Value);
            }
        }


        public void OnPaintingColorChanged(TileColor color)
        {
            switch (color)
            {
                case TileColor.Grey:
                case TileColor.NoColor:
                    yellowColor.SetActive(false);
                    redColor.SetActive(false);
                    whiteColor.SetActive(true);
                    break;

                case TileColor.Red:
                    yellowColor.SetActive(false);
                    redColor.SetActive(true);
                    whiteColor.SetActive(false);
                    break;

                case TileColor.Yellow:
                    yellowColor.SetActive(true);
                    redColor.SetActive(false);
                    whiteColor.SetActive(false);
                    break;
            }
        }
    }
}