using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnScriptableAction : MonoBehaviour
{
    [SerializeField]
    ScriptableAction scriptableAction;

    [SerializeField]
    List<GameObject> gameObjectsToActivate = new List<GameObject>();

    [SerializeField]
    List<GameObject> gameObjectsToHide = new List<GameObject>();

    void Start()
    {
        scriptableAction.action += OnScriptableAction;
    }

    void OnScriptableAction()
    {
        foreach (GameObject gObject in gameObjectsToHide)
        {
            gObject.SetActive(false);
        }

        foreach (GameObject gObject in gameObjectsToActivate)
        {
            gObject.SetActive(true);
        }
    }
}
