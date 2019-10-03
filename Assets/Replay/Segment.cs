using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Replay
{
    public class Segment : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Material material;

        public void OnValidate()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
        }

        public Vector2Int LOrigin;

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
                return origin + SideOffset - FrontOffset + Vector3.up;
            }
        }

        private Vector3 destination;

        public Vector3 Destination
        {
            get
            {
                return destination + SideOffset + FrontOffset + Vector3.up;
            }
        }

        private float offsetAmount = 0f;

        private float maxOffsetSize = 0f;

        private Vector3 offsetDirection;

        private Vector3 SideOffset
        {
            get
            {
                return offsetDirection * maxOffsetSize * offsetAmount;
            }
        }


        private Vector3 FrontOffset
        {
            get
            {
                return (destination - origin).normalized * maxOffsetSize * offsetAmount;
            }
        }

        public void AdjustOffset(float amount, float maxSize)
        {
            maxOffsetSize = maxSize;
            offsetAmount = amount;
            //lineRenderer.material = mat;
            Draw();
        }

        private float time = 0.6f;

        public IEnumerator DrawCoroutine()
        {
            float t = 0;
            transform.position = Origin;
            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Origin);

            for (; t < time; t += Time.deltaTime)
            {
                Current = Vector3.Lerp(Origin, Destination, t / time);
                lineRenderer.SetPosition(1, Current);
                yield return null;
            }

            lineRenderer.SetPosition(1, Destination);
            yield return null;
        }

        public void Draw()
        {
            transform.position = Origin;
            lineRenderer.material = material;
            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Destination);
            Current = Destination;
        }

        public Segment Create(
            Transform transform,
            Vector2Int lorigin, // keep track of origin to remove from layout
            Vector3 origin, 
            Vector3 destination,
            Vector3 offsetDirection,
            Material material,
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
            segm.origin = origin;
            segm.destination = destination;
            segm.time = time;
            segm.lineRenderer.material = material;
            segm.lineRenderer.widthMultiplier = width;
            segm.offsetDirection = offsetDirection;
            segm.material = material;

            return segm;
        }
    }
}