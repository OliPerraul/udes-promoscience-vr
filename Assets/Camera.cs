using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience
{
    public class Camera : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        private UnityEngine.Camera topDownCamera;

        [SerializeField]
        private UnityEngine.Camera gameplayCamera;

        [SerializeField]
        private UnityEngine.Camera oculusCameraLeft;

        [SerializeField]
        private UnityEngine.Camera oculusCameraRight;

        public enum State
        {
            Topdown,
            Gameplay
        }

        private State state;

        public void Awake()
        {
            gameState.valueChangedEvent += OnGameStateChanged;
        }

        public void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.ViewingPlayback)
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

                    break;
            }
        }
    }
}
