using UnityEngine;
using System.Collections;
using System;

namespace UdeS.Promoscience
{
    public class GameManager : Cirrus.BaseSingleton<GameManager>
    {
        [SerializeField]
        private GameAsset asset;

        [SerializeField]
        private RoundPreset[] predefinedLevels;

        [SerializeField]
        private Game currentGame = null;

        public Game CurrentGame => currentGame;

        // Use these events instead
        //public Cirrus.Event<Game> OnGameCreatedHandler;

        //public Cirrus.Event<Game> OnGameEndedHandler;

        public void StartQuickplay()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = new Quickplay(SQLiteUtilities.GetNextGameID());

            SQLiteUtilities.InsertGame(currentGame.Id);

            currentGame.Start();

            //OnGameCreatedHandler?.Invoke(currentGame);
        }

        public void StartNewGame()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = Server.Instance.Settings.IsLevelOrderPredefined.Value ?
                    new Game(
                        SQLiteUtilities.GetNextGameID(),
                        predefinedLevels) :
                    new Game(
                        SQLiteUtilities.GetNextGameID());

            SQLiteUtilities.InsertGame(currentGame.Id);

            currentGame.Start();
        }

        public void StopGame()
        {
            if (currentGame != null)
            {
                currentGame.Stop();
            }

            currentGame = null;
        }
    }
}
