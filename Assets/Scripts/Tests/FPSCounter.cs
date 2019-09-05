using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UdeS.Promoscience.Tests
{

    public class FPSCounter : MonoBehaviour
    {
        public Text fpsDisplay;

        public void Update()
        {
            string fpsText = (int)(1f / Time.unscaledDeltaTime) + " FPS";
            fpsDisplay.text = fpsText;
        }
    }
}
 