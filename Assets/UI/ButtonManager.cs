using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.UI
{

    public enum ButtonCanvasFlag
    {
        None = 0,
        Exit = 1 << 0,
        Random = 1 << 1,
        Info = 1 << 2,
    }

    // TODO remove: I think this is bad practice but im tired

    public class ButtonManager : Cirrus.BaseSingleton<ButtonManager>
    {
        [SerializeField]
        public UnityEngine.UI.Button ExitButton;

        [SerializeField]
        public UnityEngine.UI.Button RandomButton;

        [SerializeField]
        public UnityEngine.UI.Button InfoButton;

        public Cirrus.ObservableValue<ButtonCanvasFlag> Flags = new Cirrus.ObservableValue<ButtonCanvasFlag>();

        public void Awake()
        {
            Flags.OnValueChangedHandler += OnFlagsChanged;
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
        }


        public void OnFlagsChanged(ButtonCanvasFlag flags)
        {
            ExitButton.gameObject.SetActive((flags & ButtonCanvasFlag.Exit) != 0);
            RandomButton.gameObject.SetActive((flags & ButtonCanvasFlag.Random) != 0);
            InfoButton.gameObject.SetActive((flags & ButtonCanvasFlag.Info) != 0);
        }





        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LabyrinthReplay:
                    Flags.Set(ButtonCanvasFlag.Exit | ButtonCanvasFlag.Info);
                    break;

                case ServerState.LevelSelect:
                    Flags.Set(ButtonCanvasFlag.Random | ButtonCanvasFlag.Exit);
                    break;

                case ServerState.ReplaySelect:
                case ServerState.Round:
                case ServerState.Quickplay:
                case ServerState.Lobby:
                    Flags.Set(ButtonCanvasFlag.Exit);
                    break;

            }
        }


    }
}