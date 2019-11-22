using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Labyrinths.UI
{

    public class LevelSection : BaseSection
    {
        [SerializeField]
        private LevelSelect levelSelect;

        public override BaseSelect Select => levelSelect;
    }
}
