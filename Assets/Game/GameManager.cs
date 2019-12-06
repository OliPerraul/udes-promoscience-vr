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

        public Cirrus.Event<Game> OnGameStartedHandler;

        public Cirrus.Event<Game> OnGameEndedHandler;

        public void StartQuickplay()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = new Quickplay();

            SQLiteUtilities.InsertGame(currentGame.Id);

            currentGame.Start();

            OnGameStartedHandler?.Invoke(currentGame);
        }

        public void StartNewGame()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = Server.Instance.Settings.IsLevelOrderPredefined.Value ?
                    new Game(predefinedLevels) :
                    new Game();

            currentGame.Start();

            SQLiteUtilities.InsertGame(currentGame.Id);

            OnGameStartedHandler?.Invoke(currentGame);
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
