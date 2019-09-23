using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerInformation", order = 1)]
    public class ScriptablePlayerInformation : ScriptableObject
    {
        bool isInitialize = false;

        int teamId;

        public int PlayerTeamId
        {
            get
            {
                return teamId;
            }
        }

        public bool IsInitialize
        {
            get
            {
                return isInitialize;
            }
        }

        public Action playerInformationChangedEvent;

        void OnPlayerInformationChanged()
        {
            if (playerInformationChangedEvent != null)
            {
                playerInformationChangedEvent();
            }
        }

        public void SetPlayerInformation(int teamId)
        {
            this.teamId = teamId;

            isInitialize = true;

            OnPlayerInformationChanged();
        }
    }
}

