using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Menu
{
    public class BinaryBorder : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        private const int MaxLength = 1028;
        
        // Update is called once per frame
        void FixedUpdate()
        {
            uint random = (uint) UnityEngine.Random.Range(0, 255);
            string binary = Convert.ToString(random, 2);

            if (text.text.Length > MaxLength)
            {
                text.text = text.text.Substring(0, MaxLength);
            }


            text.text = binary + " " + text.text;

        }
    }
}
