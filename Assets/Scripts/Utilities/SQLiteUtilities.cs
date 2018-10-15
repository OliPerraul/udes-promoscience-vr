#if UNITY_EDITOR || UNITY_STANDALONE_WIN

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public static class SQLiteUtilities
{
    const string fileName = "algorintheDatabase.db";

    static void CreateDatabase()
    {
        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Team ( " +
                                  "TeamID INTEGER(10) NOT NULL, " +
                                  "Name varchar(255) NOT NULL, " +
                                  "Color varchar(255) NOT NULL, " +
                                  "CreationDateTime DATETIME NOT NULL, " +
                                  "PRIMARY KEY(TeamID) );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Labyrinth ( " +
                  "LabyrinthID INTEGER(10) NOT NULL, " +
                  "Specs varchar(255), " +
                  "PRIMARY KEY(LabyrinthID) );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Course ( " +
                  "CourseID INTEGER(10) NOT NULL, " +
                  "TeamID INTEGER(10) NOT NULL, " +
                  "LabyrinthID INTEGER(10) NOT NULL, " +
                  "NoAlgo INTEGER(10) NOT NULL, " +
                  "PRIMARY KEY(CourseID), " +
                  "FOREIGN KEY(TeamID) REFERENCES Team(TeamID), " +
                  "FOREIGN KEY(LabyrinthID) REFERENCES Labyrinth(LabyrinthID)); ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Event ( " +
                                  "EventID INTEGER PRIMARY KEY ASC, " +
                                  "Type INTEGER(10) NOT NULL, " +
                                  "Time DATETIME NOT NULL, " +
                                  "CourseID INTEGER(10) NOT NULL, " +
                                  "FOREIGN KEY(CourseID) REFERENCES Course(CourseID) ); ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS DevicePairing ( " +
                                  "TabletID varchar(255) NOT NULL UNIQUE, " +
                                  "HeadsetID varchar(255) NOT NULL UNIQUE, " +
                                  "PRIMARY KEY(TabletID,HeadsetID)); ";
                cmd.ExecuteNonQuery();
            }
        }
    }

    static void CreateDatabaseIfItDoesntExist()
    {
        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        if (!System.IO.File.Exists(dbPath))
        {
            CreateDatabase();
        }
    }


    public static void FillTableWithTestData()
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "PRAGMA foreign_keys = ON";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Team (TeamID, Name, Color, CreationDateTime) VALUES (1, 1, 1, DATETIME('2018-08-27',  '13:10:10'));";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Team (TeamID, Name, Color) VALUES (2, 2, 2, DATETIME('2018-08-27',  '13:10:10'));";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Labyrinth (LabyrinthID, Specs) VALUES (1, 'ahah1');";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Labyrinth (LabyrinthID, Specs) VALUES (2, 'ahah2');";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Course (CourseID, TeamID, LabyrinthID, NoAlgo) VALUES (1, 2, 1, 55);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Course (CourseID, TeamID, LabyrinthID, NoAlgo) VALUES (2, 2, 2, 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Course (CourseID, TeamID, LabyrinthID, NoAlgo) VALUES (3, 1, 2, 154);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, CourseID) VALUES (1, 33, DATETIME('2018-08-27',  '13:10:10'), 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, CourseID) VALUES (2, 35, DATETIME('2018-08-27',  '13:10:20'), 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, CourseID) VALUES (3, 10, DATETIME('2018-08-27',  '13:10:15'), 1);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, CourseID) VALUES (4, 10, DATETIME('2018-08-27',  '15:10:10'), 1);";
                cmd.ExecuteNonQuery();
            }
        }
    }


    public static void ReadDatabase()
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())   
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Team";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("TeamId: " + reader["TeamId"] + "\t Name: " + reader["Name"] + "\t Color: " + reader["Color"] + "\t CreationDateTime: " + reader["CreationDateTime"]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Labyrinth";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("LabyrinthID: " + reader["LabyrinthID"] + "\t Specs: " + reader["Specs"]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Course";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("CourseID: " + reader["CourseID"] + "\t TeamId: " + reader["TeamId"] + "\t LabyrinthID: " + reader["LabyrinthID"] + "\t NoAlgo: " + reader["NoAlgo"]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Event";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("EventID: " + reader["EventID"] + "\t Type: " + reader["Type"] + "\t Time: " + reader["Time"] + "\t CourseID: " + reader["CourseID"]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM DevicePairing";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("TabletID: " + reader["TabletID"] + "\t HeadsetID: " + reader["HeadsetID"]);
                    }

                    reader.Close();
                }
            }
        }
    }

    public static void InsertOrReplaceLabyrinth(int id, int sizeX, int sizeY, int[] data)
    {
        CreateDatabaseIfItDoesntExist();
        string specs = sizeX + " " + sizeY;
         
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            specs += " " + data[i];
        }

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT OR REPLACE INTO Labyrinth (LabyrinthID, Specs) VALUES ('" + id + "', '" + specs + "');";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void SetLabyrintheDataWithId(int id, ref int sizeX, ref int sizeY, ref int[] data)
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Labyrinth WHERE LabyrinthID=" + id;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("LabyrinthID: " + reader["LabyrinthID"] + "\t Specs: " + reader["Specs"]);

                        string[] specs = reader["Specs"].ToString().Split();
                        int[] labyrinthSpecs = Array.ConvertAll(specs, int.Parse);
                        sizeX = labyrinthSpecs[0];
                        sizeY = labyrinthSpecs[1];
                        data = new int[sizeX * sizeY];

                        for(int i = 0; i < sizeX * sizeY; i++)
                        {
                            data[i] = labyrinthSpecs[i + 2];
                        }
                    }

                    reader.Close();
                }
            }
        }
    }

    public static string GetPairing(string id, int deviceType)
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;
        string pairedId = null;
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                if (deviceType == Constants.DEVICE_TABLET)
                {
                    cmd.CommandText = "SELECT * FROM DevicePairing WHERE TabletID='" + id + "'";
                }
                else if(deviceType == Constants.DEVICE_HEADSET)
                {
                    cmd.CommandText = "SELECT * FROM DevicePairing WHERE HeadsetID='" + id + "'";
                }

                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (deviceType == Constants.DEVICE_TABLET)
                        {
                            pairedId = reader["HeadsetID"].ToString();
                        }
                        else if (deviceType == Constants.DEVICE_HEADSET)
                        {
                            pairedId = reader["TabletID"].ToString();
                        }
                    }

                    reader.Close();
                }
            }
        }
        return pairedId;
    }

    static void RemovePairing(string id,int deviceType)
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
;
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                if(deviceType == Constants.DEVICE_TABLET)
                {
                    cmd.CommandText = "DELETE FROM DevicePairing WHERE TabletID='" + id + "'";
                }
                else if(deviceType == Constants.DEVICE_HEADSET)
                {
                    cmd.CommandText = "DELETE FROM DevicePairing WHERE HeadsetID='" + id + "'";
                }
                
                cmd.ExecuteNonQuery();
            }
        }
    }


    public static void InsertPairing(string tabletId,string headsetId)
    {
        CreateDatabaseIfItDoesntExist();

        RemovePairing(tabletId, 0);
        RemovePairing(headsetId, 1);

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "INSERT INTO DevicePairing (TabletID, HeadsetID) VALUES ( '" + tabletId + "', '" + headsetId + "');";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void InsertPlayerAction(int teamId, string teamName, string teamColor, int courseId, int labyrinthId, int algorithmId, int eventType, string date, string time)
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "PRAGMA foreign_keys = ON";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT Count(*) FROM Team WHERE TeamID='" + teamId + "'";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            Debug.Log("No team with id : " + teamId);
                            reader.Close();
                            cmd.CommandText = "INSERT INTO Team (TeamID, Name, Color, CreationDateTime) VALUES ('" + teamId + "', '" + teamName + "', '" + teamColor + "', DATETIME('" + date + "', '" + time + "'));";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }

                cmd.CommandText = "SELECT Count(*) FROM Course WHERE CourseID='" + courseId
                    + "' AND TeamID='" + teamId
                    + "' AND LabyrinthID='" + labyrinthId
                    + "' AND NoAlgo='" + algorithmId + "'";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            Debug.Log("No course with specifics");
                            reader.Close();
                            cmd.CommandText = "INSERT INTO Course (CourseID, TeamId, LabyrinthID, NoAlgo) VALUES ('" + courseId + "', '" + teamId + "', '" + labyrinthId + "', '" + algorithmId + "');";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }

                cmd.CommandText = "INSERT INTO Event (Type, Time, CourseID) VALUES ('" + eventType + "',  DATETIME('" + date + "', '" + time + "'), '" + courseId + "');";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static int GetNextTeamID()
    {
        int teamId = 0;
        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Count(*) FROM Team";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        teamId = reader.GetInt32(0);
                    }
                    reader.Close();
                }

                bool isNotUnique = true;
                while(isNotUnique)
                {
                    teamId++;
                    cmd.CommandText = "SELECT Count(*) FROM Team WHERE TeamID='" + teamId + "'";
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetInt32(0) == 0)
                            {
                                isNotUnique = false;
                            }
                        }
                        else
                        {
                            isNotUnique = false;
                        }
                    }
                }
            }
        }

        return teamId;
    }

    public static int GetNextCourseID()
    {
        int courseId = 0;
        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT Count(*) FROM Course";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        courseId = reader.GetInt32(0);
                    }
                    reader.Close();
                }

                bool isNotUnique = true;
                while (isNotUnique)
                {
                    courseId++;

                    cmd.CommandText = "SELECT Count(*) FROM Course WHERE CourseID='" + courseId + "'";
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetInt32(0) == 0)
                            {
                                isNotUnique = false;
                            }
                        }
                        else
                        {
                            isNotUnique = false;
                        }
                    }
                }
            }
        }

        return courseId;
    }
}

#endif
