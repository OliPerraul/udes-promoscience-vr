using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Playbacks
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
            lineRenderer.SetPosition(0, Origin);
            lineRenderer.SetPosition(1, Destination);
        }

        public Segment Create(
            Transform transform, 
            Vector3 origin, 
            Vector3 destination,
            Material material, 
            float time, 
            float width, 
            bool backtrack)
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
            segm.lineRenderer.enabled = !backtrack;
            segm.lineRenderer.widthMultiplier = width;
            segm.lineRenderer.material = material;
            segm.sprite.color = material.color;

            return segm;
        }

        public void Enable(bool enable=true)
        {
            lineRenderer.enabled = enable;
            sprite.enabled = enable;
        }

    }
}