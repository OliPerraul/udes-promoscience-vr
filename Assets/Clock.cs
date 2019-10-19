using Cirrus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience
{
    public class Clock : MonoBehaviour
    {
        public OnEvent OnTickedHandler;

        private static Clock _instance;

        public void Awake()
        {
            _instance = this;
        }
        

        public static Clock Instance
        {
            get {
                return _instance;
            }
        }


        public void FixedUpdate()
        {
            if(OnTickedHandler != null)
                OnTickedHandler.Invoke();//
        }


        // TODO: in order to move clock to cirrus.
        //public CreateTimer(float limit, bool start = true, bool repeat = false)
        //{

        //}

    }
}
