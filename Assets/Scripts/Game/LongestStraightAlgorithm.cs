using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongestStraightAlgorithm : MonoBehaviour
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
        alreadyVisitedTile[position.x, position.y] = true;

        //Priorizing x position version
        while (!asReachedTheEnd)
        {
            int direction0Straight = alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]] ? 0 : GetStraightLenghtInDirection(position, 0);
            int direction1Straight = alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]] ? 0 : GetStraightLenghtInDirection(position, 1);
            int direction2Straight = alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]] ? 0 : GetStraightLenghtInDirection(position, 2);
            int direction3Straight = alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]] ? 0 : GetStraightLenghtInDirection(position, 3);

            if (direction0Straight > 0 && direction0Straight >= direction1Straight && direction0Straight >= direction2Straight && direction0Straight >= direction3Straight)
            {
                direction = 1;

                for(int i = 0; i < direction0Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
            }
            else if (direction1Straight > 0 && direction1Straight >= direction2Straight && direction1Straight >= direction3Straight)
            {
                direction = 3;

                for (int i = 0; i < direction1Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
            }
            else if (direction2Straight > 0 && direction2Straight >= direction3Straight)
            {
                direction = 0;

                for (int i = 0; i < direction2Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
            }
            else if (direction3Straight > 0)
            {
                direction = 2;

                for (int i = 0; i < direction3Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
            }
            else //RightHandAlgorithm
            {
                if (labyrinth.GetIsTileWalkable(position.x + xByDirection[direction % 4], position.y + yByDirection[direction % 4]) && !labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 1) % 4], position.y + yByDirection[(direction + 1) % 4]))
                {
                    direction = direction % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
                else if (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 1) % 4], position.y + yByDirection[(direction + 1) % 4]))
                {
                    direction = (direction + 1) % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
                else if (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 3) % 4], position.y + yByDirection[(direction + 3) % 4]))
                {
                    direction = (direction + 3) % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
                else if (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 2) % 4], position.y + yByDirection[(direction + 2) % 4]))
                {
                    direction = (direction + 2) % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmSteps.Add(direction);
                }
                else
                {
                    //Not possible, caged in a box
                }
            }
        }

        return algorithmSteps;
    }

    int GetStraightLenghtInDirection(Vector2Int position, int direction)
    {
        int straightLenght = 0;

        return straightLenght;
    }
}
