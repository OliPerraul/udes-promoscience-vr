using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmRespect : MonoBehaviour
{
    struct Step
    {
        public int action;
        public int posX;
        public int posY;
    }

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
    StandardRecursiveAlgorithm standardRecursiveAlgorithm;

    [SerializeField]
    GameObject algorithRespectBar;

    float ratioLostPerExtraActions = 0.02f;

    List<int> algorithmSteps = new List<int>();
    List<int> playerSteps = new List<int>();
    //List<Step> algorithmSteps;
    //List<Step> playerSteps;

    void Start()
    {
       // currentGameState.valueChangedEvent += OnGameStateChanged;
       // action.valueChangedEvent += OnAction;
    }


    void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.PLAYING_TUTORIAL || currentGameState.value == Constants.PLAYING)
        {
            algorithmRespect.value = 1.0f;
            SetAlgorithmStepsWithId(algorithmId.value);
            algorithRespectBar.SetActive(true);
        }
        else if (currentGameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {
            algorithRespectBar.SetActive(false);
        }
    }

    void OnAction()
    {
        //Modifie algorithm respect depending on algorithm id and action?
        playerSteps.Add(action.value);

        UpdateAlgorithmRespect();
    }

    void UpdateAlgorithmRespect()
    {

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
        else if (id == Constants.STANDARD_RECURSIVE_ALGORITH)
        {
            algorithmSteps = standardRecursiveAlgorithm.GetAlgorithmSteps();
        }
    }
}
