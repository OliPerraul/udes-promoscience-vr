using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Directive", order = 1)]
    public class DirectiveManagerAsset : ScriptableObject
    {
        [SerializeField]
        public Cirrus.MonitoredValue<Directive> Directive = new Cirrus.MonitoredValue<Directive>();

        public void Set(Directive directive)
        {
            Directive.Value = directive;
        }

    }
}

