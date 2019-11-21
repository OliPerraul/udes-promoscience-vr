using System;
using UnityEngine;

namespace Cirrus.Editor
{

    /// <summary>
    /// Display multi-select popup for Flags enum correctly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumFlagAttribute : PropertyAttribute
    {
    }

}