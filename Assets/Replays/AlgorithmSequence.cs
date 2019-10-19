using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System.Threading;

namespace UdeS.Promoscience.Replays
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

        private int moveIndex = 0;

        public override int LocalMoveIndex
        {
            get
            {
                return moveIndex;
            }
        }

        protected override bool HasPrevious
        {
            get
            {
                if (LocalMoveCount == 0)
                    return false;

                return moveIndex > 0;
            }
        }

        protected override bool HasNext
        {
            get
            {
                if (LocalMoveCount == 0)
                    return false;

                return moveIndex < LocalMoveCount;
            }
        }


        public AlgorithmSequence Create(
            Labyrinths.Labyrinth labyrinth,
            Algorithm algorithm,
            Vector2Int startPosition)
        {
            AlgorithmSequence sequence = Instantiate(
                gameObject,
                labyrinth.GetLabyrinthPositionInWorldPosition(startPosition), Quaternion.identity)
                .GetComponent<AlgorithmSequence>();

            sequence.labyrinth = labyrinth;
            sequence.lposition = startPosition;
            sequence.startlposition = startPosition;
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

        protected override IEnumerator DoNextCoroutine()
        {
            DoNext();
            yield return new WaitForSeconds(speed);
            yield return null;
        }

        protected override void DoNext()
        {
            // Clamp
            moveIndex = HasPrevious ?
                (HasNext ?
                    moveIndex :
                    LocalMoveCount - 1) :
                0;
            
            lposition = tiles[moveIndex].Position;

            Stack <TileColor> stack;

            if (!dictionary.TryGetValue(lposition, out stack))
            {
                stack = new Stack<TileColor>();
                stack.Push(TileColor.Grey); // add base color
                dictionary.Add(lposition, stack);
            }

            PaintTile(tiles[moveIndex]);
            stack.Push(tiles[moveIndex].color);

            moveIndex++;
        }

        protected override void DoPrevious()
        {
            moveIndex--;

            // Clamp
            moveIndex = HasPrevious ?
                (HasNext ?
                    moveIndex :
                    LocalMoveCount - 1) :
                0;

            Stack<TileColor> stack;

            lposition = HasPrevious ? tiles[moveIndex].Position : startlposition;

            if (dictionary.TryGetValue(lposition, out stack))
            {
                if (stack.Count != 0)
                {
                    stack.Pop();
                    if (stack.Count != 0)
                    {
                        PaintTile(lposition, stack.Peek());
                    }
                }
            }
        }
    }
}





