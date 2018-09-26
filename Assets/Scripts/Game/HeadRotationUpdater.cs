using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotationUpdater : MonoBehaviour
{
    [SerializeField]
    ScriptableVector3 headRotation;

    [SerializeField]
    Transform headTransform;

	void FixedUpdate ()
    {
		if(headTransform.rotation.eulerAngles != headRotation.value)
        {
            headRotation.value = headTransform.localRotation.eulerAngles;
        }
	}
}
