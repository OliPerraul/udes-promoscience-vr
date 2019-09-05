using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Generator
{
    public class CellGenerator
    {
        private int x;
        private int y;
        private int lx;
        private int ly;
        private bool visited;
        private bool current;
        private bool North;
        private bool South;
        private bool West;
        private bool East;
        private List<int> possibilities;

        public CellGenerator(int x, int y)
        {
            this.x = x;
            this.y = y;
            current = false;
            //true = mur, false = ouvert
            North = true;
            South = true;
            West = true;
            East = true;
            visited = false;
            possibilities = new List<int>();
            possibilities.Add(0);
            possibilities.Add(1);
            possibilities.Add(2);
            possibilities.Add(3);
        }

        public int nextCell(System.Random rd)
        {
            return possibilities[rd.Next(0, possibilities.Count)];
        }

        public void setLast(int i, int j)
        {
            lx = i;
            ly = j;
        }

        public int getLX()
        {
            return lx;
        }

        public int getLY()
        {
            return ly;
        }

        public bool getCurrent()
        {
            return current;
        }

        public void switchCurrent()
        {
            current = !current;
        }

        public bool getEast()
        {
            return East;
        }

        public bool getSouth()
        {
            return South;
        }

        public bool getNorth()
        {
            return North;
        }

        public bool getWest()
        {
            return West;
        }

        public void openNorth()
        {
            North = false;
            northNotPossible();
        }

        public void openSouth()
        {
            South = false;
            southNotPossible();
        }

        public void openWest()
        {
            West = false;
            westNotPossible();
        }

        public void openEast()
        {
            East = false;
            eastNotPossible();
        }

        public void northNotPossible()
        {
            possibilities.Remove(0);
        }

        public void southNotPossible()
        {
            possibilities.Remove(2);
        }

        public void westNotPossible()
        {
            possibilities.Remove(1);
        }

        public void eastNotPossible()
        {
            possibilities.Remove(3);
        }

        public bool isVisited()
        {
            return visited;
        }

        public void setVisited()
        {
            visited = true;
        }

        public void setNonVisited()
        {
            visited = false;
        }

        public List<int> getPossibilities()
        {
            return possibilities;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public bool isDeadEnd()
        {
            int res = 0;
            if (North) res++;
            if (South) res++;
            if (East) res++;
            if (West) res++;
            if (res == 3) return true;
            return false;
        }

        public bool isIntersect()
        {
            int res = 0;
            if (North) res++;
            if (South) res++;
            if (East) res++;
            if (West) res++;
            if (res > 2) return true;
            return false;
        }
    }
}
