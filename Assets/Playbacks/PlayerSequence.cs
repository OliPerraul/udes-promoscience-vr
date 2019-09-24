using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

using Cirrus.Extensions;

using System.Linq;
using System.Collections.Generic;

namespace UdeS.Promoscience.Playbacks
{

    public class PlayerSequenceData
    {
        public ScriptableTeam Team;

        public int[] Steps;

        public string[] StepValues;
    }

    public class PlayerSequence : MonoBehaviour
    {
        [SerializeField]
        private float speed = 0.6f;

        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector2Int lastLabyrinthPosition;

        private Vector3 targetPosition;

        private PlayerSequenceData data;

        private Vector3 lastPosition;

        private TileColor lastColor;
        private Vector3 lastOffset;
        [SerializeField]
        private float drawTime = 0.6f;

        [SerializeField]
        private float normalWidth = 1.25f;

        [SerializeField]
        private Segment currentSegment;

        [SerializeField]
        private GameObject arrowHead;

        [SerializeField]
        private Segment segmentTemplate;

        [SerializeField]
        private Transform positionsParent;

        [SerializeField]
        private List<Transform> markers;

        [SerializeField]
        private Vector3[] positions;

        [SerializeField]
        private Material templateSegmentMaterial;

        private Material segmentMaterial;

        private Dictionary<Vector2Int, Segment> segments;

        [SerializeField]
        private float offsetCoefficient = 5;

        private float offsetSize = 0;

        private Vector3 offset = Vector3.zero;

        private bool backtrack = false;


        public PlayerSequence Create(
            PlayerSequenceData data,
            int sequenceNumber,
            Labyrinth labyrinth,
            Vector2Int labpos,
            Vector3 worldPos)
        {
            PlayerSequence sequence = Instantiate(
                gameObject,
                worldPos, Quaternion.identity)
                .GetComponent<PlayerSequence>();

            sequence.labyrinth = labyrinth;
            sequence.labyrinthPosition = labpos;
            sequence.transform.position = worldPos;
            sequence.targetPosition = worldPos;
            sequence.data = data;
            sequence.segments = new Dictionary<Vector2Int, Segment>();
            sequence.segmentMaterial = new Material(templateSegmentMaterial);
            sequence.segmentMaterial.color = data.Team.TeamColor;

            // TODO: use size of the map
            // TODO use initial direction
            sequence.offsetSize = sequenceNumber * offsetCoefficient;
            sequence.offset = new Vector3(-1, 0, 0) * sequence.offsetSize;
            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.segmentMaterial.color;
            return sequence;
        }

        public void OnValidate()
        {
            if (positionsParent != null)
            {
                if (positions.Length == 0)
                {
                    markers = positionsParent.GetComponentsInChildren<Transform>().ToList();
                    markers.Remove(positionsParent.transform);
                    positions = markers.Select(x => x.position).ToArray();
                }
            }
        }

        public void FixedUpdate()
        {
            if (currentSegment != null)
            {
                arrowHead.transform.rotation =
                    Quaternion.LookRotation(
                        currentSegment.Destination - currentSegment.Origin,
                        Vector3.up);

                arrowHead.transform.position = currentSegment.Current;
            }
        }

        public IEnumerator BeginCoroutine()
        {
            for (int i = 0; i < data.Steps.Length; i++)
            {
                yield return StartCoroutine(
                    Perform(
                        (GameAction)data.Steps[i],
                        data.StepValues[i]));
            }

            yield return null;
        }

        public void Redraw(Tile[] tiles)
        {
            positions = tiles.Select
                    (x => labyrinth.GetLabyrinthPositionInWorldPosition(x.Position)).ToArray();

            Segment segment;

            for (int i = 1; i < tiles.Length; i++)
            {
                Vector3 offset = Vector3.zero;
                if (segments.TryGetValue(tiles[i].Position, out segment))
                {
                    offset = segment.Offset;
                    Destroy(segment.gameObject);
                }

                Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(tiles[i - 1].Position);
                Vector3 dest = labyrinth.GetLabyrinthPositionInWorldPosition(tiles[i].Position);

                segment = segmentTemplate.Create(
                    transform,
                    positions[i - 1],
                    positions[i],
                    offset,
                    segmentMaterial,
                    drawTime,
                    normalWidth,
                    backtrack);

                segment.Draw();

                segments[tiles[i - 1].Position] = segment;
            }
        }

