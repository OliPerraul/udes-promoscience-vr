using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience
{
    public class CameraWrapper : MonoBehaviour
    {
        [SerializeField]
        private ScriptableServerGameInformation serverGameState;

        [SerializeField]
        private ScriptableClientGameState gameState;

        [SerializeField]
        private Camera topDownCamera;

        [SerializeField]
        private Camera gameplayCamera;

        [SerializeField]
        private Camera oculusCameraLeft;

        [SerializeField]
        private Camera oculusCameraRight;

        public Camera Camera;

        public enum State
        {
            Topdown,
            Gameplay
        }

        private State state;

        public void Awake()
        {
            gameState.clientStateChangedEvent += OnGameStateChanged;

            if(serverGameState != null)
                serverGameState.gameStateChangedEvent += OnServerGameStateChanged;
        }

        public void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.ViewingLocalReplay)
            {
                ChangeState(State.Topdown);
            }
        }

        public void OnServerGameStateChanged()
        {
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            {
                ChangeState(State.Topdown);
            }
        }


        public void ChangeState(State state)
        {
            switch (state)
            {
                case State.Topdown:

                    if (gameplayCamera)
                        gameplayCamera.gameObject.SetActive(false);

                    if (oculusCameraLeft)
                        oculusCameraLeft.gameObject.SetActive(false);

                    if (oculusCameraRight)
                        oculusCameraRight.gameObject.SetActive(false);

                    topDownCamera.gameObject.SetActive(true);

                    Camera = topDownCamera;

                    break;

                case State.Gameplay:

                    if(topDownCamera)
                        topDownCamera.gameObject.SetActive(false);

                    if (gameplayCamera)
                        gameplayCamera.gameObject.SetActive(true);

                    if (oculusCameraLeft)
                        oculusCameraLeft.gameObject.SetActive(true);

                    if (oculusCameraRight)
                        oculusCameraRight.gameObject.SetActive(true);

                    Camera = gameplayCamera;

                    break;
            }
        }
    }
}
