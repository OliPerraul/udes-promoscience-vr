using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Cirrus.Extensions;

namespace Cirrus.UI
{
    public interface IAlphaContent
    {
        float Alpha { set; }
    }

    public static partial class Utils
    {
        public static void Set(this IAlphaContent alphaContent, float alpha)
        {
            alphaContent.Alpha = alpha;
        }
    }

    public class AlphaContent : MonoBehaviour, IAlphaContent
    {
        public float Alpha {
            set {

                foreach (var g in _graphics)
                {
                    if (g == null)
                        continue;

                    g.color = g.color.SetA(value);
                }
            }
        }

        [SerializeField]
        private MaskableGraphic[] _graphics;

        public void OnValidate()
        {
            if (_graphics == null || _graphics.Length == 0)
            {

                _graphics = GetComponentsInChildren<Image>();
            }
        }

    }
}