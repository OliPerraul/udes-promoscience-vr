using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Replay
{
    public class Segment : SequenceElement
    {
        [SerializeField]
        private float overlayHeight = 50f;

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Material material;

        [SerializeField]
        private Material materialAlpha;

        [SerializeField]
        private BoxCollider boxCollider;

        public OnEvent OnMouseEvent;
        
        private bool isDrawing = false;

        private bool isTurnFinished = false;

        private bool isTurn = false;

        public Vector2Int LPosition;

        private float time = 0.6f;

        [SerializeField]
        private float offsetAmount = 0f;

        private bool isInversed = false;


        public void OnValidate()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();

            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider>();
        }

        private Direction direction;

        public Direction Direction
        {
            get
            {
                return direction;
            }
        }

        public Quaternion Rotation
        {

            get
            {
                return 
                    isDrawing ?
                        Quaternion.LookRotation(
                            lerpDest - lerpOrigin,
                            Vector3.up) :

                        isTurn ?
                            Quaternion.LookRotation(
                                destination - middle,
                                Vector3.up) :

                            Quaternion.LookRotation(
                                destination - origin,
                                Vector3.up);
            }
        }

        public bool Alpha
        {
            set
            {
                lineRenderer.material = value ? materialAlpha : material;
            }
        }

        public Vector3 Position;

        private Vector3 lerpOrigin;

        private Vector3 lerpDest;

        public float Length
        {
            get
            {
                return (Destination - Origin).magnitude;
            }
        }

        private Vector3 middle;

        // The only reason to have middle is for turns.
        public Vector3 Middle
        {
            get
            {
                // If a turn, we need to add both offsets
                // Otherwise the offset is the same: just add one
                return middle + OriginOffset + DestinationOffset + Vector3.up * overlayHeight;
            }
        }

        private Vector3 origin;

        public Vector3 Origin
        {
            get
            {
                return origin + OriginOffset + Vector3.up * overlayHeight;
            }
        }

        private Vector3 destination;

        public Vector3 Destination
        {
            get
            {
                return destination + DestinationOffset + Vector3.up * overlayHeight;
            }
        }

        private Vector3 OriginOffset
        {
            get
            {
                // If turn the offset is calculated from the pivot, otherwise the same as the destination
                var rotation = isTurn ? Quaternion.LookRotation(middle - origin) : Quaternion.LookRotation(destination - origin);
                return rotation * Vector3.right * (isInversed ? -offsetAmount : offsetAmount);
            }
        }

        private Vector3 DestinationOffset
        {
            get
            {
                var rotation = isTurn ? Quaternion.LookRotation(destination - middle) : Quaternion.LookRotation(destination - origin);
                return rotation * Vector3.right * (isInversed ? -offsetAmount : offsetAmount);
            }
        }

        public virtual void AdjustOffset(float amount)
        {
            offsetAmount = amount;

            Position = isDrawing ? Position : Destination;
            transform.position = Destination;

            if (isDrawing)
            {
                if (isTurnFinished)
                {
                    lineRenderer.SetPosition(0, Origin);
                    lineRenderer.SetPosition(1, Middle);
                }
                else
                {
                    lineRenderer.SetPosition(1, Origin);
                }
            }
            else
            { 
                // TODO interpolation
                if (isTurn)
                {
                    lineRenderer.SetPosition(0, Origin);
                    lineRenderer.SetPosition(1, Middle);
                    lineRenderer.SetPosition(2, Destination);
                }
                else
                {
                    lineRenderer.SetPosition(0, Origin);
                    lineRenderer.SetPosition(1, Destination);
                }
            }
        }

        public IEnumerator DrawCoroutine()
        {
            float t = 0;

            isDrawing = true;
            
            if (isTurn)
            {
                transform.position = Middle;

                lineRenderer.SetPosition(0, Origin);
                lineRenderer.SetPosition(1, Origin);
                lineRenderer.SetPosition(2, Origin);

                isTurnFinished = false;

                lerpOrigin = Origin;
                lerpDest = Middle;

                for (; t < time/2; t += Time.deltaTime)
                {
                    Position = Vector3.Lerp(lerpOrigin, lerpDest, t / (time/2));
                    lineRenderer.SetPosition(1, Position);
                    yield return null;
                }

                isTurnFinished = true;

                lerpOrigin = Middle;
                lerpDest = Destination;

                t = 0;

                for (; t < time/2; t += Time.deltaTime)
                {
                    Position = Vector3.Lerp(Middle, Destination, t / (time/2));
                    lineRenderer.SetPosition(2, Position);
                    yield return null;
                }                
            }
            else
            {
                transform.position = (Origin + Destination) / 2;


                lerpOrigin = Origin;
                lerpDest = Destination;

                lineRenderer.SetPosition(0, Origin);
                lineRenderer.SetPosition(1, Origin);

                for (; t < time; t += Time.deltaTime)
                {
                    Position = Vector3.Lerp(Origin, Destination, t / time);
                    lineRenderer.SetPosition(1, Position);
                    yield return null;
                }

                lineRenderer.SetPosition(1, Destination);
            }

            isDrawing = false;
            yield return null;
        }

        public void Draw()
        {
            Position = Destination;

            if (isTurn)
            {
                transform.position = Middle;
                lineRenderer.SetPosition(0, Origin);
                lineRenderer.SetPosition(1, Middle);
                lineRenderer.SetPosition(2, Destination);
            }
            else
            {
                transform.position = (Origin + Destination) / 2;
                lineRenderer.SetPosition(0, Origin);
                lineRenderer.SetPosition(1, Destination);
            }
        }

        public Segment CreateTurn(
            Transform transform,
            Vector2Int lposition, // keep track of origin to remove from layout
            Vector3 origin,
            Vector3 middle,
            Vector3 destination,
            Direction direction,
            bool isInversed,
            Material material,
            Material materialAlpha,
            float time, 
            float width)
        {
            var segm = Instantiate(                
                gameObject,
                Vector3.zero,
                Quaternion.identity,
                transform)                
                .GetComponent<Segment>();

            segm.isTurn = true;
            segm.lineRenderer.positionCount = 3;

            segm.LPosition = lposition;
            segm.origin = origin;
            segm.middle = middle;
            segm.destination = destination;
            segm.direction = direction;
            segm.time = time;
            segm.lineRenderer.material = material;
            segm.lineRenderer.widthMultiplier = width;
            segm.lineRenderer.positionCount = 3;
            segm.isInversed = isInversed;
            segm.material = material;
            segm.materialAlpha = materialAlpha;    

            return segm;
        }

        public Segment Create(
            Transform transform,
            Vector2Int lposition, // keep track of origin to remove from layout
            Vector3 origin,
            Vector3 destination,
            Direction direction,
            bool isInversed,
            Material material,
            Material materialAlpha,
            float time,
            float width)
        {
            var segm = Instantiate(
                gameObject,
                Vector3.zero,
                Quaternion.identity,
                transform)
                .GetComponent<Segment>();

            segm.isTurn = false;
            segm.lineRenderer.positionCount = 2;

            segm.LPosition = lposition;
            segm.origin = origin;
            segm.destination = destination;
            segm.direction = direction;
            segm.time = time;
            segm.lineRenderer.material = material;
            segm.lineRenderer.widthMultiplier = width;
            segm.isInversed = isInversed;
            segm.material = material;
            segm.materialAlpha = materialAlpha;

            return segm;
        }
    }
}