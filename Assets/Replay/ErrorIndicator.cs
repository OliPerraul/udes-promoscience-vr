using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay
{
    // TODO: inherit sequence elemet
    public class ErrorIndicator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void OnValidate()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public Color Color
        {
            set
            {
                spriteRenderer.color = value;

            }
        }

        public ErrorIndicator Create(Transform parent, Color color)
        {
            var er = gameObject.Create<ErrorIndicator>(parent);
            //er.Color = color;
            return er;
        }

        public ErrorIndicator Create(Vector3 position, Transform parent, Color color)
        {
            var er = gameObject.Create<ErrorIndicator>(position, parent);
            //er.Color = color;
            return er;
        }
    }
}