using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    public class Log
    {
        private List<Tile> steps = new List<Tile>();

        private Logger logger;

        public void Record(Tile tile)
        {
            steps.Add(tile);
        }

        public Log(Logger logger)
        {
            this.steps = new List<Tile>();
            this.logger = logger;
        }
    }

    public class Logger : MonoBehaviour
    {
        List<Log> logs;

        public void Awake()
        {
            logs = new List<Log>();
        }

        public void Save(Log log)
        {
            logs.Add(log);
        }
    }
}