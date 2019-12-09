using UnityEngine;
using System.Collections;
//using static UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Controls
{
    public class HeadsetToolManagerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<ToolId> CurrentTool = new Cirrus.ObservableValue<ToolId>();

    }
}