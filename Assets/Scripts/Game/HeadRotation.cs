using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    [SerializeField]
    ScriptableVector3 headRotation;

    [SerializeField]
    Transform headTransform;

    private void Start()
    {
        headRotation.valueChangedEvent += SetRotation;
    }

    void SetRotation()
    {
		if(headTransform.rotation.eulerAngles != headRotation.value)
        {
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = headRotation.value;
            headTransform.localRotation = rotation;
        }
	}
}
