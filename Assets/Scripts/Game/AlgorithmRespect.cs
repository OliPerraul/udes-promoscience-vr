using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmRespect : MonoBehaviour
{

    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableFloat algorithmRespect;

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

    float ratioLostPerExtraActions = 0.02f;

    bool isDiverging;

    int errorCounter;

    const float E = 2.71828f;
    Vector2Int currentPosition;

    List<Vector2Int> algorithmSteps = new List<Vector2Int>();
    List<Vector2Int> playerSteps = new List<Vector2Int>();


    void Start()
    {
       currentGameState.valueChangedEvent += OnGameStateChanged;
       action.valueChangedEvent += OnAction;
    }


    void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.PLAYING_TUTORIAL || currentGameState.value == Constants.PLAYING)
        {
            errorCounter = 0;
            isDiverging = false;
            algorithmRespect.value = 1.0f;
            SetAlgorithmStepsWithId(algorithmId.value);
            playerSteps.Clear();
            currentPosition = labyrinth.GetLabyrithStartPosition();
            playerSteps.Add(new Vector2Int(currentPosition.x, currentPosition.y));
            algorithRespectBar.SetActive(true);
        }
        else if (currentGameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {
            algorithRespectBar.SetActive(false);
        }
    }

    void OnAction()
    {
        if(action.value == Constants.ACTION_MOVE_UP)
        {
            currentPosition += new Vector2Int(0, -1);
            UpdateAlgorithmRespect();
        }
        else if (action.value == Constants.ACTION_MOVE_RIGHT)
        {
            currentPosition += new Vector2Int(1, 0);
            UpdateAlgorithmRespect();
        }
        else if (action.value == Constants.ACTION_MOVE_DOWN)
        {
            currentPosition += new Vector2Int(0, 1);
            UpdateAlgorithmRespect();
        }
        else if (action.value == Constants.ACTION_MOVE_LEFT)
        {
            currentPosition += new Vector2Int(-1, 0);
            UpdateAlgorithmRespect();
        }
    }

    void UpdateAlgorithmRespect()// Quand reset pour le tutorial, le playerSteps n'est pas ressetté? c,est une cause mais pas la seul
    {
        if (!isDiverging)
        {
            if (currentPosition != algorithmSteps[playerSteps.Count])
            {
                isDiverging = true;
                errorCounter++;
                algorithmRespect.value = 1.0f - MathFunction((playerSteps[playerSteps.Count - 1] - currentPosition).magnitude);
            }
            else
            {
                playerSteps.Add(new Vector2Int(currentPosition.x, currentPosition.y));
            }
        }
        else
        {
            if (currentPosition == playerSteps[playerSteps.Count - 1])
            {
                isDiverging = false;
                algorithmRespect.value = 1.0f;
            }
            else
            {
                algorithmRespect.value = 1.0f - MathFunction((playerSteps[playerSteps.Count - 1] - currentPosition).magnitude);
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
        playerSteps.Add(new Vector2Int(currentPosition.x, currentPosition.y));
    }
}
