using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class APIExtension
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (null == component)
            component = obj.AddComponent<T>();

        return component;
    }

    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        T target = component.GetComponent<T>();
        if (null == target)
            target = component.gameObject.AddComponent<T>();
        return target;
    }


    public static bool MatchTag(this Transform trans, string[] tagNames)
    {
        if (null == tagNames)
            return false;
        for (int i = 0; i < tagNames.Length; ++i)
        {
             if (trans.CompareTag(tagNames[i]))
                return true;
        }
        return false;
    }
}

