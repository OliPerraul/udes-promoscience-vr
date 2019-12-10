using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience.Labyrinths.Editor.UI
{

    public class SelectedTileDisplay : Cirrus.BaseSingleton<SelectedTileDisplay>
    {
        [SerializeField]
        private UnityEngine.UI.Text selectedTileText;

        public void Awake()
        {
            EditorDrawer.Instance.SelectedTileType.OnValueChangedHandler += OnSelectedTileChanged;
        }

        public void OnSelectedTileChanged(TileType tile)
        {
            selectedTileText.text = System.Enum.GetName(typeof(TileType), tile);
        }
    }
}