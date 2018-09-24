using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithRespect : MonoBehaviour
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
    ScriptableInteger algorithId;

    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableFloat algorithRespect;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    GameObject algorithRespectBar;

    float ratioLostPerExtraActions = 0.02f;

    List<int> algorithmSteps = new List<int>();
    List<int> playerSteps = new List<int>();
    //List<Step> algorithmSteps;
    //List<Step> playerSteps;

    void Start()
    {
        currentGameState.valueChangedEvent += OnGameStateChanged;
        action.valueChangedEvent += OnAction;
    }


    void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.PLAYING_TUTORIAL || currentGameState.value == Constants.PLAYING)
        {
            algorithRespect.value = 1.0f;
            //Precomput algorithm for lsit comparaison
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

        ComputeAlgorithmRespect();
    }

    void ComputeAlgorithmRespect()
    {

    }

    void ComputeRightForWinAlgorithm()
    {
        bool asReachedTheEnd = false;
        int direction = -1;
        Vector2Int position = labyrinth.GetLabyrithStartPosition();

        while(!asReachedTheEnd)
        {
            TileInformation tileInfo = labyrinth.GetLabyrinthTileInfomation(position.x, position.y - 1);
            bool isDirection0Walkable = tileInfo.isWalkable;
            tileInfo = labyrinth.GetLabyrinthTileInfomation(position.x + 1, position.y);
            bool isDirection1Walkable = tileInfo.isWalkable;
            tileInfo = labyrinth.GetLabyrinthTileInfomation(position.x, position.y + 1);
            bool isDirection2Walkable = tileInfo.isWalkable;
            tileInfo = labyrinth.GetLabyrinthTileInfomation(position.x - 1, position.y);
            bool isDirection3Walkable = tileInfo.isWalkable;

            if(direction == 0)
            {
                if(isDirection0Walkable && !isDirection1Walkable)
                {
                    algorithmSteps.Add(direction);
                }
                else if (isDirection1Walkable)
                {
                    direction = 1;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection3Walkable)
                {
                    direction = 3;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection2Walkable)
                {
                    direction = 2;
                    algorithmSteps.Add(direction);
                }
            }
            else if (direction == 1)
            {
                if (isDirection1Walkable && !isDirection2Walkable)
                {
                    algorithmSteps.Add(direction);
                }
                else if (isDirection2Walkable)
                {
                    direction = 2;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection0Walkable)
                {
                    direction = 0;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection3Walkable)
                {
                    direction = 3;
                    algorithmSteps.Add(direction);
                }
            }
            else if (direction == 2)
            {
                if (isDirection2Walkable && !isDirection3Walkable)
                {
                    algorithmSteps.Add(direction);
                }
                else if (isDirection3Walkable)
                {
                    direction = 3;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection1Walkable)
                {
                    direction = 1;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection0Walkable)
                {
                    direction = 0;
                    algorithmSteps.Add(direction);
                }
            }
            else if (direction == 3)
            {
                if (isDirection3Walkable && !isDirection0Walkable)
                {
                    algorithmSteps.Add(direction);
                }
                else if (isDirection0Walkable)
                {
                    direction = 0;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection2Walkable)
                {
                    direction = 2;
                    algorithmSteps.Add(direction);
                }
                else if (isDirection1Walkable)
                {
                    direction = 1;
                    algorithmSteps.Add(direction);
                }
            }
            else
            {
                //Si tu est sur un 4 fourche au départ ! wtf! bug
            }
        }
    }

    void ComputeShortestDistanceToTheEndAlgorithm()
    {

    }


    void ComputeLonguestStraitMovementAlgorithm()
    {

    }

    void ComputeStandardRecursiveAlgorithm()
    {

    }
}
