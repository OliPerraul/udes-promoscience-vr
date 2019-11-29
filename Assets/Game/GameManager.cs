using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    public class GameManager : Cirrus.BaseSingleton<GameManager>
    {
        // TODO put in Option menu
        [SerializeField]
        private LevelSelectionMode levelSelectionMode;

        public Game CurrentGame { get; private set; }

        public void StartQuickplay()
        {
            CurrentGame = new Quickplay(levelSelectionMode);
        }

        public void StartNewGame()
        {
            CurrentGame = new Game(levelSelectionMode);
        }
    }
}
