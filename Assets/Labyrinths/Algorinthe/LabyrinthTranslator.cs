#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class LabyrinthTranslator
    {
        private int lenght;
        private int width;
        private int style;
        private List<Cell> labyrinth = new List<Cell>();
        private LabyrinthGenerator generator;
        private List<int> moves;
        private int currentMove;
        private Cell currentCell;

        public LabyrinthTranslator(int l, int w, int s, int e, int st, int cy)
        {
            generator = new LabyrinthGenerator(l, w, s, e, cy);
            this.lenght = l * 2 + 1;
            this.width = w * 2 + 1;
            this.style = st;
            for (int i = 0; i < this.lenght; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (i == 0 || j == 0 || j == this.width - 1 || i == this.lenght - 1)
                    {
                        labyrinth.Add(new BorderWall(i, j));
                    }
                    else labyrinth.Add(new Wall(i, j));
                }
            }
            for (int i = 1; i < this.lenght - 1; i += 2)
            {
                for (int j = 1; j < this.width - 1; j += 2)
                {
                    replaceWallByRoad(i, j, true);
                    if (!generator.getCell(i / 2, j / 2).getEast()) replaceWallByRoad(i, j + 1, false);
                    if (!generator.getCell(i / 2, j / 2).getSouth()) replaceWallByRoad(i + 1, j, false);
                }
            }
        }

        public LabyrinthTranslator(int[] data, int lenght, int width, List<int> moves)
        {
            this.lenght = lenght;
            this.width = width;
            this.moves = moves;
            this.currentMove = 0;
            reverseTranslate(data);
        }

        public void replaceWallByRoad(int i, int j, bool type)
        {
            if (generator.isStart(i / 2, j / 2) && type)
            {
                labyrinth.Insert(i * width + j, new StartRoad(i, j));
            }
            else if (generator.isEnd(i / 2, j / 2) && type)
            {
                labyrinth.Insert(i * width + j, new EndRoad(i, j));
            }
            else
            {
                labyrinth.Insert(i * width + j, new Road(i, j));
            }
            labyrinth.RemoveAt(i * width + j + 1);
        }

        public void placeTower(int x, int y)
        {
            labyrinth.Insert(x * width + y, new Tower(x, y));
            labyrinth.RemoveAt(x * width + y + 1);
        }

        public void placeHorWall(int x, int y)
        {
            labyrinth.Insert(x * width + y, new HorizontalWall(x, y));
            labyrinth.RemoveAt(x * width + y + 1);
        }

        public void placeVerWall(int x, int y)
        {
            labyrinth.Insert(x * width + y, new VerticalWall(x, y));
            labyrinth.RemoveAt(x * width + y + 1);
        }

        public void translatorStep()
        {
            for (int i = 1; i < this.lenght - 1; i += 2)
            {
                for (int j = 1; j < this.width - 1; j += 2)
                {
                    if (generator.getCell(i / 2, j / 2).isVisited()) getCell(i, j).isVisited();
                    if (generator.getCell(i / 2, j / 2).getCurrent()) getCell(i, j).setCurrent();
                    else getCell(i, j).setNonCurrent();
                    if (!generator.getCell(i / 2, j / 2).getEast()) replaceWallByRoad(i, j + 1, false);
                    if (!generator.getCell(i / 2, j / 2).getSouth()) replaceWallByRoad(i + 1, j, false);
                }
            }
        }

        public int[] translate()
        {
            determineWall();
            int[] res = new int[lenght * width];
            for (int i = 0; i < labyrinth.Count; i++)
            {
                res[i] = labyrinth[i].translate(style);
            }
            return res;
        }

        public void reverseTranslate(int[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] >= Labyrinths.Utils.TILE_START_START_ID && data[i] <= Labyrinths.Utils.TILE_START_END_ID)
                {
                    labyrinth.Add(new StartRoad(i / width, i % width));
                    currentCell = labyrinth[labyrinth.Count - 1];
                    labyrinth[labyrinth.Count - 1].setCurrent();
                    labyrinth[labyrinth.Count - 1].isVisited();
                }
                else if (data[i] >= Labyrinths.Utils.TILE_FLOOR_START_ID && data[i] <= Labyrinths.Utils.TILE_FLOOR_END_ID) labyrinth.Add(new Road(i / width, i % width));
                else if (data[i] >= Labyrinths.Utils.TILE_END_START_ID && data[i] <= Labyrinths.Utils.TILE_END_END_ID) labyrinth.Add(new EndRoad(i / width, i % width));
                else labyrinth.Add(new Wall(i / width, i % width));
            }
        }

        public bool step()
        {
            if (currentMove >= moves.Count) return false;
            currentCell.setNonCurrent();
            switch (moves[currentMove])
            {
                case 0:
                    currentCell = getCell(currentCell.getX() - 1, currentCell.getY());
                    break;
                case 1:
                    currentCell = getCell(currentCell.getX(), currentCell.getY() + 1);
                    break;
                case 2:
                    currentCell = getCell(currentCell.getX() + 1, currentCell.getY());
                    break;
                default:
                    currentCell = getCell(currentCell.getX(), currentCell.getY() - 1);
                    break;
            }
            currentCell.setCurrent();
            currentCell.isVisited();
            currentMove++;
            if (currentCell.isEnd()) return true;
            return false;
        }


        public int getLenght()
        {
            return lenght;
        }

        public int getWidth()
        {
            return width;
        }

        public Cell getCell(int x, int y)
        {
            return labyrinth[x * width + y];
        }

        public void determineWall()
        {

            for (int i = 0; i < this.lenght; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (getCell(i, j).isBorderWall())
                    {
                        if ((i == 0 && j == 0) || (i == 0 && j == width - 1) || (i == lenght - 1 && j == 0) || (i == lenght - 1 && j == width - 1)) placeTower(i, j);
                        else if ((i == 0 && getCell(i + 1, j).isWall()) || (i == lenght - 1 && getCell(i - 1, j).isWall()) || (j == 0 && getCell(i, j + 1).isWall()) || (j == width - 1 && getCell(i, j - 1).isWall())) placeTower(i, j);
                        else if (i == 0 || i == lenght - 1)
                        {
                            placeVerWall(i, j);
                        }
                        else
                        {
                            placeHorWall(i, j);
                        }
                    }
                    else if (getCell(i, j).isWall())
                    {
                        int res = getMurAdj(i, j);
                        if (res != 2) placeTower(i, j);
                        else determineHorVer(i, j);

                    }
                }
            }
        }

        public bool determineHorVer(int x, int y)
        {
            if (getCell(x - 1, y).isTower() && getCell(x + 1, y).isTower())
            {
                placeHorWall(x, y);
                return false;
            }
            if (getCell(x - 1, y).isWall() && getCell(x + 1, y).isWall())
            {
                placeHorWall(x, y);

                return false;
            }
            if (getCell(x, y - 1).isTower() && getCell(x, y + 1).isTower())
            {
                placeVerWall(x, y);
                return true;
            }
            if (getCell(x, y - 1).isWall() && getCell(x, y + 1).isWall())
            {
                placeVerWall(x, y);
                return true;
            }
            placeTower(x, y);
            return false;
        }

        public int getMurAdj(int x, int y)
        {
            int res = 0;
            if (getCell(x - 1, y).isWall()) res++;
            if (getCell(x + 1, y).isWall()) res++;
            if (getCell(x, y - 1).isWall()) res++;
            if (getCell(x, y + 1).isWall()) res++;
            return res;
        }
    }
}

#endif