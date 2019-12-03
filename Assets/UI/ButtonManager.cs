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

        [SerializeField]
        private Cirrus.SceneWrapperAsset StartScene;

        public Cirrus.ObservableValue<ButtonCanvasFlag> Flags = new Cirrus.ObservableValue<ButtonCanvasFlag>();

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;

            Flags.OnValueChangedHandler += OnFlagsChanged;
            
            ExitButton.onClick.AddListener(OnExitClicked);
        }

        public void Start()
        {
            OnServerStateChanged(Server.Instance.State.Value);
        }

        public void OnDestroy()
        {
            if(Server.Instance != null && Server.Instance.gameObject != null) Server.Instance.State.OnValueChangedHandler -= OnServerStateChanged;
        }

        public void OnFlagsChanged(ButtonCanvasFlag flags)
        {
            ExitButton.gameObject.SetActive((flags & ButtonCanvasFlag.Exit) != 0);
            RandomButton.gameObject.SetActive((flags & ButtonCanvasFlag.Random) != 0);
            InfoButton.gameObject.SetActive((flags & ButtonCanvasFlag.Info) != 0);
        }

        public void OnExitClicked()
        {
            switch (Server.Instance.State.Value)
            {
                case ServerState.LabyrinthReplay:
                case ServerState.ReplaySelect:
                case ServerState.LevelSelect:
                    Server.Instance.ReturnToGame();
                    break;

                //case ServerState.ReplaySelect:
                //case ServerState.Round:
                //case ServerState.Quickplay:
                case ServerState.Lobby:
                    Server.Instance.StartMenu();
                    break;

            }
        }

        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LabyrinthReplay:
                    //Flags.Set(ButtonCanvasFlag.Exit | ButtonCanvasFlag.Info);
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