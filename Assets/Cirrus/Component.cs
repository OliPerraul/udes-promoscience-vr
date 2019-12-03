using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Cirrus
{
    public class Component : MonoBehaviour
    {
        public Event<Collider> OnTriggerEnterHandler;

        public Event<Collider> OnTriggerExitHandler;

        public Event<Collider> OnTriggerStayHandler;

        public Event<Collision> OnCollisionEnterHandler;

        public Event<Collision> OnCollisionExitHandler;

        public Event<Collision> OnCollisionStayHandler;

        public Event OnAwakeHandler;

        public Event OnStartHandler;

        public Event OnEnableHandler;

        public Event OnDisableHandler;

        public Event OnValidateHandler;

        public Event<Scene, LoadSceneMode> OnSceneLoadedHandler;

        public virtual void Awake()
        {
            OnAwakeHandler?.Invoke();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoadedHandler?.Invoke(scene, mode);
        }

        public virtual void Start()
        {
            OnStartHandler?.Invoke();
        }

        public virtual void OnEnable()
        {
            OnEnableHandler?.Invoke();
        }

        public virtual void OnDisable()
        {
            OnDisableHandler?.Invoke();
        }


        public virtual void OnValidate()
        {
            OnValidateHandler?.Invoke();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            OnTriggerEnterHandler?.Invoke(collider);
        }

        public virtual void OnTriggerStay(Collider collider)
        {
            OnTriggerStayHandler?.Invoke(collider);
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            OnTriggerExitHandler?.Invoke(collider);
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterHandler?.Invoke(collision);
        }

        public virtual void OnCollisionStay(Collision collision)
        {
            OnCollisionStayHandler?.Invoke(collision);
        }

        public virtual void OnCollisionExit(Collision collision)
        {
            OnCollisionExitHandler?.Invoke(collision);
        }

    }
}
