using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class PaintingColorDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        Labyrinths.ScriptableTileColor paintingColor;

        [SerializeField]
        GameObject colorRings;

        [SerializeField]
        GameObject greyRing;

        [SerializeField]
        GameObject yellowRing;

        [SerializeField]
        GameObject redRing;

        private bool init = false;

        void OnEnable()
        {
            if (init) return;

            init = true;

            paintingColor.valueChangedEvent += OnPaintingColorValueChanged;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;
        }

        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    colorRings.gameObject.SetActive(true);
                    break;

                default:
                    colorRings.gameObject.SetActive(false);
                    break;

            }
        }


        void OnPaintingColorValueChanged()
        {
            if (paintingColor.Value == TileColor.Grey)
            {
                greyRing.SetActive(true);
                yellowRing.SetActive(false);
                redRing.SetActive(false);
            }
            else if (paintingColor.Value == TileColor.Yellow)
            {
                yellowRing.SetActive(true);
                greyRing.SetActive(false);
                redRing.SetActive(false);
            }
            else if (paintingColor.Value == TileColor.Red)
            {
                redRing.SetActive(true);
                greyRing.SetActive(false);
                yellowRing.SetActive(false);
            }
        }
    }
}
