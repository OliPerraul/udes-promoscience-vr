using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths;

namespace UdeS.Promoscience.Replays
{
    public class InstantReplay : LabyrinthReplay
    {
        public InstantReplay(ScriptableController controller, IData labyrinth) : base(controller, labyrinth) { }

        public override void Start()
        {
            base.Start();          

            Resume();

            controller.SendAction(ReplayAction.ToggleOptions, false);
        }
    }
}