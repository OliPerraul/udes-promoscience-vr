using Cirrus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience
{
    public class Clock : BaseSingleton<Clock>
    {
        public Cirrus.Event OnTickedHandler;

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
