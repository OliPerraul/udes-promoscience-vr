using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Algorithms
{
    public interface ICourseExecution
    {

    }

    public class Action
    {
        public Direction dir;
        public Vector2Int pos;
        public TileColor color;
    }

    [System.Serializable]
    public class AlgorithmExecutionState
    {
        [SerializeField]
        public List<Action> stack = new List<Action>();

        [SerializeField]
        public Action lastRemoved = null;

        [SerializeField]
        public List<Tile> algorithmSteps = new List<Tile>();// = new List<Tile>();

        public void SetVisited(Vector2Int pos)
        {
            isTileAlreadyVisited[pos.x, pos.y] = true;
        }

        public bool IsAlreadyVisisted(Vector2Int pos)
        {
            return isTileAlreadyVisited[pos.x, pos.y];
        }

        [SerializeField]
        public bool[,] isTileAlreadyVisited;// = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        [SerializeField]
        public bool hasReachedTheEnd;// = false;

        [SerializeField]
        public int direction;// = labyrinth.StartDirection;

        [SerializeField]
        public Vector2Int position;/// = labyrinth.StartPos;

        [SerializeField]
        public Vector2Int endPosition;// = labyrinth.EndPos;
    }

    [System.Serializable]
    public class AlgorithmExecution
    {
        [SerializeField]
        private List<Tile> steps;

        [SerializeField]
        public List<Tile> Steps => steps;

        public int MoveCount => steps.Count;

        [SerializeField]
        private int moveIndex = 0;

        public int MoveIndex => moveIndex;

        public bool HasPrevious => moveIndex > 0;

        public bool HasNext => moveIndex < MoveCount - 1;

        public AlgorithmExecution(
            Algorithm algorithm, 
            Labyrinths.ILabyrinth labyrinth)
        {
            steps = algorithm.GetAlgorithmSteps(labyrinth);
        }

        public void Reset()
        {
            moveIndex = 0;
        }


        //public AlgorithmExecution(
        //    Algorithm algorithm, 
        //    Labyrinths.ILabyrinth labyrinth, 
        //    int until) : this(algorithm, labyrinth)
        //{
        //    moveIndex = until < MoveCount ? until : MoveCount - 1;
        //}

        public bool Next()
        {
            moveIndex = HasPrevious ?
                (HasNext ?
                    moveIndex :
                    MoveCount - 1) :
                0;

            moveIndex++;

            return true;
        }

        public bool Previous()
        {
            moveIndex--;

            // Clamp
            moveIndex = HasPrevious ?
                (HasNext ?
                    moveIndex :
                    MoveCount - 1) :
                0;

            return true;
        }
    }
}