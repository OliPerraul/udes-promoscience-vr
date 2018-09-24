using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestFlighDistanceAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    public List<int> GetAlgorithmSteps()
    {
        List<int> algorithmSteps = new List<int>();
        bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()]; 

        bool asReachedTheEnd = false;

        int direction = 0;//Hardcoded start direction
        Vector2Int position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();//flight view


        //Priorizing x position version
        while (!asReachedTheEnd)
        {
            alreadyVisitedTile[position.x, position.y] = true;

            bool[] isDirectionWalkable = new bool[4];
            isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
            isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
            isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
            isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

            if (!alreadyVisitedTile[position.x + 1, position.y] && position.x < endPosition.x && isDirectionWalkable[1])
            {
                direction = 1;
                position.x += 1;
                algorithmSteps.Add(direction);
            }
            else if (!alreadyVisitedTile[position.x - 1, position.y] && position.x > endPosition.x && isDirectionWalkable[3])
            {
                direction = 3;
                position.x -= 1;
                algorithmSteps.Add(direction);
            }
            else if (!alreadyVisitedTile[position.x, position.y - 1] && position.y > endPosition.y && isDirectionWalkable[0])
            {
                direction = 0;
                position.y -= 1;
                algorithmSteps.Add(direction);
            }
            else if (!alreadyVisitedTile[position.x, position.y + 1] && position.y < endPosition.y && isDirectionWalkable[2])
            {
                direction = 2;
                position.y += 1;
                algorithmSteps.Add(direction);
            }
            else if (direction >= 0 && direction <= 3)//RightHandAlgorithm
            {
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
            }
        }

        return algorithmSteps;
    }
}
