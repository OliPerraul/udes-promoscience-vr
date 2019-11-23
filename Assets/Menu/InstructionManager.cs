using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Menu
{
    public class InstructionManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] pages;

        private int currentPageIndex = 0;

        [SerializeField]
        private UnityEngine.UI.Button previousButton;


        [SerializeField]
        private UnityEngine.UI.Button nextButton;

        public void Awake()
        {
            previousButton.onClick.AddListener(() =>
            {
                pages[currentPageIndex].SetActive(false);
                currentPageIndex--;
                currentPageIndex = currentPageIndex.Mod(pages.Length);
                pages[currentPageIndex].SetActive(true);
            });

            nextButton.onClick.AddListener(() =>
            {
                pages[currentPageIndex].SetActive(false);
                currentPageIndex++;
                currentPageIndex = currentPageIndex.Mod(pages.Length);
                pages[currentPageIndex].SetActive(true);
            });
        }

    }
}