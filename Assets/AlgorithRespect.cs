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
    }

    void ComputeRightForWinAlgorithm()
    {
        bool asReachedTheEnd = false;
        int direction = 0;
        Vector2Int position = labyrinth.GetLabyrithStartPosition();

        while(!asReachedTheEnd)
        {
            TileInformation tileInfo = labyrinth.GetLabyrinthTileInfomation(position.x, position.y);
            bool b = tileInfo.isWalkable;
            //Si tu est sur un 4 fourche wtf! bug
            //si non peut tu avancer oui/non okey peut tu avancer à ta droite oui/non .... jusqua oui - non, avance si non tourne de direction vers la droite 
        }
    }
}
