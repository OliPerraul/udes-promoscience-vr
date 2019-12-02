using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class PaintingColorDisplay : MonoBehaviour
    {
        [SerializeField]
        AvatarControllerAsset controls;

        [SerializeField]
        GameObject colorRings;

        [SerializeField]
        GameObject greyRing;

        [SerializeField]
        GameObject yellowRing;

        [SerializeField]
        GameObject redRing;

        void Awake()
        {
            controls.PaintingColor.OnValueChangedHandler += OnPaintingColorValueChanged;
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;
            OnClientStateChanged(Client.Instance.State.Value);
        }

        void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
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


        void OnPaintingColorValueChanged(TileColor paintingColor)
        {
            if (paintingColor == TileColor.Grey)
            {
                greyRing.SetActive(true);
                yellowRing.SetActive(false);
                redRing.SetActive(false);
            }
            else if (paintingColor == TileColor.Yellow)
            {
                yellowRing.SetActive(true);
                greyRing.SetActive(false);
                redRing.SetActive(false);
            }
            else if (paintingColor == TileColor.Red)
            {
                redRing.SetActive(true);
                greyRing.SetActive(false);
                yellowRing.SetActive(false);
            }
        }
    }
}
