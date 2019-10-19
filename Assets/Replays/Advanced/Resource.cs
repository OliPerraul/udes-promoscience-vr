﻿using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays.Advanced
{
    public class Resource : ScriptableObject
    {
        [SerializeField]
        public int MaxHorizontal = 2;

        [SerializeField]
        public float SelectionOffset = 60;
    }
}