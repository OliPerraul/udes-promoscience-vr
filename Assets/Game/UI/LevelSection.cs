using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Labyrinths.UI
{

    public class LevelSection : BaseSection
    {
        [SerializeField]
        private LevelSelectionInterface levelSelect;

        public override BaseSelectionInterface Select => levelSelect;

        [SerializeField]
        private LevelButton buttonTemplate;

        public override BaseButton ButtonTemplate => buttonTemplate;
    }
}
