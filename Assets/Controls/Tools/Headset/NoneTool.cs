using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// No tool selected
    /// </summary>
    public class NoneTool : HeadsetTool
    {
        public override ToolId Id => ToolId.None;
    }
}