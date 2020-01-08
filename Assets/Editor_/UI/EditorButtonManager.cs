using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    public enum EditorButtonFlag
    {

        BottomExit = 1 << 3,
        BottomInfo = 1 << 4,

        BottomLeftTrash = 1 << 5,
        BottomLeftSave = 1 << 6,
    }

    /// <summary>
    /// Manages editor main buttons
    /// </summary>
    public class EditorButtonManager : Cirrus.BaseSingleton<EditorButtonManager>
    {
        [SerializeField]
        public UnityEngine.UI.Button BottomExitButton;

        [SerializeField]
        public UnityEngine.UI.Button BottomInfoButton;

        [SerializeField]
        public UnityEngine.UI.Button BottomLeftTrash;

        [SerializeField]
        public UnityEngine.UI.Button BottomLeftSave;


        [SerializeField]
        private Cirrus.SceneWrapperAsset StartScene;

        public Cirrus.ObservableValue<EditorButtonFlag> Flags = new Cirrus.ObservableValue<EditorButtonFlag>();

        public Cirrus.Event<EditorButtonFlag> OnClickHandler;


        public void Awake()
        {
            EditorController.Instance.State.OnValueChangedHandler += OnServerStateChanged;

            Flags.OnValueChangedHandler += OnFlagsChangedHandler;

            BottomExitButton.onClick.AddListener(() => OnClickHandler?.Invoke(EditorButtonFlag.BottomExit));
            BottomInfoButton.onClick.AddListener(() => OnClickHandler?.Invoke(EditorButtonFlag.BottomInfo));
            BottomLeftTrash.onClick.AddListener(() => OnClickHandler?.Invoke(EditorButtonFlag.BottomLeftTrash));
            BottomLeftSave.onClick.AddListener(() => OnClickHandler?.Invoke(EditorButtonFlag.BottomLeftSave));
        }


        public void Start()
        {
            OnServerStateChanged(EditorController.Instance.State.Value);
        }

        public void OnDestroy()
        {
            EditorController.Instance.State.OnValueChangedHandler += OnServerStateChanged;
        }

        public void OnFlagsChangedHandler(EditorButtonFlag flags)
        {
            BottomExitButton.gameObject.SetActive((flags & EditorButtonFlag.BottomExit) != 0);
            BottomInfoButton.gameObject.SetActive((flags & EditorButtonFlag.BottomInfo) != 0);
            BottomLeftTrash.gameObject.SetActive((flags & EditorButtonFlag.BottomLeftTrash) != 0);
            BottomLeftSave.gameObject.SetActive((flags & EditorButtonFlag.BottomLeftSave) != 0);
        }

        public void OnServerStateChanged(EditorState state)
        {
            switch (state)
            {
                case EditorState.Select:
                    //Flags.Set(ButtonCanvasFlag.Exit | ButtonCanvasFlag.Info);
                    Flags.Set(EditorButtonFlag.BottomExit);
                    break;

                case EditorState.Editor:
                    Flags.Set(EditorButtonFlag.BottomExit | EditorButtonFlag.BottomInfo);
                    break;

            }
        }


    }
}