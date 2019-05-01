namespace BoBo.Light.Utility
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class UtilityHelper
    {
        public static void CheckParentAndSame(Transform child, Transform parent)
        {
            if (child.parent == parent)
                return;
            child.parent = parent;
            child.transform.localPosition = Vector3.zero;
            child.transform.localEulerAngles = Vector3.zero;
        }

        public static void CopyTransform(Transform dest, Transform src, bool copySize = false)
        {
            dest.position = src.position;
            dest.rotation = src.rotation;
            if (copySize)
                dest.localScale = src.localScale;
        }

        public static void SetGameobjectLayer(Transform destination, int layer)
        {
            Transform[] childTransArray = destination.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childTransArray.Length; ++i)
            {
                childTransArray[i].gameObject.layer = layer;
            }
        }
    }
}
