using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays
{

    /// <summary>
    /// Shared state of the team toggle interface
    /// </summary>
    public class TeamToggleAsset : ScriptableObject
    {
        public Cirrus.Event<Course, bool> OnCourseToggledHandler;

        public Cirrus.Event<Course> OnCourseSelectedHandler;

        public Cirrus.Event<Course> OnReplayCourseAddedHandler;

        public Cirrus.Event<Course> OnReplayCourseRemovedHandler;

        public Cirrus.Event OnReplayCourseRemoveAllHandler;


    }
}
