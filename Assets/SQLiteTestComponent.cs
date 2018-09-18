using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteTestComponent : MonoBehaviour
{
	void Start ()
    {
        int teamId = 1;
        int teamName = 1;
        int teamColor = 1;
        int courseId = 1;
        int labyrithId = 1;
        int algorithmId = 1;
        int eventType = 1;
        System.DateTime now = System.DateTime.Now;

        SQLiteUtilities.InsertPlayerAction(teamId, teamName, teamColor, courseId, labyrithId, algorithmId, eventType, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss"));

        SQLiteUtilities.ReadDatabase();
    }
	

}
