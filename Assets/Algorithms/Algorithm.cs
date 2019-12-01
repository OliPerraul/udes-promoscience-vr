using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

namespace UdeS.Promoscience.Algorithms
{
    [System.Serializable]
    public enum Id : int
    {
        Randomized = -1,
        GameRound = -2,

        Tutorial = 0,
        RightHand = 0,
        ShortestFlightDistance = 1,
        LongestStraight = 2,
        Standard = 3,
        None = 4
    }

    public class Utils
    {
        public static Id Random
        {
            get
            {
                return (Id)UnityEngine.Random.Range((int)Id.Tutorial, (int)Id.Standard);
            }
        }
    }

    public class Algorithm : ScriptableObject
    {

        [SerializeField]
        public LocalizeInlineString name;

        public string Name
        {
            get
            {
                return name.Value;                
            }
        }

        [SerializeField]
        public LocalizeString description;

        public string Description
        {
            get
            {
                return description.Value;
            }
        }

        public const int NumAlgorithms = 4;

        protected Algorithm resource;

        //protected Labyrinths.IData labyrinth;


        public virtual List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth) { return null; }

        public virtual bool GetNextStep(
            AlgorithmProgressState wrapper, 
            Labyrinths.IData labyrinth,
            out Tile tile) {
            tile = new Tile();
            return false; }


        public virtual void ResetProgressState(AlgorithmProgressState state, Labyrinths.IData labyrinth)
        {
            state.isTileAlreadyVisited = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];
            state.hasReachedTheEnd = false;
            state.lastRemoved = null;

            state.direction = labyrinth.StartDirection;
            state.position = labyrinth.StartPos;
            state.endPosition = labyrinth.EndPos;
            state.algorithmSteps.Add(new Tile(state.position.x, state.position.y, TileColor.Yellow));

            state.stack.Add(new Action { pos = state.position, dir = (Direction)state.direction });
            
            //state.alreadyVisitedTile[state.position.x, state.position.y] = true;
        }


        public virtual Id Id { get { return 0; } }

    }

    public class Action
    {
        public Direction dir;
        public Vector2Int pos;
    }

    [System.Serializable]
    public class AlgorithmProgressState
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

}