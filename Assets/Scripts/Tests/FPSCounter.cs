using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsDisplay;

    public void Update()
    {
        string fpsText = (int)(1f / Time.unscaledDeltaTime) + " FPS";
        fpsDisplay.text = fpsText;
    }
}
 