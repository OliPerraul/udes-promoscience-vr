using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class TabletBucketTool : BaseTool
    {
        public override ToolId Id => ToolId.PaintBucket;

        [SerializeField]
        private HeadsetControlsAsset controls;

        [SerializeField]
        private GameObject yellowColor;

        [SerializeField]
        private GameObject redColor;

        [SerializeField]
        private GameObject whiteColor;

        public void Awake()
        {
            controls.PaintingColor.OnValueChangedHandler += OnPaintingColorChanged;
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