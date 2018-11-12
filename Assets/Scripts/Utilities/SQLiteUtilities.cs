#if UNITY_EDITOR || UNITY_STANDALONE_WIN

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public static class SQLiteUtilities
{
    const string fileName = "AlgorintheDatabase.db";

    //Database table 
    const string TEAM = "Team";
    const string LABYRINTH = "Labyrinth";
    const string COURSE = "Course";
    const string EVENT = "Event";
    const string DEVICE_PAIRING = "DevicePairing";

    //Team table column
    const string TEAM_ID = "TeamID";
    const string TEAM_NAME = "TeamName";
    const string TEAM_COLOR = "TeamColor";
    const string TEAM_CREATION_DATE_TIME = "TeamCreationDateTime";

    //Labyrinth table column
    const string LABYRINTH_ID = "LabyrinthID";
    const string LABYRINTH_SPECS = "LabyrinthSpecs";

    //Course table column
    const string COURSE_ID = "CourseID";
    const string COURSE_TEAM_ID = TEAM_ID;
    const string COURSE_LABYRINTH_ID = LABYRINTH_ID;
    const string COURSE_NO_ALGO = "NoAlgo";

    //Event table column
    const string EVENT_ID = "EventID";
    const string EVENT_TYPE = "EventType";
    const string EVENT_TIME = "EventTime";
    const string EVENT_COURSE_ID = COURSE_ID;

    //DevicePairing table column
    const string DEVICE_PAIRING_TABLET_ID = "TabletID";
    const string DEVICE_PAIRING_HEADSET_ID = "HeadsetID";

    static void CreateDatabase()
    {
        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TEAM + "( " +
                                  TEAM_ID + " INTEGER(10) NOT NULL, " +
                                  TEAM_NAME + " varchar(255) NOT NULL, " +
                                  TEAM_COLOR + " varchar(255) NOT NULL, " +
                                  TEAM_CREATION_DATE_TIME + " DATETIME NOT NULL, " +
                                  "PRIMARY KEY(" + TEAM_ID + ") );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + LABYRINTH + " ( " +
                                  LABYRINTH_ID + " INTEGER(10) NOT NULL, " +
                                  LABYRINTH_SPECS + " varchar(255), " +
                                  "PRIMARY KEY(" + LABYRINTH_ID + ") );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + COURSE + " ( " +
                                  COURSE_ID + " INTEGER(10) NOT NULL, " +
                                  COURSE_TEAM_ID + " INTEGER(10) NOT NULL, " +
                                  COURSE_LABYRINTH_ID + " INTEGER(10) NOT NULL, " +
                                  "NoAlgo INTEGER(10) NOT NULL, " +
                                  "PRIMARY KEY(" + COURSE_ID + "), " +
                                  "FOREIGN KEY(" + COURSE_TEAM_ID + ") REFERENCES " + TEAM +"(" + TEAM_ID + "), " +
                                  "FOREIGN KEY(" + COURSE_LABYRINTH_ID + ") REFERENCES " + LABYRINTH +"(" + LABYRINTH_ID + ")); ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + EVENT + " ( " +
                                  EVENT_ID + " INTEGER PRIMARY KEY ASC, " +
                                  EVENT_TYPE + " INTEGER(10) NOT NULL, " +
                                  EVENT_TIME + " DATETIME NOT NULL, " +
                                  EVENT_COURSE_ID + " INTEGER(10) NOT NULL, " +
                                  "FOREIGN KEY(" + EVENT_COURSE_ID + ") REFERENCES " + COURSE + "(" + COURSE_ID + ") ); ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + DEVICE_PAIRING + " ( " +
                                  DEVICE_PAIRING_TABLET_ID + " varchar(255) NOT NULL UNIQUE, " +
                                  DEVICE_PAIRING_HEADSET_ID + " varchar(255) NOT NULL UNIQUE, " +
                                  "PRIMARY KEY(" + DEVICE_PAIRING_TABLET_ID + "," + DEVICE_PAIRING_HEADSET_ID + ")); ";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void CreateDatabaseIfItDoesntExist()
    {
        string dbPath = Application.persistentDataPath + "/" + fileName;

        if (!System.IO.File.Exists(dbPath))
        {
            CreateDatabase();
            InsertBasicLabyrinths();
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
                
                cmd.CommandText = "INSERT INTO " + TEAM + " (" + TEAM_ID + ", " + TEAM_NAME + ", " + TEAM_COLOR + ", " + TEAM_CREATION_DATE_TIME + ") VALUES (1, 1, 1, DATETIME('2018-08-27',  '13:10:10'));";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + TEAM + " (" + TEAM_ID + ", " + TEAM_NAME + ", " + TEAM_COLOR + ", " + TEAM_CREATION_DATE_TIME + ") VALUES (2, 2, 2, DATETIME('2018-08-27',  '13:10:10'));";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO " + LABYRINTH + " (" + LABYRINTH_ID + ", " + LABYRINTH_SPECS + ") VALUES (1, 'ahah1');";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO " + LABYRINTH + " (" + LABYRINTH_ID + ", " + LABYRINTH_SPECS + ") VALUES (2, 'ahah2');";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ") VALUES (1, 2, 1, 55);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ") VALUES (2, 2, 2, 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ") VALUES (3, 1, 2, 154);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (1, 33, DATETIME('2018-08-27',  '13:10:10'), 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (2, 35, DATETIME('2018-08-27',  '13:10:20'), 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (3, 10, DATETIME('2018-08-27',  '13:10:15'), 1);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (4, 10, DATETIME('2018-08-27',  '15:10:10'), 1);";
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
                cmd.CommandText = "SELECT * FROM " + TEAM;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log( TEAM_ID + ": " + reader[TEAM_ID] + "\t " + TEAM_NAME + ": " + reader[TEAM_NAME] + "\t " + TEAM_COLOR + ": " + reader[TEAM_COLOR] + "\t " + TEAM_CREATION_DATE_TIME + ": " + reader[TEAM_CREATION_DATE_TIME]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM " + LABYRINTH;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log( LABYRINTH_ID + ": " + reader[LABYRINTH_ID] + "\t " + LABYRINTH_SPECS + ": " + reader[LABYRINTH_SPECS]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM " + COURSE;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log( COURSE_ID + ": " + reader[COURSE_ID] + "\t " + COURSE_TEAM_ID + ": " + reader[COURSE_TEAM_ID] + "\t " + COURSE_LABYRINTH_ID + ": " + reader[COURSE_LABYRINTH_ID] + "\t " + COURSE_NO_ALGO + ": " + reader[COURSE_NO_ALGO]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM " + EVENT;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log(EVENT_ID + ": " + reader[EVENT_ID] + "\t " + EVENT_TYPE + ": " + reader[EVENT_TYPE] + "\t " + EVENT_TIME + ": " + reader[EVENT_TIME] + "\t " + EVENT_COURSE_ID + ": " + reader[EVENT_COURSE_ID]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM " + DEVICE_PAIRING;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log(DEVICE_PAIRING_TABLET_ID + ": " + reader[DEVICE_PAIRING_TABLET_ID] + "\t " + DEVICE_PAIRING_HEADSET_ID + ": " + reader[DEVICE_PAIRING_HEADSET_ID]);
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
                cmd.CommandText = "INSERT OR REPLACE INTO " + LABYRINTH + " (" + LABYRINTH_ID + ", " + LABYRINTH_SPECS + ") VALUES ('" + id + "', '" + specs + "');";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void ReadLabyrinthDataFromId(int id, ref int sizeX, ref int sizeY, ref int[] data)
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM " + LABYRINTH + " WHERE " + LABYRINTH_ID + "=" + id;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string[] specs = reader[LABYRINTH_SPECS].ToString().Split();
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

    public static string GetPairing(string id, DeviceType deviceType)
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
                if (deviceType == DeviceType.Tablet)
                {
                    cmd.CommandText = "SELECT * FROM " + DEVICE_PAIRING + " WHERE " + DEVICE_PAIRING_TABLET_ID + "='" + id + "'";
                }
                else if(deviceType == DeviceType.Headset)
                {
                    cmd.CommandText = "SELECT * FROM " + DEVICE_PAIRING + " WHERE " + DEVICE_PAIRING_HEADSET_ID + "='" + id + "'";
                }

                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (deviceType == DeviceType.Tablet)
                        {
                            pairedId = reader[DEVICE_PAIRING_HEADSET_ID].ToString();
                        }
                        else if (deviceType == DeviceType.Headset)
                        {
                            pairedId = reader[DEVICE_PAIRING_TABLET_ID].ToString();
                        }
                    }

                    reader.Close();
                }
            }
        }
        return pairedId;
    }

    static void RemovePairing(string id, DeviceType deviceType)
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
                if(deviceType == DeviceType.Tablet)
                {
                    cmd.CommandText = "DELETE FROM " + DEVICE_PAIRING + " WHERE " + DEVICE_PAIRING_TABLET_ID + "='" + id + "'";
                }
                else if(deviceType == DeviceType.Headset)
                {
                    cmd.CommandText = "DELETE FROM " + DEVICE_PAIRING + " WHERE " + DEVICE_PAIRING_HEADSET_ID + "='" + id + "'";
                }
                
                cmd.ExecuteNonQuery();
            }
        }
    }


    public static void InsertPairing(string tabletId,string headsetId)
    {
        CreateDatabaseIfItDoesntExist();

        RemovePairing(tabletId, DeviceType.Tablet);
        RemovePairing(headsetId, DeviceType.Headset);

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "INSERT INTO " + DEVICE_PAIRING + " (" + DEVICE_PAIRING_TABLET_ID + ", " + DEVICE_PAIRING_HEADSET_ID + ") VALUES ( '" + tabletId + "', '" + headsetId + "');";
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

                cmd.CommandText = "SELECT Count(*) FROM " + TEAM + " WHERE " + TEAM_ID + "='" + teamId + "'";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            reader.Close();
                            cmd.CommandText = "INSERT INTO " + TEAM + " (" + TEAM_ID + ", " + TEAM_NAME + ", " + TEAM_COLOR + ", " + TEAM_CREATION_DATE_TIME + ") VALUES ('" + teamId + "', '" + teamName + "', '" + teamColor + "', DATETIME('" + date + "', '" + time + "'));";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }

                cmd.CommandText = "SELECT Count(*) FROM " + COURSE + " WHERE " + COURSE_ID + "='" + courseId
                    + "' AND " + COURSE_TEAM_ID + "='" + teamId
                    + "' AND " + COURSE_LABYRINTH_ID + "='" + labyrinthId
                    + "' AND " + COURSE_NO_ALGO + "='" + algorithmId + "'";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            Debug.Log("No course with specifics");
                            reader.Close();
                            cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ") VALUES ('" + courseId + "', '" + teamId + "', '" + labyrinthId + "', '" + algorithmId + "');";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }

                cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_TYPE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES ('" + eventType + "',  DATETIME('" + date + "', '" + time + "'), '" + courseId + "');";
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
                cmd.CommandText = "SELECT Count(*) FROM " + TEAM;
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
                    cmd.CommandText = "SELECT Count(*) FROM " + TEAM + " WHERE " + TEAM_ID + "='" + teamId + "'";
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

                cmd.CommandText = "SELECT Count(*) FROM " + COURSE;
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

                    cmd.CommandText = "SELECT Count(*) FROM " + COURSE + " WHERE " + COURSE_ID + "='" + courseId + "'";
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

    static void InsertBasicLabyrinths()
    {
        //Labyrinth Tutorial


        //Labyrinth 1
        int id = 1;
        int sizeX = 10;
        int sizeY = 11;

        int[] data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(0 * sizeX) + 10] = Constants.TILE_PTOL_TOWER_WALL_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_PTOL_START_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_PTOL_END_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_PTOL_FLOOR_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_PTOL_FLOOR_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_PTOL_FLOOR_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_PTOL_FLOOR_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(1 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(2 * sizeX) + 10] = Constants.TILE_PTOL_TOWER_WALL_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(3 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(4 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_PTOL_FLOOR_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_PTOL_FLOOR_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_PTOL_FLOOR_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(5 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_PTOL_FLOOR_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_PTOL_FLOOR_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(6 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_PTOL_FLOOR_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_PTOL_FLOOR_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(7 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_PTOL_FLOOR_ID;
        data[(8 * sizeX) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
        data[(9 * sizeX) + 10] = Constants.TILE_PTOL_TOWER_WALL_ID;

        SQLiteUtilities.InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

        //Labyrinth 2
        id = 2;
        sizeX = 10;
        sizeY = 10;

        data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_END_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_START_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_WALL_ID;

        InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

        //Labyrinth 3
        id = 3;
        sizeX = 10;
        sizeY = 10;

        data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_WALL_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_END_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_FLOOR_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_FLOOR_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_START_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_WALL_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_WALL_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_WALL_ID;

        InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);
    }
}

#endif
