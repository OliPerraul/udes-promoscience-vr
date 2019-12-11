using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{

    public enum ToolId : int
    {
        None = 0,
        FlightDistanceScanner = 1,
        WallDistanceScanner = 2,
        Compass = 3,
        AlgorithmClipboard = 4,
        PaintBucket = 5
    }

    public abstract class BaseTool : MonoBehaviour
    {
        public abstract ToolId Id { get; }



    }

}