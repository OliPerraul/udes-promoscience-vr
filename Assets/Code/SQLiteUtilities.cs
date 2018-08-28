using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class SQLiteUtilities : MonoBehaviour {

    string fileName = "exampleDatabase.db";
    string dbPath;

    // Use this for initialization
    void Start()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/" + fileName;

        CreateDatabase();
        //FillTableWithTestData();
        ReadDatabase();
    }

    void CreateDatabase()
    {
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
                                  "EventID INTEGER(10) NOT NULL, " +
                                  "Type INTEGER(10) NOT NULL, " +
                                  "Time DATETIME NOT NULL, " +
                                  "ParcoursID INTEGER(10) NOT NULL, " +
                                  "PRIMARY KEY(EventID), " +
                                  "FOREIGN KEY(ParcoursID) REFERENCES Parcours(ParcoursID) ); ";
                cmd.ExecuteNonQuery();
            }
        }
    }

    void FillTableWithTestData()
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

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


    void ReadDatabase()
    {
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
            }
        }
    }

}