        public IEnumerator DrawCoroutine(Vector2Int o, Vector3 origin, Vector3 dest, bool backtrack = false)
        {
            if (segments.TryGetValue(o, out currentSegment))
            {
                Destroy(currentSegment.gameObject);
            }

            currentSegment = segmentTemplate.Create(
                transform,
                origin,
                dest,
                offset,
                segmentMaterial,
                drawTime,
                normalWidth,
                backtrack);

            segments[o] = currentSegment;

            yield return StartCoroutine(currentSegment.DrawCoroutine());
        }

        public IEnumerator DrawTurn(Vector2Int o, Vector3 origin, Vector3 oldOffset, bool backtrack = false)
        {
            currentSegment = segmentTemplate.Create(
                transform,
                origin - offset + oldOffset,
                origin,
                offset,
                segmentMaterial,
                drawTime,
                normalWidth,
                backtrack);

            yield return StartCoroutine(currentSegment.DrawCoroutine());
        }


        public IEnumerator Perform(GameAction gameAction, string info)
        {
            yield return new WaitForEndOfFrame();

            int forwardDirection = labyrinth.GetStartDirection();
            lastLabyrinthPosition = labyrinthPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);
            lastOffset = offset;

            Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            Vector3 dest;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    labyrinthPosition.y -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.TurnLeft:

                    lastOffset = offset;
                    offset = Quaternion.AngleAxis(-90, Vector3.up) * offset;

                    yield return StartCoroutine(DrawTurn(
                        lastLabyrinthPosition,
                        origin,
                        lastOffset,
                        backtrack));

                    break;


                case GameAction.TurnRight:

                    offset = Quaternion.AngleAxis(90, Vector3.up) * offset;

                    yield return StartCoroutine(DrawTurn(
                        lastLabyrinthPosition,
                        origin,
                        lastOffset,
                        backtrack));

                    break;

                case GameAction.PaintFloorRed:
                    backtrack = true;

                    break;

                case GameAction.PaintFloorYellow:
                    backtrack = false;

                    break;

                case GameAction.ReturnToDivergencePoint:

                    ActionValue actionInfo = JsonUtility.FromJson<ActionValue>(info);
                    labyrinthPosition = actionInfo.position;
                    targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    transform.position = targetPosition;
                    transform.rotation = actionInfo.rotation;

                    foreach (var pair in segments)
                    {
                        if (pair.Value == null)
                            continue;

                        pair.Value.Enable(false);
                    }

                    Redraw(actionInfo.playerSteps);

                    break;
            }

            yield return null;


            //else if (gameAction == GameAction.TurnRight)
            //{
            //    forwardDirection = (forwardDirection + 1) % 4;
            //}
            //else if (gameAction == GameAction.TurnLeft)
            //{
            //    forwardDirection = (forwardDirection - 1) < 0 ? 3 : (forwardDirection - 1);
            //}
            //else if (gameAction == GameAction.PaintFloorYellow)
            //{
            //    lastColor = labyrinth.GetTileColor(labyrinthPosition);
            //    labyrinth.SetTileColor(labyrinthPosition, TileColor.Yellow);
            //}
            //else if (gameAction == GameAction.PaintFloorRed)
            //{
            //    lastColor = labyrinth.GetTileColor(labyrinthPosition);
            //    labyrinth.SetTileColor(labyrinthPosition, TileColor.Red);
            //}
            //else if (gameAction == GameAction.UnpaintFloor)
            //{;
            //    lastColor = labyrinth.GetTileColor(labyrinthPosition);
            //    labyrinth.SetTileColor(labyrinthPosition, TileColor.Grey);
            //}
            //else if (gameAction == GameAction.ReturnToDivergencePoint)
            //{
            //    ActionInfo actionInfo = JsonUtility.FromJson<ActionInfo>(info);

            //    targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition) ;

            //    transform.position = targetPosition;
            //    transform.rotation = actionInfo.rotation;

            //    // Undo wrong tiles (apply the correct colors)
            //    foreach (Tile tile in actionInfo.tiles)
            //    {
            //        labyrinth.SetTileColor(tile.Position, tile.color);
            //    }

            //    // Undo last action
            //    labyrinth.SetTileColor(labyrinthPosition, lastColor);

            //    // Reset labyrinth position and mark current tile
            //    labyrinthPosition = actionInfo.position;
            //    labyrinth.SetTileColor(labyrinthPosition, TileColor.Yellow);
            //}
        }


    }
}