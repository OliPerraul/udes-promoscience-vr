using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class ProgessionBar : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState clientState;

        [SerializeField]
        GameObject progress;

        void OnEnable()
        {
            clientState.OnRespectChangedHandler += OnValueChanged;
        }


        void OnValueChanged(float value)
        {
            progress.transform.localScale = new Vector3(value, 1, 1);
        }
    }
}
