using UnityEngine;
using System.Collections;

// Sorry Lol: we are too fare gone with this mess

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Misc", order = 1)]
    public class ScriptableClientGameData : ScriptableObject
    {
        public string[] ActionValues;
    }
}