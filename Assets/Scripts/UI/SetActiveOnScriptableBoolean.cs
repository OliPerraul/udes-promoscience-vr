using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.UI
{

    public class SetActiveOnScriptableBoolean : MonoBehaviour
    {
        [SerializeField]
        ScriptableBoolean scriptableBoolean;

        [SerializeField]
        List<GameObject> gameObjectsToActivateOnTrue = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnTrue = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToActivateOnFalse = new List<GameObject>();

        [SerializeField]
        List<GameObject> gameObjectsToHideOnFalse = new List<GameObject>();

        void OnEnable()
        {
            scriptableBoolean.valueChangedEvent += OnScriptableBooleanValueChanged;
        }

        void OnScriptableBooleanValueChanged()
        {
            if (scriptableBoolean.Value == true)
            {
                foreach (GameObject gObject in gameObjectsToHideOnTrue)
                {
                    gObject.SetActive(false);
                }

                foreach (GameObject gObject in gameObjectsToActivateOnTrue)
                {
                    gObject.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject gObject in gameObjectsToHideOnFalse)
                {
                    gObject.SetActive(false);
                }

                foreach (GameObject gObject in gameObjectsToActivateOnFalse)
                {
                    gObject.SetActive(true);
                }
            }
        }
    }
}
