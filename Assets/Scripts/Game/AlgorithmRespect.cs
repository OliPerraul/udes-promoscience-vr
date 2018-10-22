using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmRespect : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableFloat algorithmRespect;

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableBoolean isDiverging;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    RightHandAlgorithm rightHandAlgorithm;

    [SerializeField]
    LongestStraightAlgorithm longestStraightAlgorithm;

    [SerializeField]
    ShortestFlighDistanceAlgorithm shortestFlighDistanceAlgorithm;

    [SerializeField]
    StandardAlgorithm standardAlgorithm;

    [SerializeField]
    GameObject algorithRespectBar;

    [SerializeField]
    Transform cameraTransform;

    bool isAlgorithmRespectActive = false;

    int errorCounter;

    const float E = 2.71828f;

    Vector2Int currentLabyrinthPosition;

    //Steps the two first value are the map position and the third value is the tile color value
    List<Vector3Int> algorithmSteps = new List<Vector3Int>();
    List<Vector3Int> playerSteps = new List<Vector3Int>();
    List<Vector3Int> wrongColorTilesWhenDiverging = new List<Vector3Int>();

    void Start()
    {
       gameState.valueChangedEvent += OnGameStateChanged;
       action.valueChangedEvent += OnAction;
    }

    private void Update()
    {
        if(isAlgorithmRespectActive)
        {
            Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

            if(labyrinthPosition != currentLabyrinthPosition)
            {
                if (!isDiverging.value)
                {
                    int previousTileColorId = (int) labyrinth.GetTileColorId(currentLabyrinthPosition);

                    if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y || previousTileColorId != algorithmSteps[playerSteps.Count - 1].z)
                    {
                        isDiverging.value = true;
                        errorCounter++;
                        algorithmRespect.value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                    }
                    else
                    {
                        playerSteps[playerSteps.Count - 1] = new Vector3Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y, previousTileColorId);
                        playerSteps.Add(new Vector3Int(labyrinthPosition.x, labyrinthPosition.y, 0));
                    }
                }
                else
                {
                    if (labyrinthPosition.x == playerSteps[playerSteps.Count - 1].x && labyrinthPosition.y == playerSteps[playerSteps.Count - 1].y)
                    {
                        if(wrongColorTilesWhenDiverging.Count == 0)
                        {
                            isDiverging.value = false;
                            algorithmRespect.value = 1.0f;
                        }
                        else if(wrongColorTilesWhenDiverging.Count == 1 && wrongColorTilesWhenDiverging[0].x == labyrinthPosition.x && wrongColorTilesWhenDiverging[0].y == labyrinthPosition.y)
                        {
                            wrongColorTilesWhenDiverging.RemoveAt(0);
                            isDiverging.value = false;
                            algorithmRespect.value = 1.0f;
                        }
                        else
                        {
                            algorithmRespect.value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                        }
                    }
                    else
                    {
                        algorithmRespect.value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                    }
                }

                currentLabyrinthPosition = labyrinthPosition;
            }
        }
    }


    void OnGameStateChanged()
    {
        if (gameState.value == GameState.PlayingTutorial || gameState.value == GameState.Playing)
        {
            errorCounter = 0;
            isDiverging.value = false;
            algorithmRespect.value = 1.0f;

            if(gameState.value == GameState.PlayingTutorial)
            {
                SetAlgorithmStepsWithId(Constants.TUTORIAL_ALGORITH);
            }
            else
            {
                SetAlgorithmStepsWithId(algorithmId.value);
            }
               
            playerSteps.Clear();
            wrongColorTilesWhenDiverging.Clear();
            currentLabyrinthPosition = labyrinth.GetLabyrithStartPosition();
            playerSteps.Add(new Vector3Int(currentLabyrinthPosition.x, currentLabyrinthPosition.y, (int) labyrinth.GetTileColorId(currentLabyrinthPosition)));
            algorithRespectBar.SetActive(true);
            isAlgorithmRespectActive = true;
        }
        else if (gameState.value == GameState.WaitingForNextRound)
        {
            isAlgorithmRespectActive = false;
            algorithRespectBar.SetActive(false);
        }
    }

    void OnAction()
    {
        if(action.value == Constants.ACTION_PAINT_FLOOR)
        {
            PaintCurrentPositionTile(); //To avoid the fact that we don't know if it was painted before or will be painted right after, we keep the tile painting here instead of in TabletControls

            if (isDiverging.value)
            {
                Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);
                int tileColorId = (int)labyrinth.GetTileColorId(labyrinthPosition);
               
                for(int i = 0; i < wrongColorTilesWhenDiverging.Count; i++)
                {
                    if(labyrinthPosition.x == wrongColorTilesWhenDiverging[i].x && labyrinthPosition.y == wrongColorTilesWhenDiverging[i].y)
                    {
                        if (tileColorId == wrongColorTilesWhenDiverging[i].z)
                        {
                            wrongColorTilesWhenDiverging.RemoveAt(i);

                            if (labyrinthPosition.x == playerSteps[playerSteps.Count - 1].x && labyrinthPosition.y == playerSteps[playerSteps.Count - 1].y && wrongColorTilesWhenDiverging.Count == 0)
                            {
                                isDiverging.value = false;
                                algorithmRespect.value = 1.0f;
                            }
                            else
                            {
                                algorithmRespect.value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                            }
                        }

                        return;
                    }
                }

                tileColorId = (tileColorId - 1) < 0 ? 2 : (tileColorId - 1);
                wrongColorTilesWhenDiverging.Add(new Vector3Int(labyrinthPosition.x, labyrinthPosition.y, tileColorId));
                algorithmRespect.value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
            }
        }
    }

    void SetAlgorithmStepsWithId(int id)
    {
        if(id == Constants.RIGHT_HAND_ALGORITH)
        {
            algorithmSteps = rightHandAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.LONGEST_STRAIGHT_ALGORITH)
        {
            algorithmSteps = longestStraightAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITH)
        {
            algorithmSteps = shortestFlighDistanceAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.STANDARD_ALGORITH)
        {
            algorithmSteps = standardAlgorithm.GetAlgorithmSteps();
        }
    }

    void PaintCurrentPositionTile()
    {
        Vector2Int position = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

        GameObject tile = labyrinth.GetTile(position.x, position.y);
        FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();

        if (floorPainter != null)
        {
            floorPainter.PaintFloor();
        }
    }

    float RespectValueComputation(float x)
    {
        return Mathf.Pow(E, -x/Constants.TILE_SIZE);
    }

    public void ResetPlayerSteps()
    {
        playerSteps.Clear();
        wrongColorTilesWhenDiverging.Clear();
        currentLabyrinthPosition = labyrinth.GetLabyrithStartPosition();
        playerSteps.Add(new Vector3Int(currentLabyrinthPosition.x, currentLabyrinthPosition.y, 0));
    }

    public void ReturnToDivergencePoint()//Not finished
    {
        //Update real player position
        currentLabyrinthPosition = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
    }
}
