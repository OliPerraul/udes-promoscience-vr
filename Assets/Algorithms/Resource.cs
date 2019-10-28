using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Algorithms
{
    public class Resource : ScriptableObject
    {
        [SerializeField]
        public LocalizeInlineString name;

        public string Name
        {
            get
            {
                return name.Value;                
            }
        }

        [SerializeField]
        public LocalizeString description;

        public string Description
        {
            get
            {
                return description.Value;
            }
        }
    }
}