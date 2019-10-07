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

        private int moveIndex = 0;

        public override int MoveIndex
        {
            get
            {
                return moveIndex;
            }
        }

        public override int MoveCount
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

        protected override void Move(int target)
        {
            if (target == moveIndex)
                return;

            if (Mathf.Sign(target - moveIndex) < 0)
            {
                while (HasPrevious)
                {
                    if (moveIndex <= target)
                    {
                        return;
                    }

                    Reverse();
                }
            }
            else
            {
                while (HasNext)
                {
                    if (moveIndex >= target)
                    {
                        return;
                    }

                    Perform();
                }
            }

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
            Stack<TileColor> stack;

            if (!dictionary.TryGetValue(labyrinthPosition, out stack))
            {
                stack = new Stack<TileColor>();
                stack.Push(TileColor.Grey); // add base color
                dictionary.Add(labyrinthPosition, stack);
            }

            PaintTile(tiles[moveIndex]);
            stack.Push(tiles[moveIndex].color);

            moveIndex++;
            if (moveIndex < MoveCount)
            {
                labyrinthPosition = tiles[moveIndex].Position;
            }


            if (replayOptions.OnProgressHandler != null)
            {
                replayOptions.OnProgressHandler.Invoke(moveIndex);
            }
        }

        protected override void Reverse()
        {
            Stack<TileColor> stack;

            moveIndex--;
            labyrinthPosition = tiles[moveIndex].Position;

            if (dictionary.TryGetValue(labyrinthPosition, out stack))
            {
                stack.Pop();
                PaintTile(labyrinthPosition, stack.Peek());
            }

            if (replayOptions.OnProgressHandler != null)
            {
                replayOptions.OnProgressHandler.Invoke(moveIndex);
            }
        }

        public override void Play()
        {
            throw new System.NotImplementedException();
        }


        public override void Pause()
        {
            isPlaying = false;
            StopAllCoroutines();
            Move(moveIndex - 1);
        }


        public override void Stop()
        {
            isPlaying = false;
            StopAllCoroutines();
            Move(0);
        }
    }


}





