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

        int playerTeamInformationId;

        public int PlayerTeamInformationId
        {
            get
            {
                return playerTeamInformationId;
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

        public void SetPlayerInformation(int teamInformationId)
        {
            playerTeamInformationId = teamInformationId;

            isInitialize = true;

            OnPlayerInformationChanged();
        }
    }
}

