using UnityEngine;
using System.Collections;
using Cirrus;

namespace UdeS.Promoscience.Replays.UI
{

    public class SidebarAsset : ScriptableObject
    {
        public ObservableValue<bool> IsToggleAlgorithm = new ObservableValue<bool>();

        public ObservableValue<bool> IsToggleGreyboxLabyrinth = new ObservableValue<bool>();

    }
}