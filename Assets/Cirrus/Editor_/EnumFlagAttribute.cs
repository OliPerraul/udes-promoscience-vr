using System;
using UnityEngine;

namespace Cirrus.Editor
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumFlagAttribute : PropertyAttribute
    {
        public bool alwaysFoldOut;
        public EnumMaskLayout layout = EnumMaskLayout.Vertical;
    }

    public enum EnumMaskLayout
    {
        Vertical,
        Horizontal
    }
}
