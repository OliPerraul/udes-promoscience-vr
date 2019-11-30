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

        public Game CurrentGame { get; private set; }

        public void Awake()
        {
            asset.Round.OnValueChangedHandler += OnGameRoundChanged;
        }


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

            CurrentGame = new Quickplay(levelSelectionMode);
        }

        public void StartNewGame()
        {
            // When new game starts set all previous courses to innactive
            foreach (Course c in SQLiteUtilities.GetSessionCourses())
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            CurrentGame = new Game(levelSelectionMode);

            SQLiteUtilities.InsertGame(CurrentGame.Id);
        }
    }
}
