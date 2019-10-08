using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System.Threading;

namespace UdeS.Promoscience.Replay
{
    public class AlgorithmSequence : Sequence
    {
        private Algorithm algorithm;

        private List<Tile> tiles;

        private Dictionary<Vector2Int, Stack<TileColor>> dictionary;

        public override int LocalMoveCount
        {
            get
            {
                return tiles.Count;
            }
        }

        public AlgorithmSequence Create(
            Labyrinth labyrinth,
            Algorithm algorithm,
            Vector2Int startPosition)
        {
            AlgorithmSequence sequence = Instantiate(
                gameObject,
                labyrinth.GetLabyrinthPositionInWorldPosition(startPosition), Quaternion.identity)
                .GetComponent<AlgorithmSequence>();

            sequence.labyrinth = labyrinth;
            sequence.labyrinthPosition = startPosition;            
            sequence.algorithm = algorithm;
            sequence.tiles = algorithm.GetAlgorithmSteps();

            return sequence;
        }

        public override void Awake()
        {
            base.Awake();

            dictionary = new Dictionary<Vector2Int, Stack<TileColor>>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        private void PaintTile(Tile tile)
        {
            labyrinth.SetTileColor(tile.Position, tile.color);
        }

        private void PaintTile(Vector2Int position, TileColor action)
        {
            labyrinth.SetTileColor(position, action);
        }

        protected override IEnumerator PerformCoroutine()
        {
            Perform();
            yield return new WaitForSeconds(speed);
            yield return null;
        }

        protected override void Perform()
        {
            if (IsWithinRange)
            {
                labyrinthPosition = tiles[replayOptions.GlobalMoveIndex].Position;
            }

            Stack<TileColor> stack;

            if (!dictionary.TryGetValue(labyrinthPosition, out stack))
            {
                stack = new Stack<TileColor>();
                stack.Push(TileColor.Grey); // add base color
                dictionary.Add(labyrinthPosition, stack);
            }

            PaintTile(tiles[replayOptions.GlobalMoveIndex]);
            stack.Push(tiles[replayOptions.GlobalMoveIndex].color);
        }

        protected override void Reverse()
        {
            Stack<TileColor> stack;

            labyrinthPosition = tiles[replayOptions.GlobalMoveIndex].Position;

            if (dictionary.TryGetValue(labyrinthPosition, out stack))
            {
                stack.Pop();
                if (stack.Count != 0)
                {
                    PaintTile(labyrinthPosition, stack.Peek());
                }
            }
        }


    }


}





