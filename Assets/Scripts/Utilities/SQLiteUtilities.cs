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

    public static void ReadLabyrinthDataFromId(int id, ref int sizeX, ref int sizeY, ref int[] data)//Modifier pour envoyer le labyrinth en référence et le modifier directement (Read labyrinth)
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
                    cmd.CommandText = "SELECT * FROM DevicePairing WHERE TabletID='" + id + "'";
                }
                else if(deviceType == DeviceType.Headset)
                {
                    cmd.CommandText = "SELECT * FROM DevicePairing WHERE HeadsetID='" + id + "'";
                }

                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (deviceType == DeviceType.Tablet)
                        {
                            pairedId = reader["HeadsetID"].ToString();
                        }
                        else if (deviceType == DeviceType.Headset)
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
                    cmd.CommandText = "DELETE FROM DevicePairing WHERE TabletID='" + id + "'";
                }
                else if(deviceType == DeviceType.Headset)
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

        RemovePairing(tabletId, DeviceType.Tablet);
        RemovePairing(headsetId, DeviceType.Headset);

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

                using (SqliteDataReader reader = cmd.ExecuteReader())//Changer dans une function
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

                using (SqliteDataReader reader = cmd.ExecuteReader())//Changer dans une function
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

    public static int GetNextTeamID()//Retravailler
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

    public static int GetNextCourseID()//Encasuler dans une fonction avec paramètres
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

    static void InsertBasicLabyrinths()//Lire d'une string
    {
        //Labyrinth 1
        int id = 1;
        int sizeX = 10;
        int sizeY = 10;

        int[] data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_START_1_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_END_1_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        SQLiteUtilities.InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

        //Labyrinth 2
        id = 2;
        sizeX = 10;
        sizeY = 10;

        data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_END_1_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_START_1_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

        //Labyrinth 3
        id = 3;
        sizeX = 10;
        sizeY = 10;

        data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_END_1_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_START_1_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);
    }
}

#endif
