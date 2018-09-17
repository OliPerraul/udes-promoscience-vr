using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgessionBar : MonoBehaviour
{
    [SerializeField]
    ScriptableFloat f;

    [SerializeField]
    GameObject progress;

	void Start ()
    {
        f.valueChangedEvent += OnValueChanged;
	}


    void OnValueChanged()
    {
        progress.transform.localScale = new Vector3(f.value, 1, 1);
    }
}
