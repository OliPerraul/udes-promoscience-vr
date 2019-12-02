using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Labyrinths
{
    public class Piece : MonoBehaviour
    {
        [SerializeField]
        private TileType[] tileTypes;

        public IEnumerable<TileType> TileTypes => tileTypes;

        public static Cirrus.Event OnTileHighlightStaticHandler;

        public GameObject[] highlights;

        [SerializeField]
        private Controls.AvatarControllerAsset controls;

        private bool highlighted = false;


        public static void RemoveHighlight()
        {
            OnTileHighlightStaticHandler?.Invoke();
        }

        public void Highlight()
        {
            if (highlighted)
                return;

            highlighted = true;

            highlights.OrderBy((x) => (controls.PlayerPosition.Value - x.transform.position).magnitude).FirstOrDefault()?.SetActive(true);

            OnTileHighlightStaticHandler?.Invoke();

            OnTileHighlightStaticHandler += OnOtherHighlight;
        }

        public void OnOtherHighlight()
        {
            foreach (var hightlight in highlights)
            {
                hightlight.SetActive(false);
            }

            OnTileHighlightStaticHandler -= OnOtherHighlight;
        }



    }
}