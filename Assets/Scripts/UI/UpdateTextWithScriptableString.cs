using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextWithScriptableString : MonoBehaviour
{
    [SerializeField]
    ScriptableString scriptableString;

    [SerializeField]
    Text text;

	void Start ()
    {
        scriptableString.valueChangedEvent += OnValueChangedEvent;
    }
	
	void OnValueChangedEvent()
    {
        text.text = scriptableString.value;
	}
}
