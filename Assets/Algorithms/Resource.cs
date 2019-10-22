using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Algorithms
{
    public class Resource : ScriptableObject
    {
        [SerializeField]
        public LocalizeString name;

        public string Name
        {
            get
            {
                return name == null ? "[?]" : name.Value;                
            }
        }
    }
}