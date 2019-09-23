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
        public string[] StepValues; //jsons

        //public 
        //SQLiteUtilities.GetPlayerStepsForCourse(player.ServerCourseId, out steps, out stepValues);
    }


    public class PlayerSequence : MonoBehaviour
    {
        //[SerializeField]
        //private Path pathTemplate;

        //private List<Path> paths;

        //private Path path;

        [SerializeField]
        private float speed = 0.6f;
        
        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector2Int lastLabyrinthPosition;

        private Vector3 targetPosition;

        private PlayerSequenceData data;

        private Vector3 lastPosition;

        private TileColor lastColor;

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

        public PlayerSequence Create(PlayerSequenceData data, Labyrinth labyrinth, Vector2Int labpos, Vector3 worldPos)
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

        public void Draw(Tile[] tiles)
        {
            positions = tiles.Select
                    (x => labyrinth.GetLabyrinthPositionInWorldPosition(x.Position)).ToArray();

            Segment segment;

            for (int i = 1; i < tiles.Length; i++)
            {
                if (segments.TryGetValue(tiles[i].Position, out segment))
                {
                    segment.Enable();
                    segment.Overwrite(positions[i - 1], positions[i], tiles[i].color == TileColor.Red);
                }
            }
        }

        public IEnumerator DrawBetween(Vector2Int o, Vector2Int d, bool backtrack = false)
        {
            Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(o);
            Vector3 dest = labyrinth.GetLabyrinthPositionInWorldPosition(d);

            if (segments.TryGetValue(o, out currentSegment))
            {
                currentSegment.Overwrite(origin, dest, backtrack);                
            }
            else if(currentSegment == null)
            {
                currentSegment = segmentTemplate.Create(
                    transform,
                    origin,
                    dest,
                    segmentMaterial,
                    drawTime,
                    normalWidth,
                    backtrack);

                segments.Add(o, currentSegment);
            }

            yield return StartCoroutine(currentSegment.DrawCoroutine());
        }

        bool backtrack = false;

        public IEnumerator Perform(GameAction gameAction, string info)
        {
            yield return new WaitForEndOfFrame();

            int forwardDirection = labyrinth.GetStartDirection();

            lastLabyrinthPosition = labyrinthPosition;
            lastPosition = targetPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);
            //bool backtrack = lastColor == TileColor.Red;

            if (gameAction == GameAction.MoveUp)
            {
                labyrinthPosition.y -= 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
                yield return StartCoroutine(DrawBetween(lastLabyrinthPosition, labyrinthPosition, backtrack));
            }
            else if (gameAction == GameAction.MoveRight)
            {
                labyrinthPosition.x += 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
                yield return StartCoroutine(DrawBetween(lastLabyrinthPosition, labyrinthPosition, backtrack));
            }
            else if (gameAction == GameAction.MoveDown)
            {
                labyrinthPosition.y += 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
                yield return StartCoroutine(DrawBetween(lastLabyrinthPosition, labyrinthPosition, backtrack));
            }
            else if (gameAction == GameAction.MoveLeft)
            {
                labyrinthPosition.x -= 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
                yield return StartCoroutine(DrawBetween(lastLabyrinthPosition, labyrinthPosition, backtrack));       
            }
            else if (gameAction == GameAction.PaintFloorRed)
            {
                backtrack = true;
            }
            else if (gameAction == GameAction.PaintFloorYellow)
            {
                backtrack = false;
            }
            else if (gameAction == GameAction.ReturnToDivergencePoint)
            {
                ActionValue actionInfo = JsonUtility.FromJson<ActionValue>(info);
                labyrinthPosition = actionInfo.position;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                //transform.position = targetPosition;
                //transform.rotation = actionInfo.rotation;

                foreach (var pair in segments)
                {
                    if (pair.Value == null)
                        continue;

                    pair.Value.Enable(false);
                }

                Draw(actionInfo.playerSteps);
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