using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnScriptableBoolean : MonoBehaviour
{
    [SerializeField]
    ScriptableBoolean scriptableBoolean;

    [SerializeField]
    GameObject gameObjectToActivate;


	void Start ()
    {
        scriptableBoolean.valueChangedEvent += OnScriptableBooleanValueChanged;
    }
	
	void OnScriptableBooleanValueChanged()
    {
        if(scriptableBoolean.value == true)
        {
            gameObjectToActivate.SetActive(true);
        }
        else
        {
            gameObjectToActivate.SetActive(false);
        }
    }
}
