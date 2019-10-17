using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay
{
    // TODO: inherit sequence elemet
    public class Error : Segment
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public override void OnValidate()
        {
            base.OnValidate();

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

        public Error Create(
            Transform transform,
            Vector3 origin,
            Vector3 middle,
            Vector3 destination,
            bool isInversed,
            bool isTurn)
        {
            var segm = this.Create(transform);
            segm.isTurn = isTurn;
            segm.origin = origin;
            segm.destination = destination;
            segm.middle = middle;
            segm.direction = direction;
            segm.isInversed = isInversed;

            //er.Color = color;
            return segm;
        }


        public override void AdjustOffset(float amount)
        {
            offsetAmount = amount;
            transform.position = Destination;
        }

        public override void Draw()
        {
            Position = Destination;

            if (isTurn)
            {
                transform.position = Middle;
            }
            else
            {
                transform.position = (Origin + Destination) / 2;
            }

        }
    }
}