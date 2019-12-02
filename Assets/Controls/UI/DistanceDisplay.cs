using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class DistanceDisplay : MonoBehaviour
    {
        //[SerializeField]
        //private AvatarControllerAsset controllerAsset;

        //[SerializeField]
        //private UnityEngine.UI.Text wallDistanceText;

        //[SerializeField]
        //private GameObject wallDistanceDisplay;



        //[SerializeField]
        //private UnityEngine.UI.Text flightDistanceText;

        //[SerializeField]
        //private GameObject flightDistanceDisplay;


        //public void Awake()
        //{
        //    Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithmChanged;

        //    controllerAsset.WallDistance.OnValueChangedHandler += OnWallDistance;
        //    controllerAsset.FlightDistance.OnValueChangedHandler += OnFlightDistance;
        //}

        //public void OnAlgorithmChanged(Algorithms.Algorithm algorithm)
        //{
        //    switch (algorithm.Id)
        //    {
        //        case Algorithms.Id.ShortestFlightDistance:
        //            wallDistanceDisplay.SetActive(false);
        //            flightDistanceDisplay.SetActive(true);
        //            break;

        //        case Algorithms.Id.LongestStraight:
        //            wallDistanceDisplay.SetActive(true);
        //            flightDistanceDisplay.SetActive(false);
        //            break;

        //        default:
        //            wallDistanceDisplay.SetActive(false);
        //            flightDistanceDisplay.SetActive(false);
        //            break;
        //    }

        //}

        //public void OnWallDistance(float distance)
        //{
        //    wallDistanceText.text = distance < 0 ? "?" : distance.ToString() + " m";
        //}

        //public void OnFlightDistance(float distance)
        //{
        //    flightDistanceText.text = distance < 0 ? "?" : distance.ToString() + " m";
        //}

    }
}
