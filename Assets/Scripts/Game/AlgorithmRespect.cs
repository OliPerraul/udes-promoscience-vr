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
    ScriptableInteger gameState;

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

    void Start()
    {
       gameState.valueChangedEvent += OnGameStateChanged;
       action.valueChangedEvent += OnAction;
    }

    private void Update()
    {
        if(isAlgorithmRespectActive)
        {
            Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.y);

            if(labyrinthPosition != currentLabyrinthPosition)
            {
                if (!isDiverging.value)
                {
                    int previousTileColorId = labyrinth.GetTileColorId(currentLabyrinthPosition);

                    if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y || previousTileColorId != algorithmSteps[playerSteps.Count - 1].z)
                    {
                        isDiverging.value = true;
                        errorCounter++;
                        algorithmRespect.value = 1.0f - MathFunction((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude);
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
                        isDiverging.value = false;
                        algorithmRespect.value = 1.0f;
                    }
                    else
                    {
                        algorithmRespect.value = 1.0f - MathFunction((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude);
                    }
                }

                currentLabyrinthPosition = labyrinthPosition;
            }
        }
    }


    void OnGameStateChanged()
    {
        if (gameState.value == Constants.PLAYING_TUTORIAL || gameState.value == Constants.PLAYING)
        {
            errorCounter = 0;
            isDiverging.value = false;
            algorithmRespect.value = 1.0f;

            if(gameState.value == Constants.PLAYING_TUTORIAL)
            {
                SetAlgorithmStepsWithId(Constants.TUTORIAL_ALGORITH);
            }
            else
            {
                SetAlgorithmStepsWithId(algorithmId.value);
            }
               
            playerSteps.Clear();
            currentLabyrinthPosition = labyrinth.GetLabyrithStartPosition();
            playerSteps.Add(new Vector3Int(currentLabyrinthPosition.x, currentLabyrinthPosition.y, labyrinth.GetTileColorId(currentLabyrinthPosition)));
            algorithRespectBar.SetActive(true);
            isAlgorithmRespectActive = true;
        }
        else if (gameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {
            isAlgorithmRespectActive = false;
            algorithRespectBar.SetActive(false);
        }
    }

    void OnAction()
    {
        if(action.value == Constants.ACTION_PAINT_FLOOR)
        {
            //Should keep track of painting when diverging so that they affect algorithmRespect value and so they can be set back to the good color before piking up were the player diverged
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

    float MathFunction(float x)
    {
        return (1 - Mathf.Pow(E, -x/Constants.TILE_SIZE));
    }

    public void ResetPlayerSteps()
    {
        playerSteps.Clear();
        currentLabyrinthPosition = labyrinth.GetLabyrithStartPosition();
        playerSteps.Add(new Vector3Int(currentLabyrinthPosition.x, currentLabyrinthPosition.y, 0));
    }

    public void ReturnToDivergencePoint()//Not finished
    {
        //Update real player position
        currentLabyrinthPosition = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
    }
}
