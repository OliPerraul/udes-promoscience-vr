﻿using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Algorithms
{
    [System.Serializable]
    public enum Id : int
    {
        //Randomized = -1,
        ////GameRound = -2,

        RightHand = 0,
        ShortestFlightDistance = 1,
        LongestStraight = 2,
        Standard = 3,
        
    }

    public static class Utils
    {
        public static Id Random => (Id)UnityEngine.Random.Range((int)Id.RightHand, (int)Id.Standard+1);

        public static Id GetRoundAlgorithm(int round, int teamId=0)
        {
            return (Id)(round + teamId).Mod(Algorithm.NumAlgorithms);
        }
    }

    public abstract class Algorithm : ScriptableObject
    {

        [SerializeField]
        public LocalizeInlineString name;

        public string Name => name.Value;    

        [SerializeField]
        public LocalizeString description;

        public string Description => description.Value;

        public const int NumAlgorithms = 4;

        protected Algorithm resource;


        public virtual List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth)
        {
            List<Tile> algorithmSteps = new List<Tile>();

            var state = new AlgorithmProgressState();

            // Add first tile
            algorithmSteps.Add(ResetProgressState(state, labyrinth));

            Tile tile;
            while (GetNextStep(state, labyrinth, out tile))
            {
                // FIX: If in dead end, use end of the stack instead (backtracking)
                if (tile.Position == algorithmSteps[algorithmSteps.Count - 1].Position)
                {
                    algorithmSteps.RemoveAt(algorithmSteps.Count - 1);
                }

                algorithmSteps.Add(tile);
            }

            // Add last tile
            algorithmSteps.Add(tile);

            return algorithmSteps;
        }

        public abstract Direction[] GetPrioritizedDirections(AlgorithmProgressState state, Labyrinths.IData labyrinth);

        // Up, right down, left

        // Originally base on:
        // https://github.com/ferenc-nemeth/maze-generation-algorithms/blob/757c6289286387ad661813e6ecc0ec04edea30c0/solver/solver.cpp
        // void maze::solver::wall_follower
        public virtual bool GetNextStep(
            AlgorithmProgressState state,
            Labyrinths.IData labyrinth,
            out Tile tile)
        {
            tile = new Tile();

            Direction[] prioritizedDirections = GetPrioritizedDirections(state, labyrinth);

            Vector2Int dest = state.position;
            bool found = false;

            for (int i = 0; i < 4; i++)
            {
                if (prioritizedDirections[i] == Direction.Up)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Up)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Up;
                        found = true;
                        break;
                    }
                }
                else if (prioritizedDirections[i] == Direction.Down)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Down)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Down;
                        found = true;
                        break;
                    }
                }
                else if (prioritizedDirections[i] == Direction.Left)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Left)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Left;
                        found = true;
                        break;
                    }
                }
                else if (prioritizedDirections[i] == Direction.Right)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Right)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Right;
                        found = true;
                        break;
                    }
                }
            }

            if (found)
            {
                // FIX: Return to last
                if (state.lastRemoved != null)
                {
                    state.stack.Add(state.lastRemoved);
                    state.lastRemoved = null;
                }

                state.stack.Add(new Action {
                    pos = dest,
                    dir = (Direction)state.direction,
                });

                state.SetVisited(dest);
                tile = new Tile
                {
                    Position = state.position,
                    Color = TileColor.Yellow
                };

                state.position = dest;
                state.hasReachedTheEnd = state.position == labyrinth.EndPos;            
            }
            else if (state.stack.Count != 0)
            {
                int last = state.stack.Count - 1;
                tile = new Tile
                {
                    // FIX: if we have pushed a yellow tile, red when popped, otherwise yellow when popped
                    Position = state.position,
                    Color = TileColor.Red
                };

                state.position = state.stack[last].pos;
                state.direction = (int)Promoscience.Utils.GetOppositeDirection(state.stack[last].dir);
                state.lastRemoved = state.stack[last];
                state.stack.RemoveAt(last);

            }
            else return true;

            return !state.hasReachedTheEnd;
        }

        public virtual Tile ResetProgressState(AlgorithmProgressState state, Labyrinths.IData labyrinth)
        {
            state.isTileAlreadyVisited = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];
            state.hasReachedTheEnd = false;
            state.lastRemoved = null;

            state.direction = labyrinth.StartDirection;
            state.position = labyrinth.StartPos;
            state.endPosition = labyrinth.EndPos;

            state.isTileAlreadyVisited[state.position.x, state.position.y] = true;

            state.stack.Add(new Action
            {
                pos = state.position,
                dir = (Direction)state.direction
            });

            return new Tile(state.position.x, state.position.y, TileColor.Yellow);
        }


        public virtual Id Id { get { return 0; } }

    }

    public class Action
    {
        public Direction dir;
        public Vector2Int pos;
        public TileColor color;
    }

    public struct PrioritizedDirection
    {
        private float prio;

        public Direction dir;

        public const float PriorityCoefficient = 100f;

        private float Bonus
        {
            get
            {
                switch (dir)
                {
                    case Direction.Up:
                        return 3;

                    case Direction.Right:
                        return 2;

                    case Direction.Down:
                        return 1;

                    case Direction.Left:
                    default:
                        return 0;
                        
                }
            }
        }

        // Prioritize north est south ouest (standard
        public float Prio
        {
            get
            {
                return (prio * PriorityCoefficient) + Bonus;
            }

            set
            {
                prio = value;
            }
        }
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