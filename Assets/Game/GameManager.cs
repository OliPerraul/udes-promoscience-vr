using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience
{
    public class GameManager : Cirrus.BaseSingleton<GameManager>
    {
        // TODO put in Option menu
        [SerializeField]
        private LevelOrder levelOrder;

        public Game CurrentGame { get; private set; }

        public void StartTutorial()
        {
            CurrentGame = new Tutorial(levelOrder);
        }

        public void StartNewGame()
        {
            CurrentGame = new Game(levelOrder);
        }
    }
}
