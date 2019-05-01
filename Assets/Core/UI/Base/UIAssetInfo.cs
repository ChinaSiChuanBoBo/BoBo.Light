namespace BoBo.Light.UI
{
    using UnityEngine;
    using System.Collections;

    public class UIAssetInfo : ScriptableObject
    {
        public string uiID;

        public float maxActiveTime=180;

        public float defaultActiveTime=60;

        public string loadMode = "Resources";

        public Vector3 anchoredPosition3D;

        public Vector2 sizeDelta;
    }
}
