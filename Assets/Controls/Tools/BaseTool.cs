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
        [SerializeField]
        private bool isLeftHanded = false;

        public bool IsLeftHanded => isLeftHanded;

        [SerializeField]
        private bool isThirdPerson = false;

        public bool IsThirdPerson => isThirdPerson;

        public abstract ToolId Id { get; }


        public virtual void OnValidate()
        {
            if (transform.parent == null) return;
            
            isThirdPerson = transform.parent.name.Contains("ThirdPerson");
            isLeftHanded = transform.parent.name.Contains("LeftHanded");
        }

    }
}
