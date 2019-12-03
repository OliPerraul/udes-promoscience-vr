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
        public Game currentGame = null;

        public Game CurrentGame => currentGame;


        public void StartQuickplay()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = new Quickplay(asset);

            currentGame.Start();

            SQLiteUtilities.InsertGame(CurrentGame.Id);
        }

        public void StartNewGame()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = Server.Instance.Settings.IsLevelOrderPredefined.Value ?
                    new Game(asset, predefinedLevels) :
                    new Game(asset);

            currentGame.Start();

            SQLiteUtilities.InsertGame(CurrentGame.Id);
        }

        public void StopGame()
        {
            if (currentGame != null)
            {
                CurrentGame.Stop();
            }

            currentGame = null;
        }
    }
}
