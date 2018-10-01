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

        return algorithmSteps;
    }

    public List<Vector2Int> GetAlgorithmStepsPosition()
    {
        int iterCount = 0;
        List<Vector2Int> algorithmStepsPosition = new List<Vector2Int>();
        bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        bool asReachedTheEnd = false;

        int direction = 1;//Hardcoded start direction,  doit être bien choisis si non dépendant des labyrinth on peut tourner en rond!
        Vector2Int position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();
        algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

        alreadyVisitedTile[position.x, position.y] = true;

        while (!asReachedTheEnd)
        {
       


            int direction0Straight = alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 0);
            int direction1Straight = alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 1);
            int direction2Straight = alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 2);
            int direction3Straight = alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 3);

            if (direction0Straight > 0 && direction0Straight >= direction1Straight && direction0Straight >= direction2Straight && direction0Straight >= direction3Straight)
            {
                direction = 0;

                for (int i = 0; i < direction0Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else if (direction1Straight > 0 && direction1Straight >= direction2Straight && direction1Straight >= direction3Straight)
            {
                direction = 1;

                for (int i = 0; i < direction1Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                    Debug.Log("position : " + position + " direction : " + direction);//temp

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else if (direction2Straight > 0 && direction2Straight >= direction3Straight)
            {
                direction = 2;

                for (int i = 0; i < direction2Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                    Debug.Log("position : " + position + " direction : " + direction);//temp

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else if (direction3Straight > 0)
            {
                direction = 3;

                for (int i = 0; i < direction3Straight; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                    Debug.Log("position : " + position + " direction : " + direction);//temp

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else
            {
                if (labyrinth.GetIsTileWalkable(position.x + xByDirection[direction % 4], position.y + yByDirection[direction % 4]) && !labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 1) % 4], position.y + yByDirection[(direction + 1) % 4]))
                {
                    direction = direction % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                }
                else if (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 1) % 4], position.y + yByDirection[(direction + 1) % 4]))
                {
                    direction = (direction + 1) % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                }
                else if (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 3) % 4], position.y + yByDirection[(direction + 3) % 4]))
                {
                    direction = (direction + 3) % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                }
                else if (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction + 2) % 4], position.y + yByDirection[(direction + 2) % 4]))
                {
                    direction = (direction + 2) % 4;
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];
                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                }
                else
                {
                    //Not possible, caged in a box
                }
            }


            iterCount++;//temp
            if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmStepsPosition;
    }

    int GetStraightLenghtInDirection(Vector2Int position, int direction)
    {
        int straightLenght = 0;

        while(labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction) % 4], position.y + yByDirection[(direction) % 4]))
        {
            straightLenght++;
            position.x += xByDirection[direction];
            position.y += yByDirection[direction];
        }

        return straightLenght;
    }
}
