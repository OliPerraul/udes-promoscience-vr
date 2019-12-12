using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays.UI
{
    public class SelectedTeamAsset : ScriptableObject
    {
        public Cirrus.Event<TeamReplay> OnReplayCourseSelectedHandler;
    }
}
