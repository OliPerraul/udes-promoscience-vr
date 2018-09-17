using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    ScriptableBoolean isTrigger;

    void OnCollisionEnter(Collision collision)
    {
        isTrigger.value = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isTrigger.value = false;
    }

}
