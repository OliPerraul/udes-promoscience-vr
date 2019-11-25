using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths.UI;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{

    public class LabyrinthEditorSection : BaseSection
    {
        [SerializeField]
        private LabyrinthEditorSelect levelSelect;

        public override BaseSelect Select => levelSelect;

        [SerializeField]
        private LabyrinthEditorButton buttonTemplate;

        public override BaseButton ButtonTemplate => buttonTemplate;

        
    }
}
