using UnityEngine;
using System.Collections;

namespace Cirrus.Extensions
{

    public static class GameObjectExtension
    {
        public static void Destroy(this GameObject gobj)
        {
            Object.Destroy(gobj);
        }

        public static void DestroyImmediate(this GameObject gobj)
        {
            Object.DestroyImmediate(gobj);
        }

        public static GameObject Create(this GameObject gobj, Vector3 position, Transform parent)
        {
            return Object.Instantiate(gobj, position, Quaternion.identity, parent);
        }

        public static GameObject Create(this GameObject gobj)
        {
            return Object.Instantiate(gobj);
        }

        public static GameObject Create(this GameObject gobj, Transform parent)
        {
            return Object.Instantiate(gobj, parent);
        }

        public static T Create<T>(this GameObject gobj, Vector3 position, Transform parent)
        {
            return Object.Instantiate(gobj, position, Quaternion.identity, parent).GetComponent<T>();
        }

        public static T Create<T>(this T gobj, Vector3 position) where T : MonoBehaviour
        {
            return Object.Instantiate(gobj.gameObject, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Create<T>(this T behv, Vector3 position, Transform parent) where T : MonoBehaviour
        {
            return Object.Instantiate(behv.gameObject, position, Quaternion.identity, parent).GetComponent<T>();
        }

        public static T Create<T>(this T behv, Transform parent) where T : MonoBehaviour
        {
            return Object.Instantiate(behv.gameObject, parent).GetComponent<T>();
        }

        public static T Create<T>(this T behv) where T : MonoBehaviour
        {
            return Object.Instantiate(behv.gameObject).GetComponent<T>();
        }
    }
}