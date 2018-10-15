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

    int errorCounter;

    const float E = 2.71828f;

    int currentTileColorId;

    Vector2Int currentPosition;

    //Steps the two first value are the map position and the third value is the tile color value
    List<Vector3Int> algorithmSteps = new List<Vector3Int>();
    List<Vector3Int> playerSteps = new List<Vector3Int>();

    void Start()
    {
       gameState.valueChangedEvent += OnGameStateChanged;
       action.valueChangedEvent += OnAction;
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
            currentPosition = labyrinth.GetLabyrithStartPosition();
            playerSteps.Add(new Vector3Int(currentPosition.x, currentPosition.y, currentTileColorId));//Current tile color might be wrong
            algorithRespectBar.SetActive(true);
        }
        else if (gameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {
            algorithRespectBar.SetActive(false);
        }
    }

    void OnAction()
    {
        if(action.value == Constants.ACTION_MOVE_UP)
        {
            SetCurrentTileColorId();
            currentPosition += new Vector2Int(0, -1);
            UpdateAlgorithmRespect();
        }
        else if (action.value == Constants.ACTION_MOVE_RIGHT)
        {
            SetCurrentTileColorId();
            currentPosition += new Vector2Int(1, 0);
            UpdateAlgorithmRespect();
        }
        else if (action.value == Constants.ACTION_MOVE_DOWN)
        {
            SetCurrentTileColorId();
            currentPosition += new Vector2Int(0, 1);
            UpdateAlgorithmRespect();
        }
        else if (action.value == Constants.ACTION_MOVE_LEFT)
        {
            SetCurrentTileColorId();
            currentPosition += new Vector2Int(-1, 0);
            UpdateAlgorithmRespect();
        }
    }

    private void SetCurrentTileColorId()
    {
        FloorPainter floorPainter = labyrinth.GetTile(currentPosition).GetComponentInChildren<FloorPainter>();
        if (floorPainter != null)
        {
            currentTileColorId = floorPainter.GetFloorColorId();
        }
    }

    void UpdateAlgorithmRespect()
    {
        if (!isDiverging.value)
        {
            if (currentPosition.x != algorithmSteps[playerSteps.Count].x || currentPosition.y != algorithmSteps[playerSteps.Count].y || currentTileColorId != algorithmSteps[playerSteps.Count - 1].z)
            {
                isDiverging.value = true;
                errorCounter++;
                algorithmRespect.value = 1.0f - MathFunction((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - currentPosition).magnitude);
            }
            else
            {
                playerSteps[playerSteps.Count - 1] = new Vector3Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y, currentTileColorId);
                playerSteps.Add(new Vector3Int(currentPosition.x, currentPosition.y, 0));
            }
        }
        else
        {
            if (currentPosition.x == playerSteps[playerSteps.Count - 1].x && currentPosition.y == playerSteps[playerSteps.Count - 1].y)
            {
                isDiverging.value = false;
                algorithmRespect.value = 1.0f;
            }
            else
            {
                algorithmRespect.value = 1.0f - MathFunction((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - currentPosition).magnitude);
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

    float MathFunction(float x)
    {
        return (1 - Mathf.Pow(E, -x/Constants.TILE_SIZE));
    }

    public void ResetPlayerSteps()
    {
        playerSteps.Clear();
        currentPosition = labyrinth.GetLabyrithStartPosition();
        playerSteps.Add(new Vector3Int(currentPosition.x, currentPosition.y, 0));
    }

    public void ReturnToDivergencePoint()
    {
        //Update real player position
        currentPosition = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
        UpdateAlgorithmRespect();
    }
}
