namespace BoBo.Light.Utility
{
    using UnityEngine;
    using System.Collections;

    public class RayUtility : MonoBehaviour
    {
        public static bool RayToFindHit(Ray ray,
            string tagName, int layerMask, out RaycastHit hit, float distance = 5f)
        {
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                if (hit.transform.CompareTag(tagName))
                    return true;
            }
            return false;
        }

        public static bool RayToFindHit(Ray ray, string tagName, out RaycastHit hit, float distance = 5f)
        {
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.CompareTag(tagName))
                    return true;
            }
            return false;
        }

        public static bool RayToFindPosition(Ray ray, string tagName, out Vector3 pos, float distance = 5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.CompareTag(tagName))
                {
                    pos = hit.point;
                    return true;
                }
            }
            pos = Vector3.zero;
            return false;
        }

        public static Transform RayToFindTransfrom(Ray ray, string tagName, float distance = 3f)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.CompareTag(tagName))
                    return hit.transform;
            }
            return null;
        }

        public static Transform RayToFindTransfrom(Ray ray, float distance, int layermask,
            out float nearDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layermask))
            {
                nearDistance = hit.distance;
                return hit.transform;
            }
            nearDistance = -1f;
            return null;
        }

        public static Transform RayToFindTransfrom(Ray ray, float distance, out float nearDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                nearDistance = hit.distance;
                return hit.transform;
            }
            nearDistance = -1f;
            return null;
        }

        public static Transform RayToFindTransfrom(Ray ray, float distance = 3f)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                return hit.transform;
            }
            return null;
        }
    }
}