using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
////using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Algorithms
{
    public class RightHandAlgorithm : Algorithm
    {
        public override Id Id => Id.RightHand;

        // Up, right down, left

        public override Direction[] GetPrioritizedDirections(
            AlgorithmExecutionState state,
            Labyrinths.ILabyrinth labyrinth)
        {
            Direction[] prioritizedDirections = new Direction[4];

            if (state.direction == (int)Direction.Up)
            {
                return new Direction[]{
                    Direction.Right,
                    Direction.Up,
                    Direction.Left,
                    Direction.Down};
            }
            else if (state.direction == (int)Direction.Down)
            {
                return new Direction[]{
                    Direction.Left,
                    Direction.Down,
                    Direction.Right,
                    Direction.Up};
            }
            else if (state.direction == (int)Direction.Left)
            {
                return new Direction[]{
                    Direction.Up,
                    Direction.Left,
                    Direction.Down,
                    Direction.Right};
            }
            else if (state.direction == (int)Direction.Right)
            {
                return new Direction[]{
                    Direction.Down,
                    Direction.Right,
                    Direction.Up,
                    Direction.Left};
            }

            return prioritizedDirections;
        }
    }
            
}