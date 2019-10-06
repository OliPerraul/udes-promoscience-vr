using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Replay;

public interface ISequence
{
    void HandleAction(ReplayAction action, params object[] args);
}
