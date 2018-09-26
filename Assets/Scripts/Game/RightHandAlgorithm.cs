using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    public List<int> GetAlgorithmSteps()
    {
        List<int> algorithmSteps = new List<int>();

        bool asReachedTheEnd = false;

        int direction = 0;//Hardcoded start direction
        Vector2Int position = labyrinth.GetLabyrithStartPosition();

        while (!asReachedTheEnd)
        {

            bool[] isDirectionWalkable = new bool[4];
            isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
            isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
            isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
            isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);


                if (isDirectionWalkable[direction] && !isDirectionWalkable[(direction + 1) % 4])
                {
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    algorithmSteps.Add(direction);
                }
                else if (isDirectionWalkable[(direction + 1) % 4])
                {
                    direction = (direction + 1) % 4;
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    algorithmSteps.Add(direction);
                }
                else if (isDirectionWalkable[(direction + 3) % 4])
                {
                    direction = (direction + 3) % 4;
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    algorithmSteps.Add(direction);
                }
                else if (isDirectionWalkable[(direction + 2) % 4])
                {
                    direction = (direction + 2) % 4;
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    algorithmSteps.Add(direction);
                }
            else//case when direction isn't determined at start
            {
                //Si tu est sur un 4 fourche au départ et que la direction n'est pas déterminé par défaut l'algo va planté
            }
        }

        return algorithmSteps;
    }

    public List<Vector2Int> GetAlgorithmStepsPosition()
    {
        int iterCount = 0;
        List<Vector2Int> algorithmStepsPosition = new List<Vector2Int>();

        bool asReachedTheEnd = false;

        int direction = 0;//Hardcoded start direction,  doit être bien choisis si non dépendant des labyrinth on peut tourner en rond! Importe peu car on commence dans un cul de sac
        Vector2Int position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();
        algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

        while (!asReachedTheEnd)
        {
            iterCount++;//temp


            bool[] isDirectionWalkable = new bool[4];
            isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
            isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
            isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
            isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

                if (isDirectionWalkable[direction % 4] && !isDirectionWalkable[(direction + 1) % 4])
                {
                    direction = direction % 4;
                }
                else if (isDirectionWalkable[(direction + 1) % 4])
                {
                    direction = (direction + 1) % 4;
                }
                else if (isDirectionWalkable[(direction + 3) % 4])
                {
                    direction = (direction + 3) % 4;
                }
                else if (isDirectionWalkable[(direction + 2) % 4])
                {
                    direction = (direction + 2) % 4;
                }
             else//case when direction isn't determined at start
             {
                Debug.Log("Aucune solution possible!");
                //Si tu est sur un 4 fourche au départ et que la direction n'est pas déterminé par défaut l'algo va planté
             }

            position.x += xByDirection[direction];
            position.y += yByDirection[direction];
            algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

            if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 50)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmStepsPosition;
    }

}
