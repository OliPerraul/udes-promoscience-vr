﻿#if UNITY_EDITOR || UNITY_STANDALONE_WIN

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Utils
{
    public static class SQLiteUtilities
    {
        const string fileName = "AlgorintheDatabase.db";

        //Database table 
        const string TEAM = "Team";
        const string LABYRINTH = "Labyrinth";
        const string COURSE = "Course";
        const string EVENT = "Event";
        const string DEVICE_PAIRING = "DevicePairing";
        const string SERVER_GAME_INFORMATION = "ServerGameInformation";
        const string SERVER_PLAYER_INFORMATION = "ServerPlayerInformation";

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
        // Boolean value used to determine weather the given course is active or is
        // merely kept for playback
        const string COURSE_ACTIVE = "Active";

        //Event table column
        const string EVENT_ID = "EventID";
        const string EVENT_TYPE = "EventType";
        const string EVENT_VALUE = "EventValue"; // JSON value used only for divergent location
        const string EVENT_TIME = "EventTime";
        const string EVENT_COURSE_ID = COURSE_ID;

        //DevicePairing table column
        const string DEVICE_PAIRING_TABLET_ID = "TabletID";
        const string DEVICE_PAIRING_HEADSET_ID = "HeadsetID";

        //ServerGameInformation table column
        const string SERVER_GAME_INFORMATION_SAVE_SLOT_ID = "SaveSlotID";
        const string SERVER_GAME_STATE_ID = "ServerGameState";
        const string SERVER_GAME_ROUND = "ServerGameRound";

        //ServerPlayerInformation table column
        const string SERVER_PLAYER_INFORMATION_SAVE_SLOT_ID = "SaveSlotID";
        const string SERVER_PLAYER_COURSE_ID = "PlayerCourseID";
        // Team number assigned in the inspector should be the same as stored in the DB
        // Reset for each school (Previously, assigned in inspector different from stored in DB [YAGNI])
        const string SERVER_PLAYER_TEAM_ID = "PlayerTeamId";
        const string SERVER_PLAYER_DEVICE_UNIQUE_IDENTIFIER = "PlayerDeviceUniqueIdentifier";
        const string SERVER_PLAYER_GAME_STATE_ID = "PlayerGameState";

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
                                      COURSE_NO_ALGO + " INTEGER(10) NOT NULL, " +
                                      COURSE_ACTIVE + " INTEGER(1) NOT NULL, " +
                                      "PRIMARY KEY(" + COURSE_ID + "), " +
                                      "FOREIGN KEY(" + COURSE_TEAM_ID + ") REFERENCES " + TEAM + "(" + TEAM_ID + "), " +
                                      "FOREIGN KEY(" + COURSE_LABYRINTH_ID + ") REFERENCES " + LABYRINTH + "(" + LABYRINTH_ID + ")); ";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + EVENT + " ( " +
                                      EVENT_ID + " INTEGER PRIMARY KEY ASC, " +
                                      EVENT_TYPE + " INTEGER(10) NOT NULL, " +
                                      EVENT_VALUE + " TEXT NOT NULL, " +
                                      EVENT_TIME + " DEFAULT(STRFTIME('YYYY-MM-DD HH:MM:SS.SSS', 'NOW')), " +
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

            ResetServerGameInformation();
            ResetServerPlayerInformation();
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

        public static void ResetServerGameInformation()
        {
            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "DROP TABLE IF EXISTS " + SERVER_GAME_INFORMATION + ";";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + SERVER_GAME_INFORMATION + " ( " +
                                      SERVER_GAME_INFORMATION_SAVE_SLOT_ID + " INTEGER(10) NOT NULL, " +
                                      SERVER_GAME_STATE_ID + " INTEGER(10) NOT NULL, " +
                                      SERVER_GAME_ROUND + " INTEGER(10) NOT NULL, " +
                                      "PRIMARY KEY(" + SERVER_GAME_INFORMATION_SAVE_SLOT_ID + ")); ";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ResetServerPlayerInformation()
        {
            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "DROP TABLE IF EXISTS " + SERVER_PLAYER_INFORMATION + ";";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + SERVER_PLAYER_INFORMATION + " ( " +
                                      SERVER_PLAYER_INFORMATION_SAVE_SLOT_ID + " INTEGER(10) NOT NULL, " +
                                      SERVER_PLAYER_COURSE_ID + " INTEGER(10), " +                                      
                                      SERVER_PLAYER_TEAM_ID + " INTEGER(10), " +
                                      SERVER_PLAYER_DEVICE_UNIQUE_IDENTIFIER + " varchar(255) NOT NULL UNIQUE, " +
                                      SERVER_PLAYER_GAME_STATE_ID + " INTEGER(10) NOT NULL, " +
                                      "PRIMARY KEY(" + SERVER_PLAYER_INFORMATION_SAVE_SLOT_ID + ")); ";
                    cmd.ExecuteNonQuery();
                }
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

                    cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ", " + COURSE_ACTIVE + ") VALUES (1, 2, 1, 55, 0);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ", " + COURSE_ACTIVE + ") VALUES (2, 2, 2, 3, 0);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ", " + COURSE_ACTIVE + ") VALUES (3, 1, 2, 154, 0);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_VALUE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (1, 33, '{}', DATETIME('2018-08-27',  '13:10:10'), 3);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_VALUE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (2, 35, '{}', DATETIME('2018-08-27',  '13:10:20'), 3);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_VALUE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (3, 10, '{}', DATETIME('2018-08-27',  '13:10:15'), 1);";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_ID + ", " + EVENT_TYPE + ", " + EVENT_VALUE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES (4, 10, '{}', DATETIME('2018-08-27',  '15:10:10'), 1);";
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
                            Debug.Log(TEAM_ID + ": " + reader[TEAM_ID]
                                + "\t " + TEAM_NAME + ": " + reader[TEAM_NAME]
                                + "\t " + TEAM_COLOR + ": " + reader[TEAM_COLOR]
                                + "\t " + TEAM_CREATION_DATE_TIME + ": " + reader[TEAM_CREATION_DATE_TIME]);
                        }

                        reader.Close();
                    }

                    cmd.CommandText = "SELECT * FROM " + LABYRINTH;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.Log(LABYRINTH_ID + ": " + reader[LABYRINTH_ID]
                                + "\t " + LABYRINTH_SPECS + ": " + reader[LABYRINTH_SPECS]);
                        }

                        reader.Close();
                    }
                    cmd.CommandText = "SELECT * FROM " + COURSE;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.Log(COURSE_ID + ": " + reader[COURSE_ID]
                                + "\t " + COURSE_TEAM_ID + ": " + reader[COURSE_TEAM_ID]
                                + "\t " + COURSE_LABYRINTH_ID + ": " + reader[COURSE_LABYRINTH_ID]
                                + "\t " + COURSE_NO_ALGO + ": " + reader[COURSE_NO_ALGO]);
                        }

                        reader.Close();
                    }

                    cmd.CommandText = "SELECT * FROM " + EVENT;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Debug.Log(EVENT_ID + ": " + reader[EVENT_ID]
                                + "\t " + EVENT_TYPE + ": " + reader[EVENT_TYPE]
                                + "\t " + EVENT_VALUE + ": " + reader[EVENT_VALUE]
                                + "\t " + EVENT_TIME + ": " + reader[EVENT_TIME]
                                + "\t " + EVENT_COURSE_ID + ": " + reader[EVENT_COURSE_ID]);
                        }

                        reader.Close();
                    }

                    cmd.CommandText = "SELECT * FROM " + DEVICE_PAIRING;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Debug.Log(DEVICE_PAIRING_TABLET_ID + ": " + reader[DEVICE_PAIRING_TABLET_ID]
                                + "\t " + DEVICE_PAIRING_HEADSET_ID + ": " + reader[DEVICE_PAIRING_HEADSET_ID]);
                        }

                        reader.Close();
                    }

                    cmd.CommandText = "SELECT * FROM " + SERVER_GAME_INFORMATION;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Debug.Log(SERVER_GAME_INFORMATION_SAVE_SLOT_ID + ": " + reader[SERVER_GAME_INFORMATION_SAVE_SLOT_ID]
                                + "\t " + SERVER_GAME_STATE_ID + ": " + reader[SERVER_GAME_STATE_ID]
                                + "\t " + SERVER_GAME_ROUND + ": " + reader[SERVER_GAME_ROUND]);
                        }

                        reader.Close();
                    }

                    cmd.CommandText = "SELECT * FROM " + SERVER_PLAYER_INFORMATION;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Debug.Log(SERVER_PLAYER_INFORMATION_SAVE_SLOT_ID + ": " + reader[SERVER_PLAYER_INFORMATION_SAVE_SLOT_ID]
                                + "\t " + SERVER_PLAYER_COURSE_ID + ": " + reader[SERVER_PLAYER_COURSE_ID]                                
                                + "\t " + SERVER_PLAYER_TEAM_ID + ": " + reader[SERVER_PLAYER_TEAM_ID]
                                + "\t " + SERVER_PLAYER_DEVICE_UNIQUE_IDENTIFIER + ": " + reader[SERVER_PLAYER_DEVICE_UNIQUE_IDENTIFIER]
                                + "\t " + SERVER_PLAYER_GAME_STATE_ID + ": " + reader[SERVER_PLAYER_GAME_STATE_ID]);
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

                            for (int i = 0; i < sizeX * sizeY; i++)
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
                    else if (deviceType == DeviceType.Headset)
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
                    if (deviceType == DeviceType.Tablet)
                    {
                        cmd.CommandText = "DELETE FROM " + DEVICE_PAIRING + " WHERE " + DEVICE_PAIRING_TABLET_ID + "='" + id + "'";
                    }
                    else if (deviceType == DeviceType.Headset)
                    {
                        cmd.CommandText = "DELETE FROM " + DEVICE_PAIRING + " WHERE " + DEVICE_PAIRING_HEADSET_ID + "='" + id + "'";
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void InsertPairing(string tabletId, string headsetId)
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

        public static bool TryGetCourseId(int teamId, out int courseId)
        {
            courseId = -1;

            bool res = false;

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
                    
                    cmd.CommandText = "SELECT * FROM " + COURSE + " WHERE " +
                        COURSE_TEAM_ID + "='" + teamId + "' AND " +
                        COURSE_ACTIVE + "='" + 1 + "'";

                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        // If active course
                        if (reader.Read())
                        {
                            // TODO Assert only one course active for a given team
                            // Get active course id
                            courseId = int.Parse(reader[COURSE_ID].ToString());
                            res = true;
                        }

                        reader.Close();
                    }
                }

                return res;
            }
        }


        // Insert new course if no active course exist for the given team
        public static void InsertPlayerCourse(
            int teamId,
            int labyrinthId,
            int algorithmId,
            int courseId)
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

                    cmd.CommandText = "INSERT INTO " + COURSE + " (" + COURSE_ID + ", " + COURSE_TEAM_ID + ", " + COURSE_LABYRINTH_ID + ", " + COURSE_NO_ALGO + ", " + COURSE_ACTIVE + ") VALUES ('" + courseId + "', '" + teamId + "', '" + labyrinthId + "', '" + algorithmId + "', '1');";
                    cmd.ExecuteNonQuery();                                             
                }
            }
        }

        public static void InsertPlayerTeam(
            int teamId,
            string teamName,
            string teamColor,
            string dateTime)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "SELECT Count(*) FROM " + TEAM + " WHERE " + TEAM_ID + "='" + teamId + "'";
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetInt32(0) == 0)
                            {
                                reader.Close();
                                cmd.CommandText = "INSERT INTO " + TEAM + " (" + TEAM_ID + ", " + TEAM_NAME + ", " + TEAM_COLOR + ", " + TEAM_CREATION_DATE_TIME + ") VALUES ('" + teamId + "', '" + teamName + "', '" + teamColor + "', '" + dateTime + "');";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        reader.Close();
                    }
                }
            }
        }


        public static void InsertPlayerAction(
            int teamId, 
            string teamName, 
            string teamColor, 
            int courseId, 
            int labyrinthId, 
            int algorithmId, 
            int eventType, 
            string dateTime, 
            string eventValue)
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

                    cmd.CommandText = "INSERT INTO " + EVENT + " (" + EVENT_TYPE + ", " + EVENT_VALUE + ", " + EVENT_TIME + ", " + EVENT_COURSE_ID + ") VALUES ('" + eventType + "',  '" + eventValue + "',  '" + dateTime + "', '" + courseId + "');";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertServerGameInformation(ScriptableServerGameInformation serverGameInformation)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "INSERT OR REPLACE INTO " + SERVER_GAME_INFORMATION + " (" + SERVER_GAME_INFORMATION_SAVE_SLOT_ID + ", " + SERVER_GAME_STATE_ID + ", " + SERVER_GAME_ROUND + ") VALUES ( '" + 1 + "', '" + (int)serverGameInformation.GameState + "', '" + serverGameInformation.GameRound + "');";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void SetServerGameInformation(ScriptableServerGameInformation serverGameInformation)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT * FROM " + SERVER_GAME_INFORMATION + " WHERE " + SERVER_GAME_INFORMATION_SAVE_SLOT_ID + "=" + 1;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serverGameInformation.GameState = (ServerGameState)int.Parse(reader[SERVER_GAME_STATE_ID].ToString());
                            serverGameInformation.GameRound = int.Parse(reader[SERVER_GAME_ROUND].ToString());
                        }

                        reader.Close();
                    }
                }
            }
        }

        public static void InsertServerPlayerInformation(ScriptableServerPlayerInformation serverPlayerInformation)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    for (int i = 0; i < serverPlayerInformation.GetPlayerCount(); i++)
                    {
                        PlayerInformation playerInformation = serverPlayerInformation.GetPlayerInformationWithId(i);

                        cmd.CommandText = "INSERT OR REPLACE INTO " + SERVER_PLAYER_INFORMATION
                            + " (" + SERVER_PLAYER_INFORMATION_SAVE_SLOT_ID + ", "
                            + SERVER_PLAYER_COURSE_ID + ", "
                            + SERVER_PLAYER_TEAM_ID + ", "
                            + SERVER_PLAYER_DEVICE_UNIQUE_IDENTIFIER + ", "
                            + SERVER_PLAYER_GAME_STATE_ID
                            + ") VALUES ( '"
                            + i + "', '"
                            + playerInformation.PlayerCourseId + "', '"                            
                            + playerInformation.PlayerTeamId + "', '"
                            + playerInformation.PlayerDeviceUniqueIdentifier + "', '"
                            + (int)playerInformation.PlayerGameState + "');";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SetServerPlayerInformation(ScriptableServerPlayerInformation serverPlayerInformation)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT * FROM " + SERVER_PLAYER_INFORMATION;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int courseId = int.Parse(reader[SERVER_PLAYER_COURSE_ID].ToString());                            
                            int teamId = int.Parse(reader[SERVER_PLAYER_TEAM_ID].ToString());
                            string deviceUniqueIdentifier = reader[SERVER_PLAYER_DEVICE_UNIQUE_IDENTIFIER].ToString();
                            ClientGameState gameState = (ClientGameState)int.Parse(reader[SERVER_PLAYER_GAME_STATE_ID].ToString());

                            PlayerInformation playerInformation = new PlayerInformation(courseId, teamId, deviceUniqueIdentifier, gameState);

                            serverPlayerInformation.AddPlayerInformation(playerInformation);
                        }

                        reader.Close();
                    }
                }
            }
        }

        public static int GetPlayerCourseLabyrinthId(int courseId)
        {
            CreateDatabaseIfItDoesntExist();

            int labyrinthId = -1;

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT " + COURSE_LABYRINTH_ID + " FROM " + COURSE + " WHERE " + COURSE_ID + "=" + courseId;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labyrinthId = reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }
            }

            return labyrinthId;
        }

        public static bool HasPlayerAlreadyCompletedTheRound(int courseId)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            bool hasPlayerAlreadyCompletedTheRound = false;

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT Count(*) FROM " + EVENT
                        + " WHERE " + EVENT_COURSE_ID + "=" + courseId
                        + " AND " + EVENT_TYPE + "=" + (int)GameAction.CompletedRound;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetInt32(0) == 1)
                            {
                                hasPlayerAlreadyCompletedTheRound = true;
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return hasPlayerAlreadyCompletedTheRound;
        }

        public static Queue<int> GetPlayerStepsForCourse(int courseId)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            Queue<int> playerSteps = new Queue<int>();

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT " + EVENT_TYPE + ", " + EVENT_TIME + " FROM " + EVENT
                        + " WHERE " + EVENT_COURSE_ID + "=" + courseId
                        + " ORDER BY " + EVENT_TIME;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            playerSteps.Enqueue(int.Parse(reader[EVENT_TYPE].ToString()));
                        }

                        reader.Close();
                    }
                }
            }

            return playerSteps;
        }

        public static void GetPlayerStepsForCourse(int courseId, out Queue<int> playerSteps, out Queue<string> stepValues)
        {
            CreateDatabaseIfItDoesntExist();

            string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

            playerSteps = new Queue<int>();
            stepValues = new Queue<string>();

            using (SqliteConnection conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT " + EVENT_TYPE + ", " + EVENT_VALUE + ", " + EVENT_TIME + " FROM " + EVENT
                        + " WHERE " + EVENT_COURSE_ID + "=" + courseId
                        + " ORDER BY " + EVENT_TIME;
                    cmd.ExecuteNonQuery();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            playerSteps.Enqueue(int.Parse(reader[EVENT_TYPE].ToString()));
                            stepValues.Enqueue(reader[EVENT_VALUE].ToString());
                        }

                        reader.Close();
                    }
                }
            }
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

            int id = 4;
            int sizeX = 11;
            int sizeY = 11;

            int[] data = new int[sizeX * sizeY];

            //Colum 0
            data[(0 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(0 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 2] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 4] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 6] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 8] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 9] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 10] = Constants.TILE_ROME_TOWER_WALL_ID;

            //Colum 1
            data[(1 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 1] = Constants.TILE_ROME_START_ID;
            data[(1 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 4] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 8] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(1 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

            //Colum 2
            data[(2 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(2 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(2 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(2 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(2 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 6] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(2 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(2 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(2 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(2 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

            //Colum 3
            data[(3 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 8] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(3 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

            //Colum 4
            data[(4 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;
            data[(4 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
            data[(4 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(4 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(4 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 6] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(4 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(4 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(4 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

            //Colum 5
            data[(5 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
            data[(5 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
            data[(5 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(5 * sizeY) + 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 5] = Constants.TILE_ROME_END_ID;
            data[(5 * sizeY) + 6] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(5 * sizeY) + 8] = Constants.TILE_ROME_FLOOR_ID;
            data[(5 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(5 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

            //Colum 6
            data[(6 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(6 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(6 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(6 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
            data[(6 * sizeY) + 6] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(6 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(6 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(6 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(6 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

            //Colum 7
            data[(7 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
            data[(7 * sizeY) + 2] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(7 * sizeY) + 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
            data[(7 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
            data[(7 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(7 * sizeY) + 8] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(7 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

            //Colum 8
            data[(8 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
            data[(8 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(8 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(8 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(8 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 6] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(8 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(8 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(8 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

            //Colum 9
            data[(9 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 4] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 8] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
            data[(9 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

            //Colum 10
            data[(10 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
            data[(10 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 2] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(10 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 4] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(10 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 6] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(10 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 8] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
            data[(10 * sizeY) + 9] = Constants.TILE_ROME_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 10] = Constants.TILE_ROME_TOWER_WALL_ID;

            SQLiteUtilities.InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

            //Labyrinth 1
            id = 1;
            sizeX = 10;
            sizeY = 11;

            data = new int[sizeX * sizeY];

            //Colum 0
            data[(0 * sizeY) + 0] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(0 * sizeY) + 1] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(0 * sizeY) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 4] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 5] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 6] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 8] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 9] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 10] = Constants.TILE_PTOL_TOWER_WALL_ID;

            //Colum 1
            data[(1 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 1] = Constants.TILE_PTOL_START_ID;
            data[(1 * sizeY) + 2] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 3] = Constants.TILE_PTOL_END_ID;
            data[(1 * sizeY) + 4] = Constants.TILE_PTOL_FLOOR_ID;
            data[(1 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(1 * sizeY) + 6] = Constants.TILE_PTOL_FLOOR_ID;
            data[(1 * sizeY) + 7] = Constants.TILE_PTOL_FLOOR_ID;
            data[(1 * sizeY) + 8] = Constants.TILE_PTOL_FLOOR_ID;
            data[(1 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(1 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;

            //Colum 2
            data[(2 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(2 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(2 * sizeY) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(2 * sizeY) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 4] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(2 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(2 * sizeY) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(2 * sizeY) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 8] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(2 * sizeY) + 9] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 10] = Constants.TILE_PTOL_TOWER_WALL_ID;

            //Colum 3
            data[(3 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 2] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 3] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 4] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 6] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 7] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 8] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(3 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;

            //Colum 4
            data[(4 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(4 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(4 * sizeY) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(4 * sizeY) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 4] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(4 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(4 * sizeY) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(4 * sizeY) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 8] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(4 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(4 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

            //Colum 5
            data[(5 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(5 * sizeY) + 2] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(5 * sizeY) + 3] = Constants.TILE_PTOL_FLOOR_ID;
            data[(5 * sizeY) + 4] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(5 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(5 * sizeY) + 6] = Constants.TILE_PTOL_FLOOR_ID;
            data[(5 * sizeY) + 7] = Constants.TILE_PTOL_FLOOR_ID;
            data[(5 * sizeY) + 8] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(5 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(5 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

            //Colum 6
            data[(6 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(6 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(6 * sizeY) + 2] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(6 * sizeY) + 3] = Constants.TILE_PTOL_FLOOR_ID;
            data[(6 * sizeY) + 4] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(6 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(6 * sizeY) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(6 * sizeY) + 7] = Constants.TILE_PTOL_FLOOR_ID;
            data[(6 * sizeY) + 8] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(6 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(6 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

            //Colum 7
            data[(7 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(7 * sizeY) + 2] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(7 * sizeY) + 3] = Constants.TILE_PTOL_FLOOR_ID;
            data[(7 * sizeY) + 4] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(7 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(7 * sizeY) + 6] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(7 * sizeY) + 7] = Constants.TILE_PTOL_FLOOR_ID;
            data[(7 * sizeY) + 8] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(7 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(7 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;

            //Colum 8
            data[(8 * sizeY) + 0] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 1] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 2] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 3] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 4] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 5] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 6] = Constants.TILE_PTOL_HORIZONTAL_WALL_ID;
            data[(8 * sizeY) + 7] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 8] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 9] = Constants.TILE_PTOL_FLOOR_ID;
            data[(8 * sizeY) + 10] = Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID;

            //Colum 9
            data[(9 * sizeY) + 0] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(9 * sizeY) + 1] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(9 * sizeY) + 2] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(9 * sizeY) + 3] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(9 * sizeY) + 4] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(9 * sizeY) + 5] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(9 * sizeY) + 6] = Constants.TILE_PTOL_TOWER_WALL_ID;
            data[(9 * sizeY) + 7] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(9 * sizeY) + 8] = Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
            data[(9 * sizeY) + 9] = Constants.TILE_PTOL_VERTICAL_WALL_ID;
            data[(9 * sizeY) + 10] = Constants.TILE_PTOL_TOWER_WALL_ID;

            SQLiteUtilities.InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

            //Labyrinth 2
            id = 2;
            sizeX = 11;
            sizeY = 11;

            data = new int[sizeX * sizeY];

            //Colum 0
            data[(0 * sizeY) + 0] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(0 * sizeY) + 1] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 2] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(0 * sizeY) + 3] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 4] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(0 * sizeY) + 5] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 6] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(0 * sizeY) + 7] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 8] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(0 * sizeY) + 9] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 10] = Constants.TILE_BRIT_TOWER_WALL_2_ID;

            //Colum 1
            data[(1 * sizeY) + 0] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 2] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 4] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 5] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 6] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 8] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(1 * sizeY) + 10] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;

            //Colum 2
            data[(2 * sizeY) + 0] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(2 * sizeY) + 1] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 2] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(2 * sizeY) + 3] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 4] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(2 * sizeY) + 5] = Constants.TILE_BRIT_FLOOR_ID;
            data[(2 * sizeY) + 6] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(2 * sizeY) + 7] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 8] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(2 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(2 * sizeY) + 10] = Constants.TILE_BRIT_TOWER_WALL_ID;

            //Colum 3
            data[(3 * sizeY) + 0] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 2] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 4] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 5] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 6] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 8] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(3 * sizeY) + 10] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;

            //Colum 4
            data[(4 * sizeY) + 0] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(4 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(4 * sizeY) + 2] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(4 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(4 * sizeY) + 4] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(4 * sizeY) + 5] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 6] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(4 * sizeY) + 7] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 8] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(4 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(4 * sizeY) + 10] = Constants.TILE_BRIT_TOWER_WALL_2_ID;

            //Colum 5
            data[(5 * sizeY) + 0] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(5 * sizeY) + 2] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(5 * sizeY) + 4] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 5] = Constants.TILE_BRIT_START_ID;
            data[(5 * sizeY) + 6] = Constants.TILE_BRIT_FLOOR_ID;
            data[(5 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(5 * sizeY) + 8] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 9] = Constants.TILE_BRIT_END_ID;
            data[(5 * sizeY) + 10] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;

            //Colum 6
            data[(6 * sizeY) + 0] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(6 * sizeY) + 2] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(6 * sizeY) + 4] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(6 * sizeY) + 5] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 6] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(6 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(6 * sizeY) + 8] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 9] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 10] = Constants.TILE_BRIT_TOWER_WALL_ID;

            //Colum 7
            data[(7 * sizeY) + 0] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(7 * sizeY) + 2] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(7 * sizeY) + 4] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 5] = Constants.TILE_BRIT_FLOOR_ID;
            data[(7 * sizeY) + 6] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(7 * sizeY) + 8] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(7 * sizeY) + 10] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;

            //Colum 8
            data[(8 * sizeY) + 0] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(8 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(8 * sizeY) + 2] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(8 * sizeY) + 3] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 4] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(8 * sizeY) + 5] = Constants.TILE_BRIT_FLOOR_ID;
            data[(8 * sizeY) + 6] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(8 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(8 * sizeY) + 8] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(8 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(8 * sizeY) + 10] = Constants.TILE_BRIT_TOWER_WALL_2_ID;

            //Colum 9
            data[(9 * sizeY) + 0] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 1] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 2] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 3] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 4] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 5] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 6] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 7] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 8] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 9] = Constants.TILE_BRIT_FLOOR_ID;
            data[(9 * sizeY) + 10] = Constants.TILE_BRIT_HORIZONTAL_WALL_ID;

            //Colum 10
            data[(10 * sizeY) + 0] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(10 * sizeY) + 1] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 2] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(10 * sizeY) + 3] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 4] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(10 * sizeY) + 5] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 6] = Constants.TILE_BRIT_TOWER_WALL_ID;
            data[(10 * sizeY) + 7] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 8] = Constants.TILE_BRIT_TOWER_WALL_2_ID;
            data[(10 * sizeY) + 9] = Constants.TILE_BRIT_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 10] = Constants.TILE_BRIT_TOWER_WALL_ID;

            InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

            //Labyrinth 3
            id = 3;
            sizeX = 13;
            sizeY = 13;

            data = new int[sizeX * sizeY];

            //Colum 0
            data[(0 * sizeY) + 0] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(0 * sizeY) + 1] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 2] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 3] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 4] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 5] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(0 * sizeY) + 6] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 7] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 8] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 9] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 10] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 11] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 12] = Constants.TILE_KART_TOWER_WALL_ID;

            //Colum 1
            data[(1 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 2] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 4] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 5] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 6] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 8] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 10] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 2
            data[(2 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(2 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(2 * sizeY) + 2] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(2 * sizeY) + 3] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 4] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(2 * sizeY) + 5] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(2 * sizeY) + 6] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 7] = Constants.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID;
            data[(2 * sizeY) + 8] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 9] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(2 * sizeY) + 10] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(2 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(2 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;

            //Colum 3
            data[(3 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 2] = Constants.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID;
            data[(3 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 4] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 6] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 8] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 10] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 4
            data[(4 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(4 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 2] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(4 * sizeY) + 3] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 4] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(4 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 6] = Constants.TILE_KART_TOWER_WALL_2_ID;
            data[(4 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 8] = Constants.TILE_KART_TOWER_WALL_2_ID;
            data[(4 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 10] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(4 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;

            //Colum 5
            data[(5 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 2] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 4] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 6] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 7] = Constants.TILE_KART_END_ID;
            data[(5 * sizeY) + 8] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 10] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 6
            data[(6 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(6 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 2] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(6 * sizeY) + 3] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 4] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(6 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 6] = Constants.TILE_KART_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 7] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 8] = Constants.TILE_KART_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 10] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(6 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;

            //Colum 7
            data[(7 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 2] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 4] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 6] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 8] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 10] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 8
            data[(8 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(8 * sizeY) + 2] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(8 * sizeY) + 4] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(8 * sizeY) + 6] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(8 * sizeY) + 7] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 8] = Constants.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID;
            data[(8 * sizeY) + 9] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 10] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(8 * sizeY) + 11] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 12] = Constants.TILE_KART_TOWER_WALL_ID;

            //Colum 9
            data[(9 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 2] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 4] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 6] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 8] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 10] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 11] = Constants.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 10
            data[(10 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(10 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 2] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 4] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 5] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(10 * sizeY) + 6] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 8] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 10] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 11] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 12] = Constants.TILE_KART_TOWER_WALL_ID;

            //Colum 11
            data[(11 * sizeY) + 0] = Constants.TILE_KART_HORIZONTAL_WALL_ID;
            data[(11 * sizeY) + 1] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 2] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 3] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 4] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 5] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 6] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 7] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 8] = Constants.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID;
            data[(11 * sizeY) + 9] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 10] = Constants.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 11] = Constants.TILE_KART_START_ID;
            data[(11 * sizeY) + 12] = Constants.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 12
            data[(12 * sizeY) + 0] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(12 * sizeY) + 1] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 2] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 3] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 4] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 5] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 6] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 7] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 8] = Constants.TILE_KART_TOWER_WALL_ID;
            data[(12 * sizeY) + 9] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 10] = Constants.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 11] = Constants.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 12] = Constants.TILE_KART_TOWER_WALL_ID;

            InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);
        }
    }
}

#endif
