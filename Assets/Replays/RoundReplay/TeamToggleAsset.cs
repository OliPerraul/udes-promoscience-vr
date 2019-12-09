using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays
{
    // Only one replaytoggle interface asset
    // replay asset did not make sense because multiple replay exist
    public class TeamToggleAsset : MonoBehaviour
    {
        public Cirrus.Event<Course, bool> OnCourseToggledHandler;


    }
}
