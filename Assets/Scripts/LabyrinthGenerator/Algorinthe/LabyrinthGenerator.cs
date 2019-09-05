using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Generator
{
    public class LabyrinthGenerator
    {
        private int lenght;
        private int width;

        private int cycle;

        private int solveChoice;
        private int error;

        private int startX;
        private int startY;
        private int endX;
        private int endY;

        private CellGenerator start;

        private List<CellGenerator> deadEnds;
        private List<CellGenerator> labyrinth;
        private System.Random rd;

        public LabyrinthGenerator(int lenght, int width, int solveChoice, int error, int cycle)
        {
            this.lenght = lenght;
            this.width = width;
            this.cycle = cycle;
            this.error = error;
            this.solveChoice = solveChoice;
            rd = new System.Random();
            generate();
        }

        public void generate()
        {
            deadEnds = new List<CellGenerator>();
            labyrinth = new List<CellGenerator>();
            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    labyrinth.Add(new CellGenerator(i, j));
                }
            }
            chooseFirstCell();
            chooseStartEnd();
        }

        public void chooseFirstCell()
        {
            int x = rd.Next(0, lenght - 1);
            int y = rd.Next(0, width - 1);
            nextCell(x, y);
        }

        public void nextCell(int i, int j)
        {
            if (getCell(i, j).isVisited())
            {
                cycle = 0;
            }
            getCell(i, j).setVisited();
            while (getCell(i, j).getPossibilities().Count != 0)
            {
                nextCellBis(i, j);
            }
            if (getCell(i, j).isDeadEnd()) deadEnds.Add(getCell(i, j));
        }

        private void nextCellBis(int i, int j)
        {
            int mov = getCell(i, j).nextCell(rd);
            switch (mov)
            {
                case 0:
                    if ((i > 0) && (!getCell(i - 1, j).isVisited() || cycle == 1))
                    {
                        getCell(i, j).openNorth();
                        getCell(i - 1, j).openSouth();
                        nextCell(i - 1, j);
                    }
                    else
                    {
                        getCell(i, j).northNotPossible();
                    }
                    break;
                case 1:
                    if ((j > 0) && (!getCell(i, j - 1).isVisited() || cycle == 1))
                    {
                        getCell(i, j).openWest();
                        getCell(i, j - 1).openEast();
                        nextCell(i, j - 1);
                    }
                    else
                    {
                        getCell(i, j).westNotPossible();
                    }
                    break;
                case 2:
                    if ((i < lenght - 1) && (!getCell(i + 1, j).isVisited() || cycle == 1))
                    {
                        getCell(i, j).openSouth();
                        getCell(i + 1, j).openNorth();
                        nextCell(i + 1, j);
                    }
                    else
                    {
                        getCell(i, j).southNotPossible();
                    }
                    break;
                case 3:
                    if ((j < width - 1) && (!getCell(i, j + 1).isVisited() || cycle == 1))
                    {
                        getCell(i, j).openEast();
                        getCell(i, j + 1).openWest();
                        nextCell(i, j + 1);
                    }
                    else
                    {
                        getCell(i, j).eastNotPossible();
                    }
                    break;
            }
        }

        public void resetVisit()
        {
            for (int i = 0; i < labyrinth.Count; i++)
            {
                labyrinth[i].setNonVisited();
            }
        }

        public void chooseStartEnd()
        {
            int res = error;
            do
            {
                resetVisit();
                error = res;
                start = deadEnds[rd.Next(deadEnds.Count)];
                deadEnds.Remove(start);
            } while (!solve(start) && deadEnds.Count > 0);
            if (deadEnds.Count == 0) generate();
            startX = start.getX();
            startY = start.getY();
        }

        public bool solve(CellGenerator start)
        {
            switch (solveChoice)
            {
                case 0:
                    return rightHand(start, 0);
                case 1:
                    return longerForward(start);
                case 2:
                    return inOrder(start);
                case 3:
                    return shorterDistance(start);
                case 4:
                    return randomSolve(start);
            }
            return false;
        }

        public bool randomSolve(CellGenerator cell)
        {
            cell.setVisited();
            if (cell.isDeadEnd() && error == 0 && cell != start)
            {
                endX = cell.getX();
                endY = cell.getY();
                return true;
            }
            if (cell.isDeadEnd() && error == 0 && cell == start)
            {
                return false;
            }
            if (cell.isDeadEnd() && cell != start)
            {
                error -= 1;
            }
            List<int> possible = getPossibleWay(cell);
            while (possible.Count > 0)
            {
                int direction = possible[rd.Next(possible.Count)];
                if (randomSolve(move(cell, direction))) return true;
            }
            return false;
        }

        public List<int> getPossibleWay(CellGenerator cell)
        {
            List<int> res = new List<int>();
            if (!cell.getNorth() && !getCell(cell.getX() - 1, cell.getY()).isVisited()) res.Add(0);
            if (!cell.getSouth() && !getCell(cell.getX() + 1, cell.getY()).isVisited()) res.Add(2);
            if (!cell.getEast() && !getCell(cell.getX(), cell.getY() + 1).isVisited()) res.Add(1);
            if (!cell.getWest() && !getCell(cell.getX(), cell.getY() - 1).isVisited()) res.Add(3);
            return res;
        }

        public CellGenerator move(CellGenerator cell, int direction)
        {
            switch (direction)
            {
                case 0:
                    if (!cell.getNorth()) return getCell(cell.getX() - 1, cell.getY());
                    break;
                case 1:
                    if (!cell.getEast()) return getCell(cell.getX(), cell.getY() + 1);
                    break;
                case 2:
                    if (!cell.getSouth()) return getCell(cell.getX() + 1, cell.getY());
                    break;
                case 3:
                    if (!cell.getWest()) return getCell(cell.getX(), cell.getY() - 1);
                    break;
            }
            return cell;
        }

        public bool rightHand(CellGenerator cell, int facing)
        {
            cell.setVisited();
            if (cell.isDeadEnd() && error == 0 && cell != start)
            {
                endX = cell.getX();
                endY = cell.getY();
                return true;
            }
            if (cell.isDeadEnd() && error == 0 && cell == start)
            {
                return false;
            }
            if (cell.isDeadEnd() && cell != start)
            {
                error -= 1;
            }
            if (move(cell, (facing + 1) % 4) != cell && !move(cell, (facing + 1) % 4).isVisited() && rightHand(move(cell, (facing + 1) % 4), (facing + 1) % 4)) return true;
            if (move(cell, facing) != cell && !move(cell, facing).isVisited() && rightHand(move(cell, facing), facing)) return true;
            if (move(cell, (facing + 3) % 4) != cell && !move(cell, (facing + 3) % 4).isVisited() && rightHand(move(cell, (facing + 3) % 4), (facing + 3) % 4)) return true;
            if (move(cell, (facing + 2) % 4) != cell && !move(cell, (facing + 2) % 4).isVisited() && rightHand(move(cell, (facing + 2) % 4), (facing + 2) % 4)) return true;

            return false;
        }

        public bool inOrder(CellGenerator cell)
        {
            cell.setVisited();
            if (cell.isDeadEnd() && error == 0 && cell != start)
            {
                endX = cell.getX();
                endY = cell.getY();
                return true;
            }
            if (cell.isDeadEnd() && error == 0 && cell == start)
            {
                return false;
            }
            if (cell.isDeadEnd() && cell != start)
            {
                error -= 1;
                return false;
            }
            if (cell.getX() > 0 && !getCell(cell.getX() - 1, cell.getY()).isVisited() && !cell.getNorth() && inOrder(getCell(cell.getX() - 1, cell.getY()))) return true;
            if (cell.getY() < width && !getCell(cell.getX(), cell.getY() + 1).isVisited() && !cell.getEast() && inOrder(getCell(cell.getX(), cell.getY() + 1))) return true;
            if (cell.getY() > 0 && !getCell(cell.getX(), cell.getY() - 1).isVisited() && !cell.getWest() && inOrder(getCell(cell.getX(), cell.getY() - 1))) return true;
            if (cell.getX() < lenght && !getCell(cell.getX() + 1, cell.getY()).isVisited() && !cell.getSouth() && inOrder(getCell(cell.getX() + 1, cell.getY()))) return true;

            return false;

        }

        int[] directionStraight = new int[4];
        List<CellGenerator> lastVisitedIntersection = new List<CellGenerator>();

        public bool longerForward(CellGenerator cell)
        {

            int direction = 0;
            CellGenerator position = cell;
            position.setVisited();



            while (true)
            {
                directionStraight[0] = (position.getNorth() || getCell(position.getX() - 1, position.getY()).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 0);
                directionStraight[1] = (position.getEast() || getCell(position.getX(), position.getY() + 1).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 1);
                directionStraight[2] = (position.getSouth() || getCell(position.getX() + 1, position.getY()).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 2);
                directionStraight[3] = (position.getWest() || getCell(position.getX(), position.getY() - 1).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 3);

                if (directionStraight[0] > 0 && directionStraight[0] >= directionStraight[1] && directionStraight[0] >= directionStraight[2] && directionStraight[0] >= directionStraight[3])
                {
                    direction = 0;
                    position = MoveInDirection(position, direction);
                }
                else if (directionStraight[1] > 0 && directionStraight[1] >= directionStraight[2] && directionStraight[1] >= directionStraight[3])
                {
                    direction = 1;
                    position = MoveInDirection(position, direction);
                }
                else if (directionStraight[2] > 0 && directionStraight[2] >= directionStraight[3])
                {
                    direction = 2;
                    position = MoveInDirection(position, direction);
                }
                else if (directionStraight[3] > 0)
                {
                    direction = 3;
                    position = MoveInDirection(position, direction);
                }
                else
                {
                    lastVisitedIntersection.Remove(position);
                    if (lastVisitedIntersection.Count > 0)
                    {
                        position = lastVisitedIntersection[lastVisitedIntersection.Count - 1];
                    }
                }

                if (position.isDeadEnd() && error == 0 && position != start)
                {
                    endX = position.getX();
                    endY = position.getY();
                    return true;
                }
                if (position.isDeadEnd() && error == 0 && position == start)
                {
                    return false;
                }
                if (position.isDeadEnd() && position != start)
                {
                    error -= 1;
                    if (lastVisitedIntersection.Count > 0)
                    {
                        position = lastVisitedIntersection[lastVisitedIntersection.Count - 1];
                    }
                }
            }
        }

        public CellGenerator MoveInDirection(CellGenerator position, int direction)
        {
            for (int i = 0; i < directionStraight[direction]; i++)
            {
                if (direction == 0) position = getCell(position.getX() - 1, position.getY());
                else if (direction == 1) position = getCell(position.getX(), position.getY() + 1);
                else if (direction == 2) position = getCell(position.getX() + 1, position.getY());
                else position = getCell(position.getX(), position.getY() - 1);
                lastVisitedIntersection.Add(position);
                position.setVisited();
            }
            return position;
        }

        public int GetStraightLenghtInDirection(CellGenerator position, int direction)
        {
            int res = -1;
            bool continuer = true;

            while (continuer)
            {
                switch (direction)
                {
                    case 0:
                        if (!position.getNorth() && !getCell(position.getX() - 1, position.getY()).isVisited()) position = getCell(position.getX() - 1, position.getY());
                        else continuer = false;
                        break;
                    case 1:
                        if (!position.getEast() && !getCell(position.getX(), position.getY() + 1).isVisited()) position = getCell(position.getX(), position.getY() + 1);
                        else continuer = false;
                        break;
                    case 2:
                        if (!position.getSouth() && !getCell(position.getX() + 1, position.getY()).isVisited()) position = getCell(position.getX() + 1, position.getY());
                        else continuer = false;
                        break;
                    case 3:
                        if (!position.getWest() && !getCell(position.getX(), position.getY() - 1).isVisited()) position = getCell(position.getX(), position.getY() - 1);
                        else continuer = false;
                        break;
                }
                res++;
            }
            return res;
        }

        public bool shorterDistance(CellGenerator cell)
        {
            int direction = 0;
            CellGenerator position = cell;
            position.setVisited();



            while (true)
            {
                directionStraight[0] = (position.getNorth() || getCell(position.getX() - 1, position.getY()).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 0);
                directionStraight[1] = (position.getEast() || getCell(position.getX(), position.getY() + 1).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 1);
                directionStraight[2] = (position.getSouth() || getCell(position.getX() + 1, position.getY()).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 2);
                directionStraight[3] = (position.getWest() || getCell(position.getX(), position.getY() - 1).isVisited()) ? 0 : GetStraightLenghtInDirection(position, 3);

                if (directionStraight[3] > 0 && (directionStraight[3] <= directionStraight[1] || directionStraight[1] == 0) && (directionStraight[3] <= directionStraight[2] || directionStraight[2] == 0) && (directionStraight[3] <= directionStraight[0] || directionStraight[0] == 0))
                {
                    direction = 3;
                    Debug.Log(direction + " " + directionStraight[0]);
                    position = MoveInDirection(position, direction);
                }
                else if (directionStraight[2] > 0 && (directionStraight[2] <= directionStraight[1] || directionStraight[1] == 0) && (directionStraight[2] <= directionStraight[0] || directionStraight[0] == 0))
                {
                    direction = 2;
                    Debug.Log(direction + " " + directionStraight[1]);
                    position = MoveInDirection(position, direction);
                }
                else if (directionStraight[1] > 0 && (directionStraight[1] <= directionStraight[0] || directionStraight[0] == 0))
                {
                    direction = 1;
                    Debug.Log(direction + " " + directionStraight[2]);
                    position = MoveInDirection(position, direction);
                }
                else if (directionStraight[0] > 0)
                {
                    direction = 0;
                    Debug.Log(direction + " " + directionStraight[3]);
                    position = MoveInDirection(position, direction);
                }
                else
                {
                    lastVisitedIntersection.Remove(position);
                    if (lastVisitedIntersection.Count > 0)
                    {
                        position = lastVisitedIntersection[lastVisitedIntersection.Count - 1];
                    }
                }

                if (position.isDeadEnd() && error == 0 && position != start)
                {
                    endX = position.getX();
                    endY = position.getY();
                    return true;
                }
                if (position.isDeadEnd() && error == 0 && position == start)
                {
                    return false;
                }
                if (position.isDeadEnd() && position != start)
                {
                    error -= 1;
                    if (lastVisitedIntersection.Count > 0)
                    {
                        position = lastVisitedIntersection[lastVisitedIntersection.Count - 1];
                    }
                }
            }
        }

        public bool isStart(int i, int j)
        {
            if (i == startX && j == startY) return true;
            return false;
        }

        public bool isEnd(int i, int j)
        {
            if (i == endX && j == endY) return true;
            return false;
        }

        public CellGenerator getCell(int i, int j)
        {
            return labyrinth[i * width + j];
        }

        public List<CellGenerator> getLabyrinth()
        {
            return labyrinth;
        }
    }
}