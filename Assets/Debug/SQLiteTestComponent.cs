using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Tests
{
    public class SQLiteTestComponent : MonoBehaviour
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        void Start()
        {
            SQLiteUtilities.CreateDatabaseIfItDoesntExist();
        }

#endif
    }

}
