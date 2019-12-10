using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths.UI;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{

    public class EditorSelectSection : BaseSection
    {
        [SerializeField]
        private EditorSelectionInterface levelSelect;

        public override BaseSelectionInterface Select => levelSelect;

        [SerializeField]
        private EditorSelectButton buttonTemplate;

        public override BaseButton ButtonTemplate => buttonTemplate;

        
    }
}
