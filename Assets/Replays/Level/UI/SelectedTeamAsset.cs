using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays.UI
{
    /// <summary>
    /// Shared state for the selected team display
    /// </summary>
    public class SelectedTeamAsset : ScriptableObject
    {
        public Cirrus.Event<TeamReplay> OnReplayCourseSelectedHandler;
    }
}
