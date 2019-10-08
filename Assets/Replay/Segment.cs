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

        public void OnValidate()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();

            if (boxCollider == null)
                boxCollider = GetComponent<BoxCollider>();
        }

        public Vector2Int LOrigin;
        
        public bool Alpha
        {
            set
            {
                lineRenderer.material = value ? materialAlpha : material;
            }
        }

        public Vector2Int LDest;

        public Vector3 Current;
        
        public float Length
        {
            get
            {
                return (Destination - Origin).magnitude;
            }
        }

        private Vector3 origin;

        public Vector3 Origin
        {
            get
            {
                return origin + SideOffset + FrontOffset + Vector3.up * overlayHeight;
            }
        }

        private Vector3 destination;

        public Vector3 Destination
        {
            get
            {
                return destination + SideOffset + FrontOffset + Vector3.up * overlayHeight;
            }
        }

        [SerializeField]
        private float offsetAmount = 0f;

        [SerializeField]
        private Vector3 offsetDirection;

        private Vector3 SideOffset
        {
            get
            {
                return offsetDirection * offsetAmount;
            }
        }

        private Vector3 FrontOffset
        {
            get
            {
                return (destination - origin).normalized * offsetAmount;
            }
        }

        public void AdjustOffset(float amount)
        {
            offsetAmount = amount;

            Current = isDrawing ? Current : Destination;

            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Current);
        }

        private float time = 0.6f;

        public IEnumerator DrawCoroutine()
        {
            float t = 0;
            transform.position = Origin;
            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Origin);
            isDrawing = true;

            for (; t < time; t += Time.deltaTime)
            {
                Current = Vector3.Lerp(Origin, Destination, t / time);
                lineRenderer.SetPosition(1, Current);
                yield return null;
            }

            lineRenderer.SetPosition(1, Destination);
            isDrawing = false;

            yield return null;
        }

        public void Draw()
        {
            transform.position = Origin;
            lineRenderer.material = material;
            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Destination);

            boxCollider.size = new Vector3(Length, 0.175f, 0.25f);
            Vector3 midPoint = (Origin + Destination) / 2;

            transform.position = midPoint;

            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.FromToRotation(transform.forward, (Destination - Origin).normalized);

            Current = Destination;
        }

        public Segment Create(
            Transform transform,
            Vector2Int lorigin, // keep track of origin to remove from layout
            Vector2Int ldest, // keep track of origin to remove from layout
            Vector3 origin, 
            Vector3 destination,
            Vector3 offsetDirection,
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

            segm.LOrigin = lorigin;
            segm.LDest = ldest;
            segm.origin = origin;
            segm.destination = destination;
            segm.time = time;
            segm.lineRenderer.material = material;
            segm.lineRenderer.widthMultiplier = width;
            segm.offsetDirection = offsetDirection;
            segm.material = material;
            segm.materialAlpha = materialAlpha;
            
            return segm;
        }



    }
}