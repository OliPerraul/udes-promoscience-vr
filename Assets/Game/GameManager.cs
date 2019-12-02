using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    public class GameManager : Cirrus.BaseSingleton<GameManager>
    {
        [SerializeField]
        private GameManagerAsset asset;

        // TODO put in Option menu
        [SerializeField]
        private LevelSelectionMode levelSelectionMode;

        [SerializeField]
        public Game currentGame;

        public Game CurrentGame => currentGame;

        public void Awake()
        {
            asset.Round.OnValueChangedHandler += OnGameRoundChanged;
        }

        //public Algorithms.Id AlgorithmId
        //{
        //    set
        //    {
        //        if (CurrentGame != null)
        //        {
        //            //CurrentGame.AlgorithmId = value;
        //        }
        //    }
        //}


        public void OnGameRoundChanged(int round)
        {
            if (CurrentGame != null)
            {
                CurrentGame.Round.Value = round;
            }
        }

        public void OnRoundCompleted(bool compl)
        {
            if (CurrentGame != null)
            {
                CurrentGame.IsRoundCompleted.Value = compl;
            }
        }

        public void StartQuickplay()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = new Quickplay(levelSelectionMode);

            SQLiteUtilities.InsertGame(CurrentGame.Id);
        }

        public void StartNewGame()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            currentGame = new Game(levelSelectionMode);

            SQLiteUtilities.InsertGame(CurrentGame.Id);
        }
    }
}
