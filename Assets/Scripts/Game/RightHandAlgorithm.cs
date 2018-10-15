using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    public List<Vector3Int> GetAlgorithmSteps()
    {
        //Steps the two first value are the map position and the third value is the tile color value
        List<Vector3Int> algorithmSteps = new List<Vector3Int>();

        bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        bool asReachedTheEnd = false;

        int direction = labyrinth.GetStartDirection();
        Vector2Int position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();
        algorithmSteps.Add(new Vector3Int(position.x, position.y, Constants.RED_COLOR_ID));
        alreadyVisitedTile[position.x, position.y] = true;

        while (!asReachedTheEnd)
        {
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
            else//case when direction isn't determined at start, or isn't a good one
            {
                Debug.Log("No possible solution!");//Temp
            }

            position.x += xByDirection[direction];
            position.y += yByDirection[direction];
            int tileColor = alreadyVisitedTile[position.x, position.y] ? Constants.GREEN_COLOR_ID : Constants.RED_COLOR_ID;

            algorithmSteps.Add(new Vector3Int(position.x, position.y, tileColor));

            alreadyVisitedTile[position.x, position.y] = true;

            if (position.x == endPosition.x && position.y == endPosition.y)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmSteps;
    }

}
