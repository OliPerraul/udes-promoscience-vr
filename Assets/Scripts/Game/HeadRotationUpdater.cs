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
		if(headTransform.rotation.eulerAngles != headRotation.Value)
        {
            headRotation.Value = headTransform.localRotation.eulerAngles;
        }
	}
}
