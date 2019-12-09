using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays
{
    // Only one replaytoggle interface asset
    // replay asset did not make sense because multiple replay exist
    public class TeamToggleAsset : ScriptableObject
    {
        public Cirrus.Event<Course, bool> OnCourseToggledHandler;

        public Cirrus.Event<Course> OnCourseSelectedHandler;

        public Cirrus.Event<Course> OnReplayCourseAddedHandler;

        public Cirrus.Event<Course> OnReplayCourseRemovedHandler;


    }
}
