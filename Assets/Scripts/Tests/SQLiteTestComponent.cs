using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteTestComponent : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    void Start ()
    {
        SQLiteUtilities.CreateDatabaseIfItDoesntExist();
    }

#endif
}
