using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Replay
{
    public class Segment : MonoBehaviour
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

        public Vector2Int LPosition;

        private float time = 0.6f;

        [SerializeField]
        private float offsetAmount = 0f;

        private bool isTurn = false;

        private bool isInversed = false;


        public void OnValidate()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();

            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider>();
        }

        public Quaternion Direction
        {

            get
            {
                return isTurn ?
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

        public Vector3 Interpolation;
        
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

        public void AdjustOffset(float amount)
        {
            offsetAmount = amount;

            Interpolation = isDrawing ? Interpolation : Destination;

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

        public IEnumerator DrawCoroutine()
        {
            float t = 0;
            transform.position = Origin;
            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Origin);
            isDrawing = true;

            for (; t < time; t += Time.deltaTime)
            {
                Interpolation = Vector3.Lerp(Origin, Destination, t / time);
                lineRenderer.SetPosition(1, Interpolation);
                yield return null;
            }

            lineRenderer.SetPosition(1, Destination);
            isDrawing = false;

            yield return null;
        }

        public void Draw()
        {
            transform.position = Origin;

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

            // Add collider to the segment (not super important)
            boxCollider.size = new Vector3(Length, 0.175f, 0.25f);
            Vector3 midPoint = (Origin + Destination) / 2;
            transform.position = midPoint;
            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.FromToRotation(transform.forward, (Destination - Origin).normalized);

            Interpolation = Destination;
        }

        public Segment CreateTurn(
            Transform transform,
            Vector2Int lposition, // keep track of origin to remove from layout
            Vector3 origin,
            Vector3 middle,
            Vector3 destination,
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
            segm.time = time;
            segm.lineRenderer.material = material;
            segm.lineRenderer.widthMultiplier = width;
            segm.isInversed = isInversed;
            segm.material = material;
            segm.materialAlpha = materialAlpha;

            return segm;
        }



        //public Segment Create(
        //    Transform transform,
        //    Vector2Int lposition, // keep track of origin to remove from layout
        //    Vector3 origin,
        //    Vector3 middle,
        //    Vector3 destination,
        //    bool isTurn,
        //    bool isInversed,
        //    Material material,
        //    Material materialAlpha,
        //    float time,
        //    float width)
        //{
        //    var segm = Instantiate(
        //        gameObject,
        //        Vector3.zero,
        //        Quaternion.identity,
        //        transform)
        //        .GetComponent<Segment>();

        //    segm.LPosition = lposition;
        //    segm.origin = origin;
        //    segm.middle = middle;
        //    segm.destination = destination;
        //    segm.time = time;
        //    segm.lineRenderer.material = material;
        //    segm.lineRenderer.widthMultiplier = width;
        //    segm.lineRenderer.positionCount = 3;
        //    segm.isInversed = isInversed;
        //    segm.isTurn = isTurn;
        //    segm.material = material;
        //    segm.materialAlpha = materialAlpha;

        //    return segm;
        //}



    }
}