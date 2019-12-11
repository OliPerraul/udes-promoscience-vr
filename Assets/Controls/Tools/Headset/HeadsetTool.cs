using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience.Controls
{

    public abstract class HeadsetTool : BaseTool
    {
        [SerializeField]
        private bool isLeftHanded = false;

        public bool IsLeftHanded => isLeftHanded;

        [SerializeField]
        private bool isThirdPerson = false;

        public bool IsThirdPerson => isThirdPerson;

        public virtual void OnValidate()
        {
            if (transform.parent == null) return;
            
            isThirdPerson = transform.parent.name.Contains("ThirdPerson");
            isLeftHanded = transform.parent.name.Contains("LeftHanded");
        }

    }
}
