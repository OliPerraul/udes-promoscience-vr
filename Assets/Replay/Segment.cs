using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Replay
{
    public class Segment : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField]
        private LineRenderer lineRenderer;

        public void OnValidate()
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
        }

        public Vector3 Origin;

        public Vector3 Current;

        public Vector3 Destination;

        private float time = 0.6f;

        public IEnumerator DrawCoroutine()
        {
            float t = 0;
            transform.position = Origin;
            lineRenderer.SetPosition(0, Origin + Vector3.up);
            lineRenderer.SetPosition(1, Origin+ Vector3.up);

            for (; t < time; t += Time.deltaTime)
            {
                Current = Vector3.Lerp(Origin + Vector3.up, Destination+ Vector3.up, t / time);
                lineRenderer.SetPosition(1, Current);
                yield return null;
            }

            lineRenderer.SetPosition(1, Destination + Vector3.up);
            yield return null;
        }

        public void Draw()
        {
            transform.position = Origin;
            lineRenderer.SetPosition(0, Origin+ Vector3.up);
            lineRenderer.SetPosition(1, Destination + Vector3.up);
        }

        public Segment Create(
            Transform transform, 
            Vector3 origin, 
            Vector3 destination,
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

            segm.Origin = origin;
            segm.Destination = destination;
            segm.time = time;
            segm.lineRenderer.material = material;
            segm.lineRenderer.widthMultiplier = width;

            //segm.sprite.color = material.color;
            return segm;
        }
    }
}