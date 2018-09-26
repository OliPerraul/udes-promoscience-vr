#if UNITY_EDITOR || UNITY_STANDALONE_WIN

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

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Equipe ( " +
                                  "EquipeID INTEGER(10) NOT NULL, " +
                                  "Name INTEGER(10) NOT NULL, " +
                                  "Color INTEGER(10) NOT NULL, " +
                                  "PRIMARY KEY(EquipeID) );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Labyrinthe ( " +
                  "LabyrintheID INTEGER(10) NOT NULL, " +
                  "Specs varchar(255), " +
                  "PRIMARY KEY(LabyrintheID) );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Parcours ( " +
                  "ParcoursID INTEGER(10) NOT NULL, " +
                  "EquipeID INTEGER(10) NOT NULL, " +
                  "LabyrintheID INTEGER(10) NOT NULL, " +
                  "NoAlgo INTEGER(10) NOT NULL, " +
                  "PRIMARY KEY(ParcoursID), " +
                  "FOREIGN KEY(EquipeID) REFERENCES Equipe(EquipeID), " +
                  "FOREIGN KEY(LabyrintheID) REFERENCES Labyrinthe(LabyrintheID)); ";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Event ( " +
                                  "EventID INTEGER PRIMARY KEY ASC, " +
                                  "Type INTEGER(10) NOT NULL, " +
                                  "Time DATETIME NOT NULL, " +
                                  "ParcoursID INTEGER(10) NOT NULL, " +
                                  "FOREIGN KEY(ParcoursID) REFERENCES Parcours(ParcoursID) ); ";
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
                
                cmd.CommandText = "INSERT INTO Equipe (EquipeID, Name, Color) VALUES (1, 1, 1);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Equipe (EquipeID, Name, Color) VALUES (2, 2, 2);";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Labyrinthe (LabyrintheID, Specs) VALUES (1, 'ahah1');";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Labyrinthe (LabyrintheID, Specs) VALUES (2, 'ahah2');";
                cmd.ExecuteNonQuery();
                
                cmd.CommandText = "INSERT INTO Parcours (ParcoursID, EquipeID, LabyrintheID, NoAlgo) VALUES (1, 2, 1, 55);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Parcours (ParcoursID, EquipeID, LabyrintheID, NoAlgo) VALUES (2, 2, 2, 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Parcours (ParcoursID, EquipeID, LabyrintheID, NoAlgo) VALUES (3, 1, 2, 154);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, ParcoursID) VALUES (1, 33, DATETIME('2018-08-27',  '13:10:10'), 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, ParcoursID) VALUES (2, 35, DATETIME('2018-08-27',  '13:10:20'), 3);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, ParcoursID) VALUES (3, 10, DATETIME('2018-08-27',  '13:10:15'), 1);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Event (EventID, Type, Time, ParcoursID) VALUES (4, 10, DATETIME('2018-08-27',  '15:10:10'), 1);";
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
                cmd.CommandText = "SELECT * FROM Equipe";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("EquipeID: " + reader["EquipeID"] + "\t Name: " + reader["Name"] + "\t Color: " + reader["Color"]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Labyrinthe";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("LabyrintheID: " + reader["LabyrintheID"] + "\t Specs: " + reader["Specs"]);
                    }

                    reader.Close();
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Parcours";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Debug.Log("ParcoursID: " + reader["ParcoursID"] + "\t EquipeID: " + reader["EquipeID"] + "\t LabyrintheID: " + reader["LabyrintheID"] + "\t NoAlgo: " + reader["NoAlgo"]);
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
                        Debug.Log("EventID: " + reader["EventID"] + "\t Type: " + reader["Type"] + "\t Time: " + reader["Time"] + "\t ParcoursID: " + reader["ParcoursID"]);
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

    //Not finished!!
    public static int[,] GetLabyrintheDataWithId(int id)
    {
        CreateDatabaseIfItDoesntExist();

        string dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;
        int[,] data = new int[4,4];//temps size should be read
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Labyrinthe WHERE LabyrintheID=" + id;
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("LabyrintheID: " + reader["LabyrintheID"] + "\t Specs: " + reader["Specs"]);
                        //should file the data with the information it gets
                    }

                    reader.Close();
                }
            }
        }
        return data;
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
                        Debug.Log("TabletId : " + reader["TabletID"] + "\t HeadsetId : " + reader["HeadsetID"]);
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


    public static void AddPairing(string tabletId,string headsetId)
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

    public static void InsertPlayerAction(int teamId, int teamName, int teamColor, int parcoursId, int labyrinthId, int algorithmId, int eventType, string date, string time)
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

                cmd.CommandText = "SELECT Count(*) FROM Equipe WHERE EquipeID='" + teamId + "'";
                cmd.ExecuteNonQuery();

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            Debug.Log("No team with id : " + teamId);
                            reader.Close();
                            cmd.CommandText = "INSERT INTO Equipe (EquipeID, Name, Color) VALUES ('" + teamId + "', '" + teamName + "', '" + teamColor + "');";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }

                cmd.CommandText = "SELECT Count(*) FROM Parcours WHERE ParcoursID='" + parcoursId
                    + "' AND EquipeID='" + teamId
                    + "' AND LabyrintheID='" + labyrinthId
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
                            cmd.CommandText = "INSERT INTO Parcours (ParcoursID, EquipeID, LabyrintheID, NoAlgo) VALUES ('" + parcoursId + "', '" + teamId + "', '" + labyrinthId + "', '" + algorithmId + "');";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                }

                cmd.CommandText = "INSERT INTO Event (Type, Time, ParcoursID) VALUES ('" + eventType + "',  DATETIME('" + date + "', '" + time + "'), '" + parcoursId + "'); ";
                cmd.ExecuteNonQuery();
            }
        }
    }

}

#endif
