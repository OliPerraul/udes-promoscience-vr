using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{

    public class EditorInterface : Cirrus.BaseSingleton<EditorInterface>
    {
        public Cirrus.Event OnSaveHandler;

        public Cirrus.Event OnTrashHandler;

        private bool isInfoToggled = false;

        public void Awake()
        {
            EditorButtonManager.Instance.OnClickHandler += OnButtonClicked;
        }

        public void Start()
        {
            OnButtonClicked(EditorButtonFlag.BottomInfo);
            OnButtonClicked(EditorButtonFlag.BottomInfo);
        }

        public void OnButtonClicked(EditorButtonFlag button)
        {
            switch(button)
            {
                case EditorButtonFlag.BottomInfo:
                    isInfoToggled = !isInfoToggled;
                    ThemeSelectionInterface.Instance.gameObject.SetActive(isInfoToggled);
                    SelectedTileDisplay.Instance.gameObject.SetActive(isInfoToggled);

                    EditorButtonManager.Instance.Flags.Value = isInfoToggled ?
                        EditorButtonFlag.BottomExit | EditorButtonFlag.BottomInfo | EditorButtonFlag.BottomLeftSave | EditorButtonFlag.BottomLeftTrash :
                        EditorButtonFlag.BottomExit | EditorButtonFlag.BottomInfo;                        
                    break; 

                case EditorButtonFlag.BottomLeftSave:
                    OnSaveHandler?.Invoke();
                    break;

                case EditorButtonFlag.BottomLeftTrash:
                    OnSaveHandler?.Invoke();
                    break;
            }
        }

    }
}