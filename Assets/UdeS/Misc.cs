using UnityEngine;
using System.Collections;

namespace UdeS
{
    public delegate void OnEvent();

    public delegate void OnFloatEvent(float value);

    public delegate void OnIntEvent(int value);

    public delegate void OnStringEvent(string value);

    public delegate void OnBoolEvent(bool value);


    public static class GameObjectExtension
    {

        public static T Create<T>(this GameObject gobj, Vector3 position, Transform parent)
        {
            return Object.Instantiate(gobj, position, Quaternion.identity, parent).GetComponent<T>();
        }

        public static T Create<T>(this T bhvr, Vector3 position, Transform parent) where T : MonoBehaviour
        {
            return Object.Instantiate(bhvr.gameObject, position, Quaternion.identity, parent).GetComponent<T>();
        }

        public static T Create<T>(this T bhvr) where T : MonoBehaviour
        {
            return Object.Instantiate(bhvr.gameObject).GetComponent<T>();
        }

        public static T Create<T>(this T bhvr, Vector3 position) where T : MonoBehaviour
        {
            return Object.Instantiate(bhvr.gameObject, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Create<T>(this GameObject gobj, Transform parent)
        {
            var obj = Object.Instantiate(gobj, parent);
            obj.transform.localPosition = Vector3.zero;            
            return obj.GetComponent<T>();            
        }

        public static T Create<T>(this T bhvr, Transform parent) where T : MonoBehaviour
        {
            var obj = Object.Instantiate(bhvr.gameObject, parent);
            obj.transform.position = Vector3.zero;
            obj.transform.localPosition = Vector3.zero;
            return obj.GetComponent<T>();
        }
    }

}