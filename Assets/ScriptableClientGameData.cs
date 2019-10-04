using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Misc", order = 1)]
    public class ScriptableClientGameData : ScriptableObject
    {
        public string[] ActionValues;

        public int[] ActionSteps;
    }
}