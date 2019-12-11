using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience;
using UnityEngine;

namespace Cirrus
{
    [CreateAssetMenu(menuName = "Cirrus/Application Wrapper")]
    public class ApplicationWrapperAsset : ScriptableObject
    {
        public void Quit()
        {
            Application.Quit();
        }
    }
}