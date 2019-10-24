#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class LabyrinthDisplay : MonoBehaviour
    {
        public GameObject cube;
        public UnityEngine.Camera cam;

        private LabyrinthTranslator labyrinth;
        private List<GameObject> map = new List<GameObject>();

        private int[] data = new int[] { 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 50, 50, 50, 50, 50, 50, 50, 50, 50, 150, 150, 50, 150, 150, 150, 50, 150, 150, 150, 50, 150, 150, 0, 150, 50, 50, 50, 150, 50, 150, 50, 150, 150, 150, 150, 50, 150, 150, 150, 50, 150, 50, 150, 150, 50, 50, 50, 150, 100, 50, 50, 50, 50, 150, 150, 50, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 50, 50, 50, 50, 50, 50, 50, 50, 50, 150, 150, 50, 150, 150, 150, 150, 150, 50, 150, 50, 150, 150, 50, 50, 50, 50, 50, 150, 50, 50, 50, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150 };
        private List<int> moves;

        //Démo générati
        /*void Start()
        {
            labyrinth = new LabyrinthTranslator(10,15,0,5,0,1);
            Display();
        }*/
        //Fin démo génération

        //Démo replay
        void Start()
        {
            moves = new List<int>();
            moves.Add(0);
            moves.Add(0);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(2);
            moves.Add(2);
            moves.Add(3);
            moves.Add(3);
            moves.Add(2);
            moves.Add(2);
            moves.Add(3);
            moves.Add(3);
            moves.Add(2);
            moves.Add(2);
            moves.Add(2);
            moves.Add(2);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(0);
            moves.Add(0);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(2);
            moves.Add(2);
            moves.Add(1);
            moves.Add(1);
            moves.Add(0);
            moves.Add(0);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(0);
            moves.Add(0);
            moves.Add(1);
            moves.Add(1);
            moves.Add(0);
            moves.Add(0);
            moves.Add(1);
            moves.Add(1);
            moves.Add(0);
            moves.Add(0);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(1);
            moves.Add(2);
            moves.Add(2);
            moves.Add(2);
            moves.Add(2);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            moves.Add(3);
            labyrinth = new LabyrinthTranslator(data, 11, 11, moves);
            Display();
        }

        void Update()
        {
            if (!labyrinth.step())
            {
                UpdateDisplay();
                System.Threading.Thread.Sleep(500);
            }
        }
        //Fin démo replay

        public void Display()
        {
            for (int i = 0; i < labyrinth.getLenght(); i++)
            {
                for (int j = 0; j < labyrinth.getWidth(); j++)
                {
                    map.Add(Instantiate(cube, new Vector3(i, 1, j), Quaternion.identity));
                }
            }
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            for (int i = 0; i < labyrinth.getLenght(); i++)
            {
                for (int j = 0; j < labyrinth.getWidth(); j++)
                {
                    if (labyrinth.getCell(i, j).isBorderWall())
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.black;
                    }
                    else if (labyrinth.getCell(i, j).isWall())
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.black;
                    }
                    else if (labyrinth.getCell(i, j).getCurrent())
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.yellow;
                    }
                    else if (labyrinth.getCell(i, j).getVisited())
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.blue;
                    }
                    else if (labyrinth.getCell(i, j).isStart())
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.white;
                    }
                    else if (labyrinth.getCell(i, j).isEnd())
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        map[i * labyrinth.getWidth() + j].GetComponent<Renderer>().material.color = Color.green;
                    }
                }
            }
        }
    }
}


#endif